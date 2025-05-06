using DotNetBackend.Data;
using DotNetBackend.Repositories.Interfaces;
using MediatR;

namespace DotNetBackend.Features.Post.Commands.DeletePost
{
    public class DeletePostCommandHandler(IGenericRepository<Models.Post> repo)
        : IRequestHandler<DeletePostCommand, Unit>
    {
        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken ct)
        {
            var post = await repo.GetByIdAsync(request.Id, ct)
                       ?? throw new KeyNotFoundException("Post not found");

            await repo.RemoveAsync(post, ct);
            await repo.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
