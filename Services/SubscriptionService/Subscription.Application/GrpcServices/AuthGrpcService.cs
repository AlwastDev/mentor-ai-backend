namespace Auth.GRPC.Protos;

public class AuthGrpcService
{
    private readonly AuthProtoService.AuthProtoServiceClient _authProtoServiceClient;

    public AuthGrpcService(AuthProtoService.AuthProtoServiceClient authProtoServiceClient)
    {
        _authProtoServiceClient = authProtoServiceClient;
    }

    public async Task<bool> CheckStudent(string studentId)
    {
        var result = await _authProtoServiceClient.CheckStudentExistsAsync(new CheckStudentRequest { StudentId = studentId });
        return result.Exists;
    }
}