using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.UpdateUser
{
    /// <summary>
    /// Represents a request to update an existing user in the system.
    /// </summary>
    public class UpdateUserRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user to update.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the updated username.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated phone number.
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated email address.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the updated status of the user account.
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the updated role assigned to the user.
        /// </summary>
        public UserRole Role { get; set; }
    }
}
