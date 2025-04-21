using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct
{
    /// <summary>
    /// Command for updating an existing product.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for updating a product, 
    /// including its identifier, name, description and unitPrice. 
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="UpdateProductResult"/>.
    /// 
    /// The data provided in this command is validated using the 
    /// <see cref="UpdateProductCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and follow the required rules.
    /// </remarks>
    public class UpdateProductCommand : IRequest<UpdateProductResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the product.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// Must be unique and not empty.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a short description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the current unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new UpdateProductCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
