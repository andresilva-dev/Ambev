using Ambev.DeveloperEvaluation.Application.Customers.CreateCustomer;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateCustomerHandler"/> class.
    /// </summary>
    public class CreateCustomerHandlerTests
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly CreateCustomerHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCustomerHandlerTests"/> class.
        /// </summary>
        public CreateCustomerHandlerTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateCustomerHandler(_customerRepository, _mapper);
        }

        /// <summary>
        /// Tests that a valid customer creation request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid customer data When creating customer Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {

            var command = CreateCustomerHandlerTestData.GenerateValidCommand();
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Cpf = command.Cpf,
                Email = command.Email,
                Status = command.Status,
                CreatedAt = DateTime.UtcNow
            };
            var result = new CreateCustomerResult { Id = customer.Id };

            _customerRepository.GetByCpfAsync(command.Cpf, Arg.Any<CancellationToken>()).Returns((Customer)null);
            _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((Customer)null);

            _mapper.Map<Customer>(command).Returns(customer);
            _mapper.Map<CreateCustomerResult>(customer).Returns(result);
            _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>())
                .Returns(customer);

            var createCustomerResult = await _handler.Handle(command, CancellationToken.None);

            createCustomerResult.Should().NotBeNull();
            createCustomerResult.Id.Should().Be(customer.Id);
            await _customerRepository.Received(1).CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid customer creation request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid customer data When creating customer Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            var command = CreateCustomerHandlerTestData.GenerateInvalidCommand();

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        /// <summary>
        /// Tests that the handler throws an exception if customer CPF already exists.
        /// </summary>
        [Fact(DisplayName = "Given existing CPF When creating customer Then throws invalid operation exception")]
        public async Task Handle_DuplicateCpf_ThrowsInvalidOperationException()
        {
            var command = CreateCustomerHandlerTestData.GenerateValidCommand();
            var existingCustomer = new Customer { Id = Guid.NewGuid(), Cpf = command.Cpf };

            _customerRepository.GetByCpfAsync(command.Cpf, Arg.Any<CancellationToken>())
                .Returns(existingCustomer);

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Customer with CPF {command.Cpf} already exists");
        }

        /// <summary>
        /// Tests that the handler throws an exception if customer Email already exists.
        /// </summary>
        [Fact(DisplayName = "Given existing Email When creating customer Then throws invalid operation exception")]
        public async Task Handle_DuplicateEmail_ThrowsInvalidOperationException()
        {
            var command = CreateCustomerHandlerTestData.GenerateValidCommand();
            var existingCustomer = new Customer { Id = Guid.NewGuid(), Email = command.Email };

            _customerRepository.GetByCpfAsync(command.Cpf, Arg.Any<CancellationToken>())
                .Returns((Customer)null);
            _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(existingCustomer);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage($"Customer with Email {command.Email} already exists");
        }

        /// <summary>
        /// Tests that the mapper is called with the correct command.
        /// </summary>
        [Fact(DisplayName = "Given valid command When handling Then maps command to customer entity")]
        public async Task Handle_ValidRequest_MapsCommandToCustomer()
        {
            var command = CreateCustomerHandlerTestData.GenerateValidCommand();
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Cpf = command.Cpf,
                Email = command.Email,
                Status = command.Status,
                CreatedAt = DateTime.UtcNow
            };

            _customerRepository.GetByCpfAsync(command.Cpf, Arg.Any<CancellationToken>()).Returns((Customer)null);
            _customerRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((Customer)null);
            _mapper.Map<Customer>(command).Returns(customer);
            _mapper.Map<CreateCustomerResult>(customer).Returns(new CreateCustomerResult { Id = customer.Id });
            _customerRepository.CreateAsync(Arg.Any<Customer>(), Arg.Any<CancellationToken>()).Returns(customer);

            await _handler.Handle(command, CancellationToken.None);

            _mapper.Received(1).Map<Customer>(Arg.Is<CreateCustomerCommand>(c =>
                c.Name == command.Name &&
                c.Cpf == command.Cpf &&
                c.Email == command.Email &&
                c.Status == command.Status));
        }
    }
}
