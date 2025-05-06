using DotNetBackend.Data;
using DotNetBackend.DTO.Workspace;
using DotNetBackend.Extentions;
using DotNetBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//namespace DotNetBackend.Controllers
//{
//    [ApiController]
//    [Authorize]
//    [Route("api/workspace")]
//    public class WorkspaceController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public WorkspaceController(ApplicationDbContext context)
//        {
//            _context = context;

//        }
//        [HttpPost]
//        public async Task <IActionResult> CreateWorkSpace(WorkspaceCreationDto dto) 
//        {
//            if(!ModelState.IsValid) return NotFound();

//            // Retrieve the current user's ID using the extension method.
//            var userId = User.GetUserId();
//            if (userId == null)
//                return Unauthorized("User not authenticated.");

//            // Retrieve the user from the database.
//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//                return NotFound("User not found.");


//            var workspace = new Workspace
//            {
//                Name = dto.Name,
//                Description = dto.Description,
//                User = user,
//                UserId = user.Id,
//            };

//            _context.Workspaces.Add(workspace);

//            await _context.SaveChangesAsync();

//            return Ok(user);
//        }

//        [HttpDelete("{ref_id}")]
//        public async Task<IActionResult> DeleteWorkSpace(Guid ref_id) 
//        {
//           var workspace = _context.Workspaces.FirstOrDefault(x => x.RefId == ref_id);
//            if(workspace == null) return NotFound(); 


//            _context.Workspaces.Remove(workspace);
//            await _context.SaveChangesAsync();

//            return Ok(new { message = "Workspace Deleted" });
//        }
//    }
//}
