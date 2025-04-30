using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.DeleteCustomer;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.UpdateCustomer;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomers;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    /// <summary>
    /// Controller for managing customer operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CustomersController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public CustomersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new customer
        /// </summary>
        /// <param name="request">The customer creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created customer details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateCustomerResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateCustomerRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<CreateCustomerCommand>(request);
                var result = await _mediator.Send(command, cancellationToken);

                return Created(
                    routeName: nameof(GetCustomerById),
                    routeValues: new { id = result.Id },
                    data: _mapper.Map<CreateCustomerResponse>(result)
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
        /// Retrieves a customer by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customer details if found</returns>
        [HttpGet("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetCustomerResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new GetCustomerRequest { Id = id };
                var validator = new GetCustomerRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<GetCustomerCommand>(request.Id);
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponseWithData<GetCustomerResponse>
                {
                    Success = true,
                    Message = "Customer retrieved successfully",
                    Data = _mapper.Map<GetCustomerResponse>(response)
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
        /// Deletes a customer by their ID
        /// </summary>
        /// <param name="id">The unique identifier of the customer to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response if the customer was deleted</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteCustomerRequest { Id = id };
                var validator = new DeleteCustomerRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<DeleteCustomerCommand>(request.Id);
                await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Customer deleted successfully"
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
        /// Updates an existing customer
        /// </summary>
        /// <param name="request">The updated customer data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Success response with updated data</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                request.Id = id;

                var validator = new UpdateCustomerRequestValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    return BadRequest("Occurred errors during the validation of the requested data.", validationResult.Errors);

                var command = _mapper.Map<UpdateCustomerCommand>(request);
                var response = await _mediator.Send(command, cancellationToken);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Customer updated successfully"
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
        /// Retrieves a paginated list of customers
        /// </summary>
        /// <param name="pageNumber">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of customers</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResponse<GetCustomerResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
        {
            try
            {
                var command = new GetCustomersCommand(pageNumber, pageSize);
                var result = await _mediator.Send(command, cancellationToken);

                var response = await PaginatedList<GetCustomerResult>.CreateAsync(result, pageNumber, pageSize);

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
    }
}
