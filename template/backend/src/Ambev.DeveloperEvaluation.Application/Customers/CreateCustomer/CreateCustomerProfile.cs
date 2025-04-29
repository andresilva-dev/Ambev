using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer
{
    /// <summary>
    /// Profile for mapping between Customer entity and CreateCustomerResponse
    /// </summary>
    public class CreateCustomerProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateCustomer operation
        /// </summary>
        public CreateCustomerProfile()
        {
            CreateMap<CreateCustomerCommand, Domain.Entities.Customer>();
            CreateMap<Domain.Entities.Customer, CreateCustomerResult>();
        }
    }
}
