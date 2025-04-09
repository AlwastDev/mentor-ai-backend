using Auth.Application.Contracts.Persistence;
using Auth.GRPC.Protos;
using Grpc.Core;

namespace Auth.GRPC.Services;

public class AuthService : AuthProtoService.AuthProtoServiceBase
{
    private readonly IAuthRepository _authRepository;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IAuthRepository authRepository, ILogger<AuthService> logger)
    {
        _authRepository = authRepository;
        _logger = logger;
    }

    public override async Task<CheckStudentResponse> CheckStudentExists(CheckStudentRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.StudentId))
        {
            _logger.LogWarning("Student id is not valid.");
            return new CheckStudentResponse { Exists = false };
        }
        
        var student = await _authRepository.GetAsync(u => u.Id == request.StudentId);
        
        _logger.LogInformation("CheckStudentExists result for id {StudentId}: {Exists}", request.StudentId, student is not null);
        
        return new CheckStudentResponse { Exists = student is not null };
    }
}