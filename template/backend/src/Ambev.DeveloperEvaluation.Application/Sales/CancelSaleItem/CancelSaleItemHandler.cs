using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    /// <summary>
    /// Handler for processing CancelSaleItemCommand requests.
    /// </summary>
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResult>
    {
        private readonly ISaleItemRepository _saleItemRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CancelSaleItemHandler.
        /// </summary>
        /// <param name="mapper">The AutoMapper instance.</param>
        /// <param name="saleRepository">The sale repository.</param>
        public CancelSaleItemHandler(IMapper mapper, ISaleItemRepository saleItemRepository)
        {
            _mapper = mapper;
            _saleItemRepository = saleItemRepository;
        }

        public async Task<CancelSaleItemResult> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleItemCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var saleItem = await _saleItemRepository.GetByIdAsync(command.Id, cancellationToken);
            if (saleItem == null)
            {
                throw new ValidationException($"SaleItem with ID '{command.Id}' not found.");
            }

            saleItem.Cancel(command.Cancelled);

            await _saleItemRepository.UpdateAsync(saleItem, cancellationToken);

            var result = _mapper.Map<CancelSaleItemResult>(saleItem);
            return result;
        }
    }
}
