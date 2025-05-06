using System.ComponentModel.DataAnnotations;

namespace DotNetBackend.DTO.Workspace
{
    public class WorkspaceCreationDto
    {
        [Required]

        public  required string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }


    }
}
