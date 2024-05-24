

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace TaskMaster.Core.Application.Dtos.Account
{
    public class AuthenticationRequest
    {
        /// <summary>
        /// The user's credential, which can be either a username or an email address.
        /// </summary>
        [SwaggerParameter("The user's credential, which can be either a username or an email address.")]
        [Required(ErrorMessage = "ID is required")]
        public string UserCredential { get; set; } = null!;

        /// <summary>
        /// The user's password.
        /// </summary>
        [SwaggerParameter("The user's password.")]
        [Required(ErrorMessage = "ID is required")]
        public string Password { get; set; } = null!;
    }
}
