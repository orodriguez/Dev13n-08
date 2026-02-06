using Okane.Application;
using Okane.Application.Auth;
using Okane.Storage.InMemory;

namespace Okane.Tests;

public class AuthServiceTests
{
    private readonly AuthService _service;

    public AuthServiceTests() => 
        _service = new AuthService(
            new InMemoryUsersRepository(), 
            new FakePasswordHasher(), 
            new FakeTokenGenerator());

    [Fact]
    public void SignUp()
    {
        var response = _service.SignUp(new("test-user", "Password@123", "Password@123"))
            .AssertOk();
        
        Assert.Equal("test-user", response.Username);
    }
    
    [Fact]
    public void SingIn()
    {
        _service.SignUp(new("test-user", "Password@123", "Password@123"))
            .AssertOk();
        
        var response = _service.SignIn(new("test-user", "Password@123"))
            .AssertOk();
        
        Assert.Equal("token-test-user", response.Token);
    }
    
    [Fact]
    public void SingIn_PasswordDoesNotMatch()
    {
        _service.SignUp(new("test-user", "Password@123", "Password@123"))
            .AssertOk();
        
        var error = _service.SignIn(new("test-user", "Diferente@123"))
            .AssertUnauthorized();
        
        Assert.Equal("Invalid username or password.", error);
    }
    
    [Fact]
    public void SingIn_UserDoesNotExist()
    {
        var error = _service.SignIn(new("test-user", "Password@123"))
            .AssertUnauthorized();
        
        Assert.Equal("Invalid username or password.", error);
    }

    [Fact]
    public void SignUp_PasswordConfirmationDoesNotMatch()
    {
        var error = _service
            .SignUp(new("test-user", "Password@123", "Different@123"))
            .AssertBadRequest();
        Assert.Equal("Passwords do not match.", error);
    }
    //tarea
    [Fact]
    public void SignUp_UsernameIsEMpty()
    {
        var error = _service.SignUp(new("", "Password@123", "Password@123"))
            .AssertBadRequest();

        
        Assert.Equal("Username cannot be empty.", error);
    }

    [Fact]
    public void SignUp_UsernameDuplic()
    {
        _service.SignUp(new("test-user", "Password@123", "Password@123"))
            .AssertOk();
        
        var error = _service
            .SignUp(new("test-user", "Password@123", "Password@123"))
            .AssertBadRequest();
        
         Assert.Equal("Username already in use.", error);
    }

    [Fact]
    public void SignUp_PasswordIsWeak()
    {
        var error = _service.SignUp(new("test-user", "1234", "1234"))
            .AssertBadRequest();
        
        Assert.Equal("Passwords do not match.",
            error);
    }

    
}