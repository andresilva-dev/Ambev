using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer
{
    /// <summary>
    /// Profile for mapping between Customer entity and UpdateCustomerResult.
    /// </summary>
    public class UpdateCustomerProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for UpdateCustomer operation.
        /// </summary>
        public UpdateCustomerProfile()
        {
            CreateMap<Customer, UpdateCustomerResult>();
        }
    }
}
