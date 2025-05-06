using MediatR;
using DotNetBackend.DTO.Post;
using System.Collections.Generic;
using DotNetBackend.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBackend.Features.Post.Queries
{
    public record GetAllUserPostsQuery : IRequest<PaginatedList<PostResponseDto>>
    {
        public int PageSize  { get; init; } = 10;
        public int PageIndex { get; init; } = 1;
        public string? Keyword { get; init; }
    }
}
