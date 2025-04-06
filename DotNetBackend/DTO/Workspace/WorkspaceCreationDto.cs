using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.DTO.Workspace
{
    public class WorkspaceCreationDto
    {
        [Required]
        [MaxLength(255)]
        public  required string Name { get; set; }
        [Required]
        public required string Description { get; set; }


    }
}
