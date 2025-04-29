using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing UpdateSaleCommand requests.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler.
        /// </summary>
        public UpdateSaleHandler(IMapper mapper, ISaleRepository saleRepository,
            IProductRepository productRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
            {
                throw new ValidationException($"Sale with ID '{command.Id}' does not exist.");
            }

            var user = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (user == null)
            {
                throw new ValidationException($"User with ID '{command.CustomerId}' does not exist.");
            }

            sale.CustomerId = command.CustomerId;
            sale.Branch = command.Branch;

            if (command.Cancelled)
            {
                sale.Cancel();
            }

            await _saleRepository.DeleteItemsAsync(command.Id, cancellationToken);

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product == null)
                {
                    throw new ValidationException($"Product with ID '{item.ProductId}' does not exist.");
                }

                sale.AddItem(item.ProductId, product.Name, item.Quantity, product.UnitPrice);
            }

            await _saleRepository.UpdateAsync(sale, cancellationToken);
            var result = _mapper.Map<UpdateSaleResult>(sale);
            return result;
        }
    }
}
