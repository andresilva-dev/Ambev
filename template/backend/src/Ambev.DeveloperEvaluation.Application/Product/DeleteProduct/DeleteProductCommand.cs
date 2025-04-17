using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Product.DeleteProduct
{
    public class DeleteProductCommand : IRequest<DeleteProductResponse>
    {
        /// <summary>
        /// The unique identifier of the product to delete
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Initializes a new instance of DeleteProductCommand
        /// </summary>
        /// <param name="id">The ID of the product to delete</param>
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }
}
