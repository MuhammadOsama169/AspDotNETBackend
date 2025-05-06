using DotNetBackend.DTO.Post;
using MediatR;

namespace DotNetBackend.Features.Post.Commands.DeletePost
{
    public record DeletePostCommand(int Id) : IRequest<Unit>;

}
