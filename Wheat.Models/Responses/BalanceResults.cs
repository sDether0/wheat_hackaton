using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheat.Models.Entities;

namespace Wheat.Models.Responses
{
    public class BalanceResults
    {
        public double Total { get; set; }
        public int Count { get; set; }
        public List<OperationResultDto> OperationResultsDto { get; set; }
    }
}
