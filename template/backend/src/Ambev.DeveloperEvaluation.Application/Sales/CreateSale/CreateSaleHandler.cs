using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of CreateSaleHandler.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="saleRepository">The sale repository.</param>
        public CreateSaleHandler(IMapper mapper, ISaleRepository saleRepository, 
            IProductRepository productRepository, IUserRepository userRepository, IMediator mediator)
        {
            _mapper = mapper;
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
            _mediator = mediator;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var user = await _userRepository.GetByIdAsync(command.CustomerId, cancellationToken);
            if (user == null)
            {
                throw new ValidationException($"User with ID '{command.CustomerId}' does not exist.");
            }

            if (!user.Role.Equals(UserRole.Customer))
            {
                throw new ValidationException($"User with ID '{command.CustomerId}' is not a customer.");
            }

            var sale = new Sale
            {
                CustomerName = user.Username,
                CustomerId = command.CustomerId,
                Branch = command.Branch,
                Date = DateTime.UtcNow
            };

            var items = command.Items.ToList();
            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
                if (product == null)
                {
                    throw new ValidationException($"Product with ID '{item.ProductId}' does not exist.");
                }

                sale.AddItem(item.ProductId, item.Quantity, product.UnitPrice, product.Name);
            }

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            await _mediator.Publish(new SaleCreatedEvent(sale.Id, DateTime.UtcNow), cancellationToken);

            var result = _mapper.Map<CreateSaleResult>(createdSale);
            return result;
        }
    }
}
