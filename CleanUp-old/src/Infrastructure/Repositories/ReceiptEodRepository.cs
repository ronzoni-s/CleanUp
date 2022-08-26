using CleanUp.Application.Interfaces.Repositories;
using CleanUp.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Repositories
{
    public class ReceiptEodRepository : IReceiptEodRepository
    {
        private readonly IRepositoryAsync<ReceiptEod, string> _repository;

        public ReceiptEodRepository(IRepositoryAsync<ReceiptEod, string> repository)
        {
            _repository = repository;
        }
    }
}