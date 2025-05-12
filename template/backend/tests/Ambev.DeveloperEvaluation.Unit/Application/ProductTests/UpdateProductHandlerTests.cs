using Ambev.DeveloperEvaluation.Application.Interfaces.Services;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Tests.Commom;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.ProductTests
{
    /// <summary>
    /// Contains unit tests for the <see cref="UpdateProductHandler"/> class.
    /// </summary>
    public class UpdateProductHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly UpdateProductHandler _handler;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateProductHandlerTests"/> class.
        /// </summary>
        public UpdateProductHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _cacheService = Substitute.For<ICacheService>();
            _handler = new UpdateProductHandler(_mapper, _productRepository, _cacheService);
        }

        /// <summary>
        /// Tests that a valid product update request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid product data When updating product Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            var command = UpdateProductHandlerTestData.GenerateValidCommand();
            var product = UpdateProductHandlerTestData.GenerateValidProduct();
            product.Id = command.Id;
            var result = new UpdateProductResult { Id = product.Id };

            _mapper.Map<UpdateProductResult>(product).Returns(result);
            _productRepository.GetByIdAsync(product.Id).Returns(product);
            _cacheService.UpdateAsync(product.Id.ToString(), Arg.Any<Product>(), Arg.Any<TimeSpan?>())
                .Returns(Task.CompletedTask);
            _productRepository.UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
                .Returns(product);

            var updateProductResult = await _handler.Handle(command, CancellationToken.None);

            updateProductResult.Should().NotBeNull();
            updateProductResult.Id.Should().Be(product.Id);

            await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid product update request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid product data When updating product Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            var command = new UpdateProductCommand();

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
