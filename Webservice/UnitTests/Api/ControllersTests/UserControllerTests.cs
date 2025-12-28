using AutoMapper;
using Core.Exceptions;
using Core.ILogs;
using Core.Response;
using Models.DTOs.User;
using Models.Models;
using Moq;
using Core.Token;
using Services.Interfaces;
using System.Net;
using TWHapi.Controllers;

namespace UnitTests.Api.ControllersTests
{
    public class UserControllerTests
    {
        private readonly UserController _userController;
        private readonly Mock<IUserOperations> _mockUserOperations;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IJWTHelper> _mockJWTHelper;
        private readonly Mock<ILoggerHelpers<UserController>> _mockLogger;

        private readonly UserRecord _userRecord;
        private readonly UserRequestDTO _userRequestDTO;

        public UserControllerTests()
        {
            this._mockUserOperations = new Mock<IUserOperations>();
            this._mockMapper = new Mock<IMapper>();
            this._mockJWTHelper = new Mock<IJWTHelper>();
            _mockLogger = new Mock<ILoggerHelpers<UserController>>();

            _userController = new UserController(
                _mockUserOperations.Object,
                _mockMapper.Object,
                _mockJWTHelper.Object,
                _mockLogger.Object);

            // instance of UserRecord
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

            // instance of UserRequestDTO
            _userRequestDTO = new UserRequestDTO
            {
                FirstName = "Test",
                LastName = "User",
                Email = "mail@domain.com",
                Password = "Password123!",

            };

            // Map UserRecord to UserResponseDTO
            this.SetUserRecordAutoMapper();
        }

        #region Private members
        private void SetUserRecordAutoMapper()
        {
            #region UserResponseDTO

            _mockMapper.Setup(x => x.Map<IEnumerable<UserResponseDTO>>(It.IsAny<IEnumerable<UserRecord>>()))
                .Returns(new List<UserResponseDTO>
                {
                    new UserResponseDTO
                    {
                        Uri = "sampleUri",
                        FirstName = _userRecord.FirstName,
                        LastName = _userRecord.LastName,
                        Email = _userRecord.Email,
                        Phone = _userRecord.Phone,
                        Disabled = _userRecord.Disabled

                    }
                });
            _mockMapper.Setup(x => x.Map<UserResponseDTO>(It.IsAny<UserRecord>()))
             .Returns(new UserResponseDTO
             {
                 Uri = "sampleUri",
                 FirstName = _userRecord.FirstName,
                 LastName = _userRecord.LastName,
                 Email = _userRecord.Email,
                 Phone = _userRecord.Phone,
                 Disabled = _userRecord.Disabled

             });

            #endregion

            #region UserRequestDTO

            _mockMapper.Setup(x => x.Map<UserRequestDTO>(It.IsAny<UserRecord>()))
                .Returns(new UserRequestDTO
                {
                    FirstName = _userRequestDTO.FirstName,
                    LastName = _userRequestDTO.LastName,
                    Email = _userRequestDTO.Email,
                    Password = _userRequestDTO.Password,
                    Phone = _userRequestDTO.Phone,
                    Disabled = _userRequestDTO.Disabled
                });

            #endregion
        }
        #endregion

        #region GetUsersAsync Tests

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
            Assert.Null(actual.Data);
            Assert.False(actual.IsSuccess);
            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
            Assert.Equal(errorMessage, actual.Message);
            // Assert Exception is null
            Assert.Null(exception);
        }

        [Fact]
        public async Task GetUsersAsync_assert_successfull_response_notExcpect_exception()
        {
            // Arrange
            var usersResponse = new List<UserRecord> { _userRecord, _userRecord };
            var mockResponse = new ServiceResponse<IEnumerable<UserRecord>>(usersResponse, HttpStatusCode.OK);

            _mockUserOperations
                .Setup(x => x.GetUsersAsync())
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.GetUsersAsync();
            var exception = await Record.ExceptionAsync(() => _userController.GetUsersAsync());

            // Assert
            Assert.NotNull(actual.Data);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.True(actual.IsSuccess);
            Assert.IsType<ServiceResponse<IEnumerable<UserResponseDTO>>>(actual);
            Assert.Equal(_userRecord.FirstName, actual.Data?.First()?.FirstName ?? string.Empty);
            Assert.Null(exception);
        }

        #endregion

        #region GetUsersByUriAsync Tests
        [Fact]
        public async Task GetUsersByUriAsync_assert_unsuccessfull_response_notExpect_Exception()
        {
            // Arrange
            const string uri = "xyz";
            const string errorMessage = "Error retrieving users";
            var mockResponse = new ServiceResponse<UserRecord>(errorMessage, HttpStatusCode.InternalServerError);

            _mockUserOperations
                .Setup(x => x.GetUserByUriAsync(uri))
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.GetUsersByUriAsync(uri);
            var exception = await Record.ExceptionAsync(() => _userController.GetUsersByUriAsync(uri));

            // Assert
            Assert.NotNull(actual);
            Assert.Null(actual.Data);
            Assert.False(actual.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.StatusCode);
            Assert.Equal(errorMessage, actual.Message);
            // Assert Exception is null
            Assert.Null(exception);
        }

        [Fact]
        public async Task GetUsersByUriAsync_assert_successfull_response_notExcpect_exception()
        {
            // Arrange
            const string uri = "xyz";
            var mockResponse = new ServiceResponse<UserRecord>(_userRecord, HttpStatusCode.OK);

            _mockUserOperations
                .Setup(x => x.GetUserByUriAsync(uri))
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.GetUsersByUriAsync(uri);
            var exception = await Record.ExceptionAsync(() => _userController.GetUsersByUriAsync(uri));

            // Assert
            Assert.NotNull(actual.Data);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
            Assert.True(actual.IsSuccess);
            Assert.IsType<ServiceResponse<UserResponseDTO>>(actual);
            Assert.Equal(_userRecord.FirstName, actual.Data?.FirstName ?? string.Empty);
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(typeof(ApiException))]
        [InlineData(typeof(Exception))]
        public async Task GetUsersByUriAsync_assert_unsuccessfull_apiException(Type exception)
        {
            // Arrange
            const string uri = "xyz";
            const string errorMessage = "Error retrieving users";
            var mockResponse = new ServiceResponse<UserRecord>(errorMessage, HttpStatusCode.InternalServerError);

            Exception toThrow = exception switch
            {
                Type t when t == typeof(ApiException) => new ApiException(ApiExceptionCode.InternalServerError, errorMessage, HttpStatusCode.InternalServerError),

                Type t when t == typeof(Exception) => new Exception(errorMessage),

                _ => throw new ArgumentOutOfRangeException(),
            };

            _mockUserOperations
                .Setup(x => x.GetUserByUriAsync(uri))
                .Throws(toThrow);


            // Act
            var _exception = await Assert.ThrowsAsync(exception, () => _userController.GetUsersByUriAsync(uri));

            // Assert
            Assert.Contains(errorMessage, _exception.Message);
        }
        #endregion

        #region InsertOneAsync Tests

        [Fact]
        public async Task InsertOneAsync_assert_successfull_response_notExcpect_exception()
        {
            // Arrange
            var mockResponse = new ServiceResponse<UserRecord>(_userRecord, HttpStatusCode.Created);

            _mockUserOperations
                .Setup(x => x.InsertOneAsync(_userRequestDTO))
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.InsertOneAsync(_userRequestDTO);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(actual.Data);
            Assert.Equal(_userRequestDTO.FirstName, actual.Data.FirstName);
            Assert.Equal(_userRequestDTO.Email, actual.Data.Email);
            Assert.Equal(HttpStatusCode.OK, actual.StatusCode);
        }

        [Fact]
        public async Task InsertOneAsync_assert_exception()
        {
            // Arrange
            _mockUserOperations
                .Setup(x => x.InsertOneAsync(_userRequestDTO))
                .ThrowsAsync(new Exception("Insertion failed"));

            // Act
            var exception = await Assert.ThrowsAsync<Exception>(() => _userController.InsertOneAsync(_userRequestDTO));

            // Assert
            Assert.Contains("Insertion failed", exception.Message);

        }
        [Fact]
        public async Task InsertOneAsync_successful_assert_Apiexception()
        {
            // Arrange
            const string errorMessage = "User already exists";
            var mockResponse = new ServiceResponse<UserRecord>(errorMessage, HttpStatusCode.Conflict);

            _mockUserOperations
                .Setup(x => x.InsertOneAsync(_userRequestDTO))
                .ReturnsAsync(mockResponse);

            // Act
            var actual = await _userController.InsertOneAsync(_userRequestDTO);

            // Assert
            Assert.NotNull(actual);
            Assert.Null(actual.Data);
            Assert.False(actual.IsSuccess);
            Assert.Equal(HttpStatusCode.Conflict, actual.StatusCode);

        }
        #endregion
    }
}
