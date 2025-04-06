using DotNetBackend.Data;
using DotNetBackend.DTO.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotNetBackend.Extentions;
using DotNetBackend.Models;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
 
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;

        }

        
        [HttpGet]
        public async Task<IActionResult> GetAllUserPosts() 
        {
 //Include eagerly loads the related User for each post so that you can access the User's Name.
            var posts = await _context.Posts
                .Include(p => p.User)
                .Select(p => new PostResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Author = p.User.Name ?? "Unknown" 
                })
                .ToListAsync();

            return Ok(posts);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllUserPosts(int id)
        {
            var postDto = await _context.Posts
               .Where(x => x.Id == id)
               .Select(p => new PostResponseDto
               {
                   Id = p.Id,
                   Title = p.Title,
                   Description = p.Description,
                   Author = p.User.Name ?? "Unknown"
               })
               .SingleOrDefaultAsync();

            if (postDto == null) return NotFound();

            return Ok(postDto);
        }
        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retrieve the current user's ID using the extension method.
            var userId = User.GetUserId();
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Retrieve the user from the database.
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // Create a new Post entity using the data from the DTO.
            var post = new Post
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = user.Id,
                User = user
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Map the Post entity to a response DTO.
            var postResponse = new PostResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Author = user.Name ?? "Unknown"
            };

            return Ok(new
            {
                data = postResponse,
                message = "Post created successfully."
            });
        }
    }
}
