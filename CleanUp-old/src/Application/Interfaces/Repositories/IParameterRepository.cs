using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanUp.Application.Interfaces.Repositories
{
    public interface IParameterRepository
    {
        Task<double> GetWeightPerBag();

        Task<int> GetBagsPerPolibox();

        Task<List<string>> GetRecipientEmailAddresses();
    }
}