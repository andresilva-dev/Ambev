using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    /// <summary>
    /// Profile for mapping between CancelSaleItemCommand and domain entities.
    /// </summary>
    public class CancelSaleItemProfile : Profile
    {
        public CancelSaleItemProfile()
        {
            CreateMap<CancelSaleItemCommand, SaleItem>();
            CreateMap<SaleItem, CancelSaleItemResult>().ReverseMap();
        }
    }
}
