using DanaWarga.Application.Abstractions.Persistence;
using DanaWarga.Application.Abstractions.Security;
using DanaWarga.Application.Models.Auth;
using MediatR;

namespace DanaWarga.Application.Features.Auth.Queries.Login;

public sealed class LoginQueryHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginQuery, LoginResult>
{
    public async Task<LoginResult> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUserNameAsync(request.UserName, cancellationToken)
            ?? throw new UnauthorizedAccessException("Invalid username or password.");

        if (!passwordHasher.Verify(user.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        var roles = user.Roles.Select(x => x.RoleName).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        var (token, expiresAt) = jwtTokenGenerator.Generate(user, roles);
        return new LoginResult(token, expiresAt, roles);
    }
}