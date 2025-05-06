using MediatR;
using DotNetBackend.DTO.Post;

namespace DotNetBackend.Features.Post.Commands.CreatePost
{
    public record CreatePostCommand(
        PostCreationDto Dto,
        Guid            UserId
    ) : IRequest<PostResponseDto>;
}

