using DotNetBackend.DTO.Post;
using MediatR;

namespace DotNetBackend.Features.Post.Queries
{
    public record GetPostByIdQuery(int Id) : IRequest<PostResponseDto>;

}
