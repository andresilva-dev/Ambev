using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser
{
    /// <summary>
    /// Validator for UpdateUserCommand that defines validation rules for updating a user.
    /// </summary>
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        /// <summary>
        /// Initializes a new instance of the UpdateUserCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Id: Must not be empty
        /// - Email: Must be in valid format (using EmailValidator)
        /// - Username: Required, must be between 3 and 50 characters
        /// - Password: Must meet security requirements (using PasswordValidator)
        /// - Phone: Must match international format (+X XXXXXXXXXX)
        /// - Status: Cannot be set to Unknown
        /// - Role: Cannot be set to None
        /// </remarks>
        public UpdateUserCommandValidator()
        {
            RuleFor(user => user.Id).NotEmpty();
            RuleFor(user => user.Email).SetValidator(new EmailValidator());
            RuleFor(user => user.Username).NotEmpty().Length(3, 50);
            RuleFor(user => user.Password).SetValidator(new PasswordValidator());
            RuleFor(user => user.Phone).Matches(@"^\+?[1-9]\d{1,14}$");
            RuleFor(user => user.Status).NotEqual(UserStatus.Unknown);
            RuleFor(user => user.Role).NotEqual(UserRole.None);
        }
    }
}
