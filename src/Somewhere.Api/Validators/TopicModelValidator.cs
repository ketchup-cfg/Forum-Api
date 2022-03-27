using FluentValidation;
using Somewhere.Data.Models;

namespace Somewhere.Api.Validators;

public class TopicModelValidator : AbstractValidator<Topic>
{
    public TopicModelValidator()
    {
        RuleFor(topic => topic.Name)
            .NotEmpty()
            .WithMessage("Name was not provided.");
    }
}