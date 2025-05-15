using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    /// <summary>
    /// Handler for processing CreateProductHandler requests
    /// </summary>
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        /// <summary>
        /// Initializes a new instance of CreateProductHandler
        /// </summary>
        /// <param name="userRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public CreateProductHandler(IMapper mapper, IProductRepository productRepository, ICacheService cache)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateProductCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var product = _mapper.Map<Product>(command);
            product.CreatedAt = DateTime.UtcNow;

            var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

            await _cache.SetAsync(createdProduct.Id.ToString(), createdProduct);

            var result = _mapper.Map<CreateProductResult>(createdProduct);
            return result;
        }
    }
}
