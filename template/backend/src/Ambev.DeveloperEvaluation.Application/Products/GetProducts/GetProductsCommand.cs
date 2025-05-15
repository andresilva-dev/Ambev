using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    /// <summary>
    /// Command for retrieving a paginated list of products.
    /// </summary>
    /// <remarks>
    /// This command encapsulates pagination parameters to request a list of products.
    /// Validation is applied using <see cref="GetProductsCommandValidator"/> to ensure valid page parameters.
    /// </remarks>
    public class GetProductsCommand : IRequest<GetProductsResult>
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
        /// Initializes a new instance of the <see cref="GetProductsCommand"/> class.
        /// </summary>
        /// <param name="pageNumber">The current page number</param>
        /// <param name="pageSize">The number of items per page</param>
        public GetProductsCommand(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public ValidationResultDetail Validate()
        {
            var validator = new GetProductsCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}