using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer
{
    /// <summary>
    /// Handler for processing UpdateCustomerCommand requests
    /// </summary>
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateCustomerHandler
        /// </summary>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="customerRepository">The customer repository</param>
        public UpdateCustomerHandler(IMapper mapper, ICustomerRepository customerRepository)
        {
            _mapper = mapper;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Handles the UpdateCustomerCommand request
        /// </summary>
        /// <param name="command">The update customer command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result of the update operation</returns>
        public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateCustomerCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingCustomer = await _customerRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingCustomer == null)
                throw new KeyNotFoundException($"Customer with ID {command.Id} not found");

            existingCustomer.Name = command.Name;
            existingCustomer.Email = command.Email;
            existingCustomer.Status = command.Status;
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer, cancellationToken);

            return _mapper.Map<UpdateCustomerResult>(updatedCustomer);
        }
    }
}
