using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Unit.Application.SaleTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMediator _mediator;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediator = Substitute.For<IMediator>();
            _handler = new CreateSaleHandler(_mapper, _saleRepository, _productRepository, _userRepository, _mediator);
        }

        [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(1);
            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                Branch = command.Branch,
                Items = command.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            var result = new CreateSaleResult
            {
                Id = sale.Id,
            };

            _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
               .Returns(new User { Id = command.CustomerId,Role = UserRole.Customer });

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

            _mapper.Map<CreateSaleResult>(sale).Returns(result);
            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(sale);

            var createSaleResult = await _handler.Handle(command, CancellationToken.None);

            createSaleResult.Should().NotBeNull();
            createSaleResult.Id.Should().Be(sale.Id);
            await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            var command = new CreateSaleCommand();

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given valid sale command When handling Then creates sale correctly")]
        public async Task Handle_ValidRequest_CreatesSaleCorrectly()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(1);
            var expectedSale = new Sale
            {
                Id = Guid.NewGuid(),
                CustomerId = command.CustomerId,
                Branch = command.Branch,
                Items = command.Items.Select(i => new SaleItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
                .Returns(new User { Id = command.CustomerId, Role = UserRole.Customer});

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

            _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
                .Returns(new CreateSaleResult { Id = expectedSale.Id });

            _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
                .Returns(expectedSale);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Id.Should().Be(expectedSale.Id);
            await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(s =>
                s.CustomerId == command.CustomerId &&
                s.Branch == command.Branch &&
                s.Items.Count == command.Items.Count
            ), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given sale with more than 4 identical items When creating sale Then apply 10 percent discount")]
        public async Task Handle_SaleAbove4Items_Applies10PercentDiscount()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(quantity: 5);
            SetupUserAndProducts(command, 100m); 

            var sale = await _handler.Handle(command, CancellationToken.None);

            await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(s =>
                s.Total == 450 &&
                s.TotalDiscountsPercentage == 10m &&
                s.TotalWithoutDiscounts == 500
            ), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given sale with between 10 and 20 identical items When creating sale Then apply 20 percent discount")]
        public async Task Handle_SaleBetween10And20Items_Applies20PercentDiscount()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(quantity: 15);
            SetupUserAndProducts(command, 100m);

            var sale = await _handler.Handle(command, CancellationToken.None);

            await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(s =>
                s.Total == 1200 && 
                s.TotalWithoutDiscounts == 1500 &&
                s.TotalDiscountsPercentage == 20m
            ), Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given sale with more than 20 identical items When creating sale Then throw validation exception")]
        public async Task Handle_SaleAbove20Items_ThrowsValidationException()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(quantity: 25);
            SetupUserAndProducts(command, 100m);

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact(DisplayName = "Given sale with less than 4 identical items When creating sale Then no discount applied")]
        public async Task Handle_SaleBelow4Items_NoDiscountApplied()
        {
            var command = CreateSaleHandlerTestData.GenerateValidCommand(quantity: 3);
            SetupUserAndProducts(command, 100m);

            var sale = await _handler.Handle(command, CancellationToken.None);

            await _saleRepository.Received(1).CreateAsync(Arg.Is<Sale>(s => 
                s.Total == 300
            ), Arg.Any<CancellationToken>());
        }

        private void SetupUserAndProducts(CreateSaleCommand command, decimal unitPrice)
        {
            _userRepository.GetByIdAsync(command.CustomerId, Arg.Any<CancellationToken>())
               .Returns(new User { Id = command.CustomerId , Role = UserRole.Customer});

            foreach (var item in command.Items)
            {
                _productRepository.GetByIdAsync(item.ProductId, Arg.Any<CancellationToken>())
                    .Returns(new Product
                    {
                        Id = item.ProductId,
                        Name = $"Product {item.ProductId}",
                        UnitPrice = unitPrice
                    });
            }

            _mapper.Map<CreateSaleResult>(Arg.Any<Sale>())
                .Returns(new CreateSaleResult
                {
                    Id = Guid.NewGuid()
                });
        }
    }
}
