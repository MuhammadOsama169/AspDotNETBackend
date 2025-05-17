using MediatR;

namespace DotNetBackend.DTO.User;

public class RegistrationResultDto: IRequest<RegistrationResultDto>
{
    public Guid   UserId  { get; set; }
    public string OtpCode { get; set; } = null!;
}