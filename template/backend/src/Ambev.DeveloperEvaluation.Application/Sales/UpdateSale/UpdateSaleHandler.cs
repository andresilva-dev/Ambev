using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System.Runtime.CompilerServices;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing UpdateSaleCommand requests.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler.
        /// </summary>
        public UpdateSaleHandler(IMapper mapper, ISaleRepository saleRepository,
            IProductRepository productRepository, IUserRepository userRepository, IMediator mediator)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mediator = mediator;
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
            sale.Items.Clear();

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) 
                {
                    throw new ValidationException($"Product with ID '{item.ProductId}' does not exist.");
                }
                sale.AddItem(product.Id, item.Quantity, product.UnitPrice, product.Name);
            }

            sale.Cancel(command.Cancelled);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
            await _mediator.Publish(new SaleModifiedEvent(sale.Id, DateTime.UtcNow), cancellationToken);

            if (command.Cancelled)
            {
                foreach (var item in sale.Items)
                {
                    await _mediator.Publish(new ItemCancelledEvent(sale.Id, item.Id), cancellationToken);
                }

                await _mediator.Publish(new SaleCancelledEvent(sale.Id), cancellationToken);
            }
            
            var result = _mapper.Map<UpdateSaleResult>(sale);
            return result;
        }
    }
}
