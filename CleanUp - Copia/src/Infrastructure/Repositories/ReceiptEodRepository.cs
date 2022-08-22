using ErbertPranzi.Application.Interfaces.Repositories;
using ErbertPranzi.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ErbertPranzi.Infrastructure.Repositories
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