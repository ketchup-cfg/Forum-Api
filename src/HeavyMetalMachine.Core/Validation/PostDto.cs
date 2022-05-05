using FluentValidation;

namespace HeavyMetalMachine.Core.Validation;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
}

public class PostValidator : AbstractValidator<PostDto>
{
    public PostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Please enter a title");
        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Please enter content for your post");
    }
}