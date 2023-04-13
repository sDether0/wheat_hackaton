using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheat.Models.Entities;

namespace Wheat.Models.Responses
{
    public class ContractsResult
    {
        public double Balance => MySellContracts.Balance - MyBuyContracts.Balance;
        public int Count => - MySellContracts.Count + MyBuyContracts.Count;
        
        public Contracts MySellContracts { get; set; }
        public Contracts MyBuyContracts { get; set; }
    }

    public class Contracts
    {
        public string SelfCode { get; set; }
        public double Balance { get; set; }
        public int Count { get; set; }
        public List<PriceRow> PriceRow { get; set; }
    }
    
    public class ContractPerson
    {
        public string Code { get; set; }
        public List<ContractPrice> Prices { get; set; }
    }

    public class ContractPrice
    {
        public int Count { get; set; }
        public SellContract Contract { get; set; }
    }

    public class PriceRow
    {
        public int Count { get; set; }
        public string Code { get; set; }
        public SellContract Contract { get; set; }
    }
}
