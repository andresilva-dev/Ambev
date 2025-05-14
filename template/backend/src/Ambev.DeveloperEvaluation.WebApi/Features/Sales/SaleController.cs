using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelOrRestoreSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    /// <summary>
    /// Controller for managing sales operations
    /// </summary>
    [ApiController]
    [Authorize(Roles = "Customer")]
    [Route("api/[controller]")]
    public class SaleController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of SalesController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public SaleController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<CreateSaleCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);

                return Created(
                    routeName: nameof(GetSaleById),
                    routeValues: new { id = result.Id },
                    data: _mapper.Map<CreateSaleResponse>(result)
                );
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Unexpected error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a sale by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale details if found</returns>
        [HttpGet("{id}", Name = "GetSaleById")]
        [Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSaleById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetSaleRequest { Id = id };
                var validator = new GetSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<GetSaleCommand>(request.Id);
                var result = await _mediator.Send(command, cancellationToken);

                if (result == null)
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = $"Sale with ID {id} not found"
                    });

                return Ok(_mapper.Map<GetSaleResponse>(result), "Sale retrieved successfully");
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Unexpected error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Retrieves a paginated list of sales
        /// </summary>
        /// <param name="pageNumber">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of sales</returns>
        [HttpGet]
        //[Authorize(Roles = "Admin,Manager")]
        [ProducesResponseType(typeof(PaginatedResponse<GetSaleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSales([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new GetSalesCommand(pageNumber, pageSize);
                var result = await _mediator.Send(command, cancellationToken);

                var response = await PaginatedList<GetSaleResult>.CreateAsync(result, pageNumber, pageSize);

                return OkPaginated(response);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Unexpected error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Updates an existing sale
        /// </summary>
        /// <param name="id">The ID of the sale to update</param>
        /// <param name="request">The updated sale data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale details</returns>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Manager,Customer")]
        public async Task<IActionResult> UpdateSale([FromBody] UpdateSaleRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateSaleRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<UpdateSaleCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<UpdateSaleResponse>
                {
                    Success = true,
                    Message = "Sale updated successfully",
                    Data = _mapper.Map<UpdateSaleResponse>(result)
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Unexpected error: {ex.Message}" });
            }
        }

        /// <summary>
        /// Cancels or restores cancellation of a sale item
        /// </summary>
        /// <param name="request">The request containing item ID and cancel flag</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Status of the operation</returns>
        [HttpPut("items/cancel")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,Manager,Customer")]
        public async Task<IActionResult> CancelOrRestoreSaleItem([FromBody] CancelSaleItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CancelSaleItemRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<CancelSaleItemCommand>(request);
                await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponse
                {
                    Success = true
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Success = false, Message = $"Unexpected error: {ex.Message}" });
            }

        }

    }
}
