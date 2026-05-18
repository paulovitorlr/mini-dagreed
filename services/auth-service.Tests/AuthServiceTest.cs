using auth_service.DTOs;
using auth_service.Models;
using auth_service.Repositories;
using auth_service.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace auth_service.Tests;

public class AuthServiceTest
{
    private readonly Mock<IAuthRepository> _repositoryMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly AuthService _service;

    public AuthServiceTest()
    {
        _repositoryMock = new Mock<IAuthRepository>();
        _configMock = new Mock<IConfiguration>();
        _configMock.Setup(c => c["JwtSettings:Secret"])
            .Returns("salve-o-Corinthias-o-campeao-dos-campeoes-eternamente-dentro-dos-nossos-corações");
        _service = new AuthService(_repositoryMock.Object, _configMock.Object);
    }

    [Fact]
    public async Task Register_ComEmailExistente_RetornaFalse()
    {
        var dto = new RegisterDTO { Email = "testte@email.com", Password = "senha123" };
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User { Email = dto.Email });

        var result = await _service.RegisterAsync(dto);

        Assert.False(result);
    }

    [Fact]
    public async Task Register_ComEmailNovo_RetornaTrue()
    {
        var dto = new RegisterDTO { Email = "novo@email.com", Password = "senha 123" };
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User?)null);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        var result = await _service.RegisterAsync(dto);

        Assert.True(result);
    }

    [Fact]
    public async Task Login_ComSenhaErrada_RetornaNull()
    {
        var dto = new LoginDTO { Email = "teste@gmail.com", Password = "senhaerrada" };
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SenhaCorreta")
            });

        var result = await _service.LoginAsync(dto);
    }

    [Fact]
    public async Task Login_ComSenhaCorreta_RetornaToken()
    {
        var dto = new LoginDTO { Email = "teste@email.com", Password = "senha123" };
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(new User
            {
                Id = 1,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("senha123")
            });

        var result = await _service.LoginAsync(dto);

        Assert.NotNull(result);
    }
}

