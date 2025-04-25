using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    /// <summary>
    /// Command for retrieving a paginated list of sales.
    /// </summary>
    /// <remarks>
    /// This command encapsulates pagination parameters to request a list of sales.
    /// Validation is applied using <see cref="GetSalesCommandValidator"/> to ensure valid page parameters.
    /// </remarks>
    public class GetSalesCommand : IRequest<IQueryable<GetSaleResult>>
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
        /// Initializes a new instance of the <see cref="GetSalesCommand"/> class.
        /// </summary>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        public GetSalesCommand(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new GetSalesCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
