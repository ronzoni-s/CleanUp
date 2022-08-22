using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ErbertPranzi.Infrastructure.Repositories
{
    public class ParameterRepository : IParameterRepository
    {
        private readonly IRepositoryAsync<Parameter, int> _repository;

        public ParameterRepository(IRepositoryAsync<Parameter, int> repository)
        {
            _repository = repository;
        }

        public async Task<double> GetWeightPerBag()
        {
            return _repository.Entities
                        .Where(p => p.Key == "WeightPerBag")
                        .Select(p => double.Parse(p.Value))
                        .FirstOrDefault();
        }

        public async Task<int> GetBagsPerPolibox()
        {
            return _repository.Entities
                        .Where(p => p.Key == "BagsPerPolibox")
                        .Select(p => int.Parse(p.Value))
                        .FirstOrDefault();
        }

        public async Task<List<string>> GetRecipientEmailAddresses()
        {
            var item = _repository.Entities
                        .Where(p => p.Key == "RecipientsEmailAddresses")
                        .Select(p => p.Value)
                        .FirstOrDefault();
            return item.Split(";").Where(str => !string.IsNullOrEmpty(str)).ToList();
        }
    }
}