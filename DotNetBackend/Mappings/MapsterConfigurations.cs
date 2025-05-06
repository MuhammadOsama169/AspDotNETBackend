using DotNetBackend.DTO.Post;
using DotNetBackend.Models;
using Mapster;

namespace DotNetBackend.Mappings
{
    public class MapsterConfigurations
    {
        public static void Configure()
        {
            // custom map for postResponseDto
            TypeAdapterConfig<Post, PostResponseDto>
                .NewConfig()
                .Map(dest => dest.Author, src => src.User.Name ?? "Unknown");


        }
    }
}
