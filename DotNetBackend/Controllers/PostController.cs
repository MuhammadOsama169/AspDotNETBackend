using DotNetBackend.DTO.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DotNetBackend.Extentions;
using DotNetBackend.Features.Post.Commands.CreatePost;
using MediatR;
using DotNetBackend.Features.Post.Queries;
using DotNetBackend.Features.Post.Commands.DeletePost;

namespace DotNetBackend.Controllers
{
    [ApiController]
    [Route("api/post")]
    [Authorize]
 
    public class PostController : ControllerBase
    {

        private readonly IMediator _mediator;

        public PostController( IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/post

        [HttpGet]
        public async Task<IActionResult> GetAllUserPosts([FromQuery] GetAllUserPostsQuery query)
        {
            var page = await _mediator.Send(query);
            return Ok(page);
        }

        // GET: api/post/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var postDto = await _mediator.Send(new GetPostByIdQuery(id));

            if (postDto == null)
                return NotFound();

            return Ok(postDto);
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost(PostCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Retrieve the current user's ID
            var userId = User.GetUserId();
            if (userId == null || userId == Guid.Empty)
                return Unauthorized("User not authenticated.");

            var command      = new CreatePostCommand(dto, userId.Value);
            var postResponse = await _mediator.Send(command);

            return Ok(new
            {
                data = postResponse,
                message = "Post created successfully."
            });
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostById(int id)
        {
            try {
                await _mediator.Send(new DeletePostCommand(id));
                return Ok(new { message = "Post deleted" });
            }
            catch (KeyNotFoundException) {
                return NotFound("Post not found.");
            }
        }
    }
}
