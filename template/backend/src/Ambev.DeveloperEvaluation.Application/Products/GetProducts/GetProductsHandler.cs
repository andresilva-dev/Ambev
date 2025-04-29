using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsCommand, IQueryable<GetProductResult>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetProductsHandler
        /// </summary>
        /// <param name="productRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetProductsCommand request
        /// </summary>
        /// <param name="request">The GetProducts command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An IQueryable of mapped GetProductResult</returns>
        public async Task<IQueryable<GetProductResult>> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetProductsCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var query = _productRepository.GetAllAsQueryable();

            return query.ProjectTo<GetProductResult>(_mapper.ConfigurationProvider);
        }
    }
}
