using FluentValidation;

namespace DotNetBackend.Features.Post.Queries.GetAllPosts;

public class GetAllUserPostsQueryValidator 
    : AbstractValidator<GetAllUserPostsQuery>
{
    public GetAllUserPostsQueryValidator()
    {
        RuleFor(x => x.PageIndex)
            .GreaterThan(0)
            .WithMessage("PageIndex must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThan(0)
            .WithMessage("PageSize must be at least 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("PageSize cannot exceed 100.");

        RuleFor(x => x.Keyword)
            .MaximumLength(50)
            .WithMessage("Keyword cannot be longer than 50 characters.");
    }
}