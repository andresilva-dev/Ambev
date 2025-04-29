using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers
{
    public class GetCustomersHandler : IRequestHandler<GetCustomersCommand, IQueryable<GetCustomerResult>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetCustomersHandler
        /// </summary>
        /// <param name="customerRepository">The customer repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetCustomersCommand request
        /// </summary>
        /// <param name="request">The GetCustomers command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An IQueryable of mapped GetCustomerResult</returns>
        public async Task<IQueryable<GetCustomerResult>> Handle(GetCustomersCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetCustomersCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var query = _customerRepository.GetAllAsQueryable();

            return query.ProjectTo<GetCustomerResult>(_mapper.ConfigurationProvider);
        }
    }
}
