using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomers
{
    /// <summary>
    /// Command for retrieving a paginated list of customers.
    /// </summary>
    /// <remarks>
    /// This command encapsulates pagination parameters to request a list of customers.
    /// Validation is applied using <see cref="GetCustomersCommandValidator"/> to ensure valid page parameters.
    /// </remarks>
    public class GetCustomersCommand : IRequest<IQueryable<GetCustomerResult>>
    {
        /// <summary>
        /// Page number (starting from 1)
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCustomersCommand"/> class.
        /// </summary>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        public GetCustomersCommand(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new GetCustomersCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }

}
