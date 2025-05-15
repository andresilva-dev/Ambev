using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.UserTestes
{
    /// <summary>
    /// Contains unit tests for the <see cref="UpdateUserHandler"/> class.
    /// </summary>
    public class UpdateUserHandlerTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly UpdateUserHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateUserHandlerTests"/> class.
        /// Sets up the test dependencies and creates fake data generators.
        /// </summary>
        public UpdateUserHandlerTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _passwordHasher = Substitute.For<IPasswordHasher>();
            _handler = new UpdateUserHandler(_userRepository, _mapper, _passwordHasher);
        }

        /// <summary>
        /// Tests that a valid user update request is handled successfully.
        /// </summary>
        [Fact(DisplayName = "Given valid user data When updating user Then returns success response")]
        public async Task Handle_ValidRequest_ReturnsSuccessResponse()
        {
            var command = UpdateUserHandlerTestData.GenerateValidCommand();
            var user = new User
            {
                Id = command.Id,
                Username = command.Username,
                Password = command.Password,
                Email = command.Email,
                Phone = command.Phone,
                Status = command.Status,
                Role = command.Role
            };

            var result = new UpdateUserResult
            {
                Id = user.Id
            };

            _mapper.Map<UpdateUserResult>(user).Returns(result);
            _userRepository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _userRepository.GetByIdAsync(command.Id).Returns(user);
            _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

            var updateUserResult = await _handler.Handle(command, CancellationToken.None);

            updateUserResult.Should().NotBeNull();
            updateUserResult.Id.Should().Be(user.Id);
            await _userRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        }

        /// <summary>
        /// Tests that an invalid user update request throws a validation exception.
        /// </summary>
        [Fact(DisplayName = "Given invalid user data When updating user Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            var command = new UpdateUserCommand();

            var act = () => _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        /// <summary>
        /// Tests that the password is hashed before saving the updated user.
        /// </summary>
        [Fact(DisplayName = "Given user update request When handling Then password is hashed")]
        public async Task Handle_ValidRequest_HashesPassword()
        {
            var command = UpdateUserHandlerTestData.GenerateValidCommand();
            var originalPassword = command.Password;
            const string hashedPassword = "h@shedPassw0rd";
            var user = new User
            {
                Id = command.Id,
                Username = command.Username,
                Password = command.Password,
                Email = command.Email,
                Phone = command.Phone,
                Status = command.Status,
                Role = command.Role
            };

            _userRepository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                .Returns(user);
            _userRepository.GetByIdAsync(command.Id).Returns(user);
            _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);

            await _handler.Handle(command, CancellationToken.None);

            _passwordHasher.Received(1).HashPassword(originalPassword);
            await _userRepository.Received(1).UpdateAsync(
                Arg.Is<User>(u => u.Password == hashedPassword),
                Arg.Any<CancellationToken>());
        }
    }
}
