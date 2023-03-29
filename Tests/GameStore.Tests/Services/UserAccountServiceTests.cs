using AutoMapper;
using GameStore.Common.Exceptions;
using GameStore.Common.Validator;
using GameStore.Context;
using GameStore.Context.Entities;
using GameStore.Services.UserAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GameStore.Tests.Services;

public class UserAccountServiceTests : IDisposable
{
    private readonly Mock<IMapper> mapperMock;
    private readonly Mock<IModelValidator<RegisterUserAccountModel>> registerUserAccountModelValidatorMock;
    private readonly DbContextOptions<MainDbContext> _options;
    private readonly Mock<UserManager<User>> _userManagerMock;

    public UserAccountServiceTests()
    {
        mapperMock = new Mock<IMapper>();
        registerUserAccountModelValidatorMock = new Mock<IModelValidator<RegisterUserAccountModel>>();

        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

        _options = new DbContextOptionsBuilder<MainDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDbUser")
            .Options;
    }

    [Fact]
    public async Task GetUser_Should_Return_UserAccountModel_When_User_Exists()
    {
        // Arrange
        var userName = "test_user";
        var user = new User { UserName = userName };
        _userManagerMock.Setup(u => u.FindByNameAsync(userName))
            .ReturnsAsync(user);

        mapperMock.Setup(x => x.Map<UserAccountModel>(It.IsAny<User>())).Returns(
            (User user) => new UserAccountModel
            {
                UserName = user.UserName
            }
        );

        var userService = new UserAccountService(
            mapperMock.Object,
            _userManagerMock.Object,
            registerUserAccountModelValidatorMock.Object
            );

        // Act
        var result = await userService.GetUser(userName);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserAccountModel>(result);
        Assert.Equal(userName, result.UserName);
    }

    [Fact]
    public async Task GetUser_Should_Throw_ProcessException_When_User_Does_Not_Exist()
    {
        // Arrange
        var userName = "non_existing_user";
        _userManagerMock.Setup(u => u.FindByNameAsync(userName))
            .ReturnsAsync((User)null);

        var userService = new UserAccountService(
            mapperMock.Object,
            _userManagerMock.Object,
            registerUserAccountModelValidatorMock.Object
            );

        // Act & Assert
        await Assert.ThrowsAsync<ProcessException>(() => userService.GetUser(userName));
    }

    [Fact]
    public async Task Create_ReturnsUserAccountModel_WhenValidDataIsProvided()
    {
        // Arrange
        var model = new RegisterUserAccountModel
        {
            FirstName = "John",
            LastName = "Doe",
            UserName = "johndoe",
            UserRole = "customer",
            Email = "johndoe@example.com",
            Password = "Password123",
            ImageUri = "ImageUri"
        };

        // Mock the UserManager to return a new User object when CreateAsync is called
        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<User, string>((user, password) =>
            {
                user.Id = Guid.NewGuid();
                user.PasswordHash = "hashed_password";
            });

        mapperMock.Setup(x => x.Map<UserAccountModel>(It.IsAny<User>())).Returns(
            (User user) => new UserAccountModel
            {
                UserName = user.UserName,
                Email = user.Email,
                UserRole = user.Role
            }
        );

        var userService = new UserAccountService(
            mapperMock.Object,
            _userManagerMock.Object,
            registerUserAccountModelValidatorMock.Object
            );

        // Act
        var result = await userService.Create(model);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(model.UserName, result.UserName);
        Assert.Equal(model.Email, result.Email);
        Assert.Equal(model.UserRole, result.UserRole);
    }

    public void Dispose()
    {
        var context = new MainDbContext(_options);
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
