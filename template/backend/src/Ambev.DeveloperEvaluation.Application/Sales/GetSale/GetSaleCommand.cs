using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Command for retrieving a sale by its unique identifier.
    /// </summary>
    /// <remarks>
    /// This command encapsulates the sale ID required to fetch a sale.
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="GetSaleResult"/>.
    /// </remarks>
    public class GetSaleCommand : IRequest<GetSaleResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleCommand"/> class with the specified sale ID.
        /// </summary>
        /// <param name="id">The unique identifier of the sale to retrieve.</param>
        public GetSaleCommand(Guid id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the unique identifier of the sale to be retrieved.
        /// </summary>
        public Guid Id { get; }

        public ValidationResultDetail Validate()
        {
            var validator = new GetSaleCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
