

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TaskMaster.Core.Application.Dtos.Account
{
    public class RegisterRequest
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        [SwaggerSchema("The first name of the user.")]
        [Required(ErrorMessage = "ID is required")]
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The last name of the user.
        /// </summary>
        [SwaggerSchema("The last name of the user.")]
        [Required(ErrorMessage = "ID is required")]
        public string LastName { get; set; } = null!;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        [SwaggerSchema("The email address of the user.")]
        [Required(ErrorMessage = "ID is required")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The username of the user.
        /// </summary>
        [SwaggerSchema("The username of the user.")]
        [Required(ErrorMessage = "ID is required")]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// The password for the account.
        /// </summary>
        [SwaggerSchema("The password for the account.")]
        [Required(ErrorMessage = "ID is required")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// The confirmation of the password.
        /// </summary>
        [SwaggerSchema("The confirmation of the password.")]
        [Required(ErrorMessage = "ID is required")]
        public string ConfirmPassword { get; set; } = null!;

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        [SwaggerSchema("The phone number of the user.")]
        [Required(ErrorMessage = "ID is required")]
        public string Phone { get; set; } = null!;
    }
}
