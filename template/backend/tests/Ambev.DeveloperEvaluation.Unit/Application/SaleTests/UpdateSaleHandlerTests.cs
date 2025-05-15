using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleTests
{
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _saleRepository = Substitute.For<ISaleRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediator = Substitute.For<IMediator>();

            _handler = new UpdateSaleHandler(_mapper, _saleRepository, _productRepository, _userRepository, _mediator);
        }

        [Fact(DisplayName = "Given valid command When handling update Then returns expected result")]
        public async Task Handle_ValidCommand_ReturnsUpdateSaleResult()
        {
            var command = UpdateSaleHandlerTestData.GenerateValidCommand(2);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                Branch = command.Branch,
            };

            var user = new User { Id = command.CustomerId, Role = UserRole.Customer };
            var expectedResult = new UpdateSaleResult { Id = sale.Id };

            foreach (var item in command.Items)
            {
                _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>())
                    .Returns(new Product
                    {
                        Id = item.ProductId,
                        Name = $"Product {item.ProductId}",
                        UnitPrice = 10m
                    });
            }

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
            _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(user);
            _mapper.Map<UpdateSaleResult>(sale).Returns(expectedResult);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(expectedResult.Id);
        }

        [Fact(DisplayName = "Given invalid command When handling update Then throws ValidationException")]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            var command = UpdateSaleHandlerTestData.GenerateInvalidCommand();

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given cancelled sale When handling update Then publishes cancellation events")]
        public async Task Handle_CancelledSale_PublishesCancellationEvents()
        {
            var command = UpdateSaleHandlerTestData.GenerateUpdateCommandCancelledSale(1);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                Branch = command.Branch,
            };

            foreach (var item in command.Items)
            {
                _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>())
                    .Returns(new Product
                    {
                        Id = item.ProductId,
                        Name = $"Product {item.ProductId}",
                        UnitPrice = 10m
                    });
            }

            var expectedResult = new UpdateSaleResult { Id = sale.Id };
            var user = new User { Id = command.CustomerId };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
            _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>()).Returns(user);
            _mapper.Map<UpdateSaleResult>(Arg.Any<Sale>()).Returns(new UpdateSaleResult { Id = sale.Id });

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(expectedResult.Id);
            sale.Cancelled.Should().BeTrue();
            sale.Items.All(i => i.Cancelled).Should().BeTrue(); 
        }
    }
}
