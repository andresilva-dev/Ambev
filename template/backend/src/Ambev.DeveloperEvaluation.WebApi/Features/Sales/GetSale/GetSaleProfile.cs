using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Profile for mapping GetSale feature requests to commands
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetSale feature
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Guid, GetSaleCommand>()
                .ConstructUsing(id => new GetSaleCommand(id));

            CreateMap<GetSaleResult, GetSaleResponse>().ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName));
            CreateMap<GetSaleItemResult, GetSaleItemResponse>().ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName));
            CreateMap<Sale, GetSaleResult>().ReverseMap();
        }
    }
}
