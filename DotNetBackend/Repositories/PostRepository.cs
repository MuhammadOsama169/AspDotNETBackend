
using DotNetBackend.Data;
using DotNetBackend.DTO.Post;
using DotNetBackend.Extensions;
using DotNetBackend.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _db;

        public PostRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        //index
        public async Task<PaginatedList<PostResponseDto>> GetPagedPostsAsync(
            int pageSize,
            int pageIndex,
            string? keyword)
        {
            var query = _db.Posts
                .Include(p => p.User)
                .AsQueryable()
                .WhereIf(
                    !string.IsNullOrWhiteSpace(keyword),
                    p => p.Title.Contains(keyword!)
                      || p.Description.Contains(keyword!));

            // ApplyPaginationAsync returns a PaginatedList<Post>
            var pagedEntities = await query
                .ApplyPaginationAsync(pageSize, pageIndex);

            // Map only the page data to DTOs
            var dtoPage = new PaginatedList<PostResponseDto>
            {
                Count = pagedEntities.Count,
                PageIndex = pagedEntities.PageIndex,
                Data = pagedEntities.Data.Adapt<List<PostResponseDto>>()
            };

            return dtoPage;
        }
        //show
        public async Task<PostResponseDto> GetByIdAsync(int id)
        {
            var post = await _db.Posts
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (post == null) 
                return null;
            return post.Adapt<PostResponseDto>();
        }
        //create 
        public async Task<Post> CreatePostAsync(Post post, CancellationToken ct = default)
        {
            _db.Posts.Add(post);
            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
            return post;
        }
    }
}
