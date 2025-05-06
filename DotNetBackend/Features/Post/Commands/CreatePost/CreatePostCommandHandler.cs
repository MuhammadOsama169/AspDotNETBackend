using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Mapster;
using MediatR;
using DotNetBackend.DTO.Post;
using DotNetBackend.Models;
using DotNetBackend.Repositories.Interfaces;

namespace DotNetBackend.Features.Post.Commands.CreatePost
{

    public class CreatePostCommandHandler 
        : IRequestHandler<CreatePostCommand, PostResponseDto>
    {
        // 1) Declare your dependencies outside any method/ctor:
        private readonly IValidator<CreatePostCommand> _validator;
        private readonly IPostRepository               _postRepo;
        private readonly IGenericRepository<User>      _userRepo;

        // 2) Inject and assign in the ctor:
        public CreatePostCommandHandler(
            IValidator<CreatePostCommand> validator,
            IPostRepository postRepo,
            IGenericRepository<User> userRepo)
        {
            _validator = validator;
            _postRepo  = postRepo;
            _userRepo  = userRepo;
        }

        public async Task<PostResponseDto> Handle(
            CreatePostCommand request, 
            CancellationToken ct)
        {
            // 3) Validate the command
            await _validator.ValidateAndThrowAsync(request, ct);

            // 4) Load the User via your generic repo 
            var user = await _userRepo
                           .GetByGuidAsync(request.UserId, ct)
                       ?? throw new KeyNotFoundException($"User {request.UserId} not found.");

            // 5) Map DTO → Entity
            var postEntity = request.Dto.Adapt<Models.Post>();
            postEntity.User   = user;
            postEntity.UserId = user.Id;

            // 6) Persist via your PostRepository
            var saved = await _postRepo.CreatePostAsync(postEntity, ct);

            // 7) Map Entity → Response DTO
            var response = saved.Adapt<PostResponseDto>();
            response.Author = user.Name ?? "Unknown";

            return response;
        }
    }
}