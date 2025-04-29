using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer
{
    /// <summary>
    /// Profile for mapping between Customer entity and UpdateCustomerResponse.
    /// </summary>
    public class UpdateCustomerProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for UpdateCustomer operation.
        /// </summary>
        public UpdateCustomerProfile()
        {
            CreateMap<Customer, UpdateCustomerResponse>();  
            CreateMap<UpdateCustomerRequest, UpdateCustomerCommand>();  
            CreateMap<UpdateCustomerResponse, UpdateCustomerResult>().ReverseMap();
        }
    }
}
