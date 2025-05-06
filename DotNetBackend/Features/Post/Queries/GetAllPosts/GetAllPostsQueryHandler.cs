using MediatR;
using DotNetBackend.DTO.Post;
using DotNetBackend.Extensions;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Features.Post.Queries.GetAllPosts
{
    public class GetAllUserPostsQueryHandler 
        : IRequestHandler<GetAllUserPostsQuery, PaginatedList<PostResponseDto>>
    {
        private readonly IPostRepository _posts;

        public GetAllUserPostsQueryHandler(IPostRepository posts)
        {
            _posts = posts;
        }

        public async Task<PaginatedList<PostResponseDto>> Handle(
         GetAllUserPostsQuery request,
         CancellationToken cancellationToken)
        {
            return await _posts.GetPagedPostsAsync(
                request.PageSize,
                request.PageIndex,
                request.Keyword);
        }
    }
}
