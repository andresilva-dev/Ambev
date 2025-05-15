using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ambev.DeveloperEvaluation.Application.Products.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly ISaleRepository _saleRepository;

        /// <summary>
        /// Initializes a new instance of DeleteProductHandler
        /// </summary>
        /// <param name="productRepository">The product repository</param>
        /// <param name="validator">The validator for DeleteProductCommand</param>
        public DeleteProductHandler(IProductRepository productRepository, 
            ICacheService cacheService, ISaleRepository saleRepository)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
            _saleRepository = saleRepository;
        }

        /// <summary>
        /// Handles the DeleteProductCommand request
        /// </summary>
        /// <param name="command">The DeleteProduct command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the delete operation</returns>
        public async Task<DeleteProductResponse> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var validator = new DeleteProductValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existsSale = await _saleRepository.ExistsSaleRelatedProductAsync(command.Id, cancellationToken);
            if (existsSale)
            {
                throw new ValidationException($"There are sales related the product with id '{command.Id}'.");
            }

            await _cacheService.RemoveAsync<Product>(command.Id.ToString());

            var success = await _productRepository.DeleteAsync(command.Id, cancellationToken);
            if (!success)
                throw new KeyNotFoundException($"Product with ID {command.Id} not found");

            return new DeleteProductResponse { Success = true };
        }
    }
}
