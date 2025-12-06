using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Services.Interfaces;
using TWHapi.Controllers;
using AutoMapper;
using Services.Authentication;
using System.Threading.Tasks;
using Core.Response;
using Models.Models;
using System.Net;
using Models.DTOs.User;

namespace UnitTests.Api.Controllers
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserOperations> _mockUserOperations;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJWTHelper> _mockJWTHelper;

        private UserRecord _userRecord;

        public UserControllerTests()
        {
            this._mockUserOperations = new Mock<IUserOperations>();
            this._mockMapper = new Mock<IMapper>();
            this._mockJWTHelper = new Mock<IJWTHelper>();

            _userController = new UserController(
                _mockUserOperations.Object,
                _mockMapper.Object,
                _mockJWTHelper.Object);

            _userRecord = new UserRecord
            {
                Disabled = false,
                Email = "mail@domain.com",
                FirstName = "Test",
                LastName = "User",
                Phone = "1234567890",
                HashPassword = new byte[] { 1, 2, 3 },
                Salt = "random"
            };
        }

        [Fact]
        public async Task GetUsersAsync_assert_unsuccessfull_response_notExpect_Exception()
        {
            // Arrange
            const string errorMessage = "Error retrieving users";
            var mockResponse = new ServiceResponse<IEnumerable<UserRecord>>(errorMessage, HttpStatusCode.Unauthorized);

            _mockUserOperations
                .Setup(x => x.GetUsersAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.GetUsersAsync();
            var exception = await Record.ExceptionAsync(() => _userController.GetUsersAsync());

            // Assert
            Assert.NotNull(actual);
            Assert.False(actual.IsSuccess);
            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
            Assert.Equal(errorMessage, actual.Message);
            // Assert Exception is null
            Assert.Null(exception);
        }

        [Fact]
        public async void GetUsersAsync_assert_successfull_response_notExcpect_exception()
        {
            // Arrange
            var usersResponse = new List<UserRecord> { _userRecord };
            var mockResponse = new ServiceResponse<IEnumerable<UserRecord>>(usersResponse, HttpStatusCode.OK);

            _mockUserOperations
                .Setup(x => x.GetUsersAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.GetUsersAsync();
            var exception = await Record.ExceptionAsync(() => _userController.GetUsersAsync());

            // Assert
            Assert.NotNull(actual);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.IsType<ServiceResponse<IEnumerable<UserResponseDTO>>>(actual);
            // TODO: Fix mapping test
            //Assert.Equal(_userRecord.FirstName, actual.Data?.First()?.FirstName ?? string.Empty);
            Assert.Null(exception);
        }
    }
}
