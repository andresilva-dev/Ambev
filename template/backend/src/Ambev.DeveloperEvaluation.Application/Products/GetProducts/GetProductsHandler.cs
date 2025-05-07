using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache;

        /// <summary>
        /// Initializes a new instance of GetProductsHandler
        /// </summary>
        /// <param name="productRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetProductsHandler(IProductRepository productRepository, IMapper mapper, ICacheService cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _cache = cache;
        }

        /// <summary>
        /// Handles the GetProductsCommand request
        /// </summary>
        /// <param name="request">The GetProducts command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An IEnumerable of mapped GetProductResult</returns>
        public async Task<GetProductsResult> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetProductsCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var products = await _cache.GetAllAsync<Product>();

            if (!products.Any())
            {
                products = _productRepository.GetAll().ToList();
            }

            var result = products.Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var productsResult = new GetProductsResult()
            {
                Items = _mapper.Map<IEnumerable<GetProductResult>>(result),
                TotalCount = products.Count
            };

            return productsResult;
        }
    }
}
