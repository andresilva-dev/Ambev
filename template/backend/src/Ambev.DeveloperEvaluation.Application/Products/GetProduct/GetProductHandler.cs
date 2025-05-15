using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        /// <summary>
        /// Initializes a new instance of GetProductHandler
        /// </summary>
        /// <param name="productRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for GetProductCommand</param>
        public GetProductHandler(
            IProductRepository productRepository,
            IMapper mapper,
            ICacheService cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// Handles the GetProductCommand request
        /// </summary>
        /// <param name="request">The GetProduct command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The product details if found</returns>
        public async Task<GetProductResult> Handle(GetProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetProductCommandValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var cached = await _cache.GetAsync<Product>(request.Id.ToString());
            if (cached != null) 
                return _mapper.Map<GetProductResult>(cached);

            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {request.Id} not found");

            return _mapper.Map<GetProductResult>(product);
        }
    }

}
