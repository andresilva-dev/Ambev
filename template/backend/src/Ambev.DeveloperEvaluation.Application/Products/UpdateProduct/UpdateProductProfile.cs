using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
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
        }
    }
}
