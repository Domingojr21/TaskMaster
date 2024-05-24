using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskMaster.Core.Application.Dtos.Account;
using TaskMaster.Core.Application.FluentValidations;
using TaskMaster.Infraestructure.Identity.Services;
using TaskMaster.WebApi.Controllers;

namespace TaskMaster.Test.UnitTest
{
    public class AccountControllerTestValidation
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly AccountController _accountController;

        public AccountControllerTestValidation()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _accountController = new AccountController(_accountServiceMock.Object);
        }

        [Fact]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var authenticationRequest = new AuthenticationRequest
            {
                UserCredential = "example@example.com",
                Password = "password123"
            };
            var authenticationResponse = new AuthenticationResponse
            {
                Id = "712b941d-9d35-4ceb-9dbf-6c57108f9102",
                UserName = "admin",
                Email = "clientprueba@email.com",
                Roles = new List<string> { "Client" },
                IsVerified = true,
                HasError = false,
                Error = null,
                JWToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImp0aSI6ImM5ZTdkNzg5LTU2NGQtNGQ3YS1iMzNkLWU4NDhhMjgyZDUzZSIsImVtYWlsIjoiY2xpZW50cHJ1ZWJhQGVtYWlsLmNvbSIsInVpZCI6IjcxMmI5NDFkLTlkMzUtNGNlYi05ZGJmLTZjNTcxMDhmOTEwMiIsInJvbGVzIjoiQ2xpZW50IiwiZXhwIjoxNzE2NTY1ODQ4LCJpc3MiOiJDb2RlSWRlbnRpdHkiLCJhdWQiOiJUYXNrTWFzdGVyVXNlciJ9.htI3ZZzQ9OrBvDbaIlSHsNv3zzBeF29EqsrxtxINcF8"
            };
            _accountServiceMock.Setup(service => service.AuthenticateAsync(authenticationRequest, true))
                .ReturnsAsync(authenticationResponse);

            // Act
            var result = await _accountController.AuthenticateAsync(authenticationRequest);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(authenticationResponse);
        }


        [Fact]
        public async Task RegisterAsync_WithValidRequest_ReturnsOkResult()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Phone = "1234567890"
            };
            _accountServiceMock.Setup(service => service.RegisterUserAsync(registerRequest, It.IsAny<string>(), "Developer"))
                .ReturnsAsync(new RegisterResponse { HasError = false });

            // Act
            var result = await _accountController.RegisterAsync(registerRequest);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);
        }


        [Fact]
        public void RegisterRequestValidator_ValidatesValidRequest_ReturnsValid()
        {
            // Arrange
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                UserName = "johndoe",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                Phone = "1234567890"
            };

            // Act
            ValidationResult validationResult = validator.Validate(registerRequest);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisterRequestValidator_InvalidFirstName_ReturnsError(string firstName)
        {
            // Arrange
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = firstName,
                LastName = "ValidLastName",
                Email = "valid@example.com",
                UserName = "ValidUserName",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!",
                Phone = "123456789"
            };

            // Act
            ValidationResult validationResult = validator.Validate(registerRequest);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(e => e.PropertyName == "FirstName" && e.ErrorMessage == "First name is required.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisterRequestValidator_InvalidLastName_ReturnsError(string lastName)
        {
            // Arrange
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = "ValidFirstName",
                LastName = lastName,
                Email = "valid@example.com",
                UserName = "ValidUserName",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!",
                Phone = "123456789"
            };

            // Act
            ValidationResult validationResult = validator.Validate(registerRequest);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(e => e.PropertyName == "LastName" && e.ErrorMessage == "Last name is required.");
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisterRequestValidator_InvalidUserName_ReturnsError(string userName)
        {
            // Arrange
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = "ValidFirstName",
                LastName = "ValidLastName",
                Email = "valid@example.com",
                UserName = userName,
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!",
                Phone = "123456789"
            };

            // Act
            ValidationResult validationResult = validator.Validate(registerRequest);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(e => e.PropertyName == "UserName" && e.ErrorMessage == "Username is required.");
        }

        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisterRequestValidator_InvalidConfirmPassword_ReturnsError(string confirmPassword)
        {
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = "ValidFirstName",
                LastName = "ValidLastName",
                Email = "valid@example.com",
                UserName = "ValidUserName",
                Password = "ValidPassword123!",
                ConfirmPassword = confirmPassword,
                Phone = "123456789"
            };

            ValidationResult validationResult = validator.Validate(registerRequest);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(e => e.PropertyName == "ConfirmPassword" && e.ErrorMessage == "Confirm password must match password.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisterRequestValidator_InvalidPhone_ReturnsError(string phone)
        {
            var validator = new RegisterRequestValidator();
            var registerRequest = new RegisterRequest
            {
                FirstName = "ValidFirstName",
                LastName = "ValidLastName",
                Email = "valid@example.com",
                UserName = "ValidUserName",
                Password = "ValidPassword123!",
                ConfirmPassword = "ValidPassword123!",
                Phone = phone
            };

            ValidationResult validationResult = validator.Validate(registerRequest);

            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().Contain(e => e.PropertyName == "Phone" && e.ErrorMessage == "Phone number is required.");
        }


    }
}
