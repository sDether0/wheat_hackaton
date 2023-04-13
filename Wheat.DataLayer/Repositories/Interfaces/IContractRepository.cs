using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheat.Models.Responses;

namespace Wheat.DataLayer.Repositories.Interfaces
{
    public interface IContractRepository
    {
        Task<ContractsResult> GetMyContractsAsync(string userId);
        Task<Contracts> GetMyBuyContracts(string userId);
        Task<Contracts> GetMySellContracts(string userId);
        Task CreateContractsAsync(string userId, int count, double price, bool? sell);
        Task DealAsync(string userId, string code, int count, double price, bool? buy);
        Task DealAsync(string userId, string bCode, string sCode, int count, double price);
        Task ReSellContractsAsync(string userId, string code, int count, double price, double newPrice);
        Task<BalanceResults> GetTotalBalanceAsync(string userId);
        Task<List<ContractPrice>> GetPublicSellContracts(string userId);
        Task<List<ContractPrice>> GetPublicBuyContracts(string userId);
    }
}
