using AutoMapper;
using Wheat.Models.Entities;
using Wheat.Models.Responses;

namespace Wheat.Api.Models
{
    public class OperationResultProfile : Profile
    {
        public OperationResultProfile()
        {
            CreateMap<OperationResult, OperationResultDto>();
        }
    

    }
}
