using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ErbertPranzi.Application.Features.Emails.Commands.UpdateLastEmailDateTime;
using ErbertPranzi.Application.Features.Emails.Queries.GetById;
using ErbertPranzi.Application.Features.Orders.Commands.AddEdit;
using ErbertPranzi.Application.Interfaces.Services;
using ErbertPranzi.Application.Requests.Mail;
using ErbertPranzi.Shared.Constants.Localization;
using ErbertPranzi.Shared.Settings;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ErbertPranzi.Server.Jobs
{
    public class ImportOrdersJob
    {
        private readonly IMailService mailService;
        private readonly ILogger<ImportOrdersJob> logger;
        private readonly IMediator mediator;
        private readonly IDateTimeService dateTimeService;

        public ImportOrdersJob(IMailService mailService, ILogger<ImportOrdersJob> logger, IMediator mediator, IDateTimeService dateTimeService)
        {
            this.mailService = mailService;
            this.logger = logger;
            this.mediator = mediator;
            this.dateTimeService = dateTimeService;
        }

        [DisableConcurrentExecution(10 * 60)]
        [AutomaticRetry(Attempts = 0)]
        public async Task Execute()
        {
            logger.LogInformation("Job running at: {time}", DateTimeOffset.Now);

            var emailObjResult = await mediator.Send(new GetEmailConfigByIdQuery { Id = mailService.ImapConfigId });
            var lastEmailRead = emailObjResult.Succeeded && emailObjResult.Data.LastEmailDateTime.HasValue
                ? emailObjResult.Data.LastEmailDateTime.Value
                : DateTime.MinValue;

            var emails = mailService.ReceiveEmailWithImap(lastEmailRead);
            int orderInserted = 0;
            foreach (var email in emails)
            {
                try
                {
                    logger.LogInformation("New mail received at {time} with body: {@message}", email.Date, email.Body);
                    var values = GetValues(email.Body);

                    if (lastEmailRead < email.Date)
                    {
                        lastEmailRead = email.Date;
                    }

                    var orderNumber = int.Parse(values["NOrdine"].First());
                    var customerName = HttpUtility.HtmlDecode(values["Azienda"].First());
                    var customerAddress = HttpUtility.HtmlDecode(values["AziendaIndirizzo"].First());
                    var contactName = HttpUtility.HtmlDecode(values["NomeCliente"].First());

                    var contactPhoneNumber = values["TelefonoCliente"].First();
                    if (contactPhoneNumber.StartsWith('+'))
                        contactPhoneNumber = contactPhoneNumber[1..];

                    //logger.LogDebug("New menu: {menu}", menu);

                    var command = new AddOrderCommand
                    {
                        OrderDate = DateTime.TryParseExact(values["Data"].First(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var orderDate)
                        ? orderDate
                        : DateTime.ParseExact(values["Data"].First(), "yyyyMMdd", CultureInfo.InvariantCulture),
                        OrderNumber = orderNumber,
                        CustomerName = customerName,
                        CustomerAddress = customerAddress,
                        ContactName = contactName,
                        ContactPhoneNumber = contactPhoneNumber,
                    };

                    logger.LogInformation("New order: {@order}", command);

                    try
                    {
                        var orderProducts = new List<AddOrderProductCommand>();
                        var orderMenus = new List<AddOrderMenuCommand>();

                        if (values.ContainsKey("Menu"))
                        {
                            var menus = values["Menu"];
                            foreach (var menu in menus)
                            {
                                logger.LogDebug("New menu (raw): {menu}", menu);

                                // {Menu#1 - prc:1000}
                                var fields = menu.Split('-', StringSplitOptions.TrimEntries).ToArray();
                                var orderMenu = new AddOrderMenuCommand
                                {
                                    Id = int.Parse(fields[0].Trim()),
                                    Price = !fields[1].Contains("prc:", StringComparison.InvariantCultureIgnoreCase) ? 0 : int.Parse(fields[1]["prc:".Length..]),
                                };

                                orderMenus.Add(orderMenu);

                                logger.LogInformation("New menu: {@menu}", orderMenu);
                            }
                        }
                        var products = values["Prodotto"];
                        foreach (var product in products)
                        {
                            logger.LogDebug("New product (raw): {product}", product);

                            // {Prodotto#NUOVO1 - Pasta - qty:1 - tax:22 - vol:0,5 - prc:800 - menu:1}
                            var fields = product.Split('-', StringSplitOptions.TrimEntries).ToArray();

                            int index = 2;
                            int quantity = !fields[index].Contains("qty:", StringComparison.InvariantCultureIgnoreCase) ? 1 : int.Parse(fields[index++]["qty:".Length..]);
                            int tax = !fields[index].Contains("tax:", StringComparison.InvariantCultureIgnoreCase) ? 0 : int.Parse(fields[index++]["tax:".Length..]);
                            double weight = !fields[index].Contains("vol:", StringComparison.InvariantCultureIgnoreCase) ? 0 : double.Parse(fields[index++]["vol:".Length..], CultureInfo.InvariantCulture);
                            int price = !fields[index].Contains("prc:", StringComparison.InvariantCultureIgnoreCase) ? 0 : int.Parse(fields[index++]["prc:".Length..]);
                            int? menu = (!fields[index].Contains("menu:", StringComparison.InvariantCultureIgnoreCase) || !int.TryParse(fields[index++]["menu:".Length..], out var menuId)) ? null : menuId;

                            if (weight == 0)
                                weight = 0.5;

                            var orderProduct = new AddOrderProductCommand
                            {
                                ProductCode = fields[0],
                                Name = fields[1],
                                Quantity = quantity,
                                Tax = tax,
                                Weight = weight,
                                Price = price,
                                MenuId = menu
                            };

                            orderProducts.Add(orderProduct);

                            logger.LogInformation("New product: {@product}", orderProduct);
                        }

                        command.OrderMenus = orderMenus;
                        command.OrderProducts = orderProducts;
                        var addOrderCommandResult = await mediator.Send(command);
                        if (!addOrderCommandResult.Succeeded)
                        {
                            if (addOrderCommandResult.ErrorCode != "OrderAlreadyExists")
                            {
                                logger.LogError("Order {orderNumber} not added because {@messages}", command.OrderNumber, addOrderCommandResult.Messages);
                            }
                        }
                        else
                        {
                            logger.LogInformation("Order {orderNumber} successfully inserted ({orderId})", command.OrderNumber, addOrderCommandResult.Data);
                            orderInserted++;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Cannot import order {orderNumber} because \"{ex.Message}\"", ex);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error importing email");
                }

                await mediator.Send(new UpdateLastEmailDateTimeCommand { Id = mailService.ImapConfigId, LastDateTime = lastEmailRead });

                logger.LogInformation("Job terminated at: {time}, orders inserted: {orderInserted}", DateTimeOffset.Now, orderInserted);
            }
        }


        private Dictionary<string, List<string>> GetValues(string text)
        {
            var values = new Dictionary<string, List<string>>();

            int i = 0;
            while (i < text.Length)
            {
                if (text[i++] == '{')
                {
                    int start = i;
                    while (text[i++] != '#') ;
                    var key = text.Substring(start, i - start - 1).Replace(" ", "");

                    start = i;
                    while (text[i++] != '}') ;
                    var value = text.Substring(start, i - start - 1);

                    if (!values.ContainsKey(key))
                        values.Add(key, new List<string>());

                    values[key].Add(value);
                }
            }

            return values;
        }

        /// <summary>
        /// Checks if the line contains 7 fields. If not returns false, otherwise true.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>true if is a product line, otherwise false</returns>
        private bool IsProduct(string line)
        {
            var fields = line.Split('-').ToArray();
            return fields.Length == 7;
        }
    }
}