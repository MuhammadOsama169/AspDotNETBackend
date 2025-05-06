using DotNetBackend.DTO.Post;
using DotNetBackend.Extensions;

namespace DotNetBackend.Repositories.Interfaces
{
    public interface IPostRepository
    {
        Task<PaginatedList<PostResponseDto>> GetPagedPostsAsync(
            int pageSize,
            int pageIndex,
            string? keyword);
        
        Task<PostResponseDto> GetByIdAsync(int id);
        
        Task<Models.Post> CreatePostAsync(Models.Post post, CancellationToken ct = default);
    }
}