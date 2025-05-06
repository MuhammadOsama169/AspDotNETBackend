using DotNetBackend.Features.Post.Commands.CreatePost;
using FluentValidation;

namespace DotNetBackend.Features.Post.Commands;

public class CreatePostCommandValidator: AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200);

        RuleFor(x => x.Dto.Description)
            .NotEmpty().WithMessage("Description is required");
    }
}