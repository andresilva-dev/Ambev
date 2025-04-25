using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesCommand, IQueryable<GetSaleResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetSalesHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetSalesCommand request
        /// </summary>
        /// <param name="request">The GetSales command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An IQueryable of mapped GetSaleResult</returns>
        public async Task<IQueryable<GetSaleResult>> Handle(GetSalesCommand request, CancellationToken cancellationToken)
        {
            var validator = new GetSalesCommandValidator();
            var validationResult = validator.Validate(request);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var query = _saleRepository.GetAllAsQueryable();

            return query.ProjectTo<GetSaleResult>(_mapper.ConfigurationProvider);
        }
    }
}
