using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Product.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Products.Mappings
{
    /// <summary>
    /// Profile for mapping between Product entity and UpdateProductResult.
    /// </summary>
    public class UpdateProductProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for UpdateProduct operation.
        /// </summary>
        public UpdateProductProfile()
        {
            CreateMap<Product, UpdateProductResult>();
            CreateMap<UpdateProductRequest, UpdateProductCommand>();
            CreateMap<UpdateProductResult, UpdateProductResponse>().ReverseMap();
        }
    }
}
