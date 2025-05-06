using DotNetBackend.Data;
using DotNetBackend.DTO.Post;
using DotNetBackend.Repositories.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DotNetBackend.Features.Post.Queries.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostResponseDto>
    {

        private readonly IPostRepository _posts;

        public GetPostByIdQueryHandler(IPostRepository posts)
        {
            _posts = posts;
        }
        
        public async Task<PostResponseDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {

            return  await _posts.GetByIdAsync(request.Id);
            
        }

    }
}
