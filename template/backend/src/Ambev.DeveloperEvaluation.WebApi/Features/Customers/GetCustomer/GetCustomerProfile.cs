using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer
{
    /// <summary>
    /// Profile for mapping GetCustomer feature requests to commands
    /// </summary>
    public class GetCustomerProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetCustomer feature
        /// </summary>
        public GetCustomerProfile()
        {
            CreateMap<Guid, GetCustomerCommand>()
                .ConstructUsing(id => new GetCustomerCommand(id));
        }
    }
}
