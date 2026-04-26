using DanaWarga.Application.Models.Auth;
using MediatR;

namespace DanaWarga.Application.Features.Auth.Queries.Login;

public sealed record LoginQuery(string UserName, string Password) : IRequest<LoginResult>;