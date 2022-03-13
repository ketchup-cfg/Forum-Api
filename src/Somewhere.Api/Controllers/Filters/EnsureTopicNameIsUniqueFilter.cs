using Somewhere.Data.Models;
using Somewhere.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Somewhere.Api.Controllers.Filters;

public class EnsureTopicNameIsUniqueFilter : IActionFilter
{
    private readonly ITopicsService _topics;

    public EnsureTopicNameIsUniqueFilter(ITopicsService topics)
    {
        _topics = topics;
    }

    public async void OnActionExecuting(ActionExecutingContext context)
    {
        var topic = (Topic) (context.ActionArguments["topic"] ?? throw new InvalidOperationException());
        if (!await _topics.NameIsUnique(topic.Name)) context.Result = new BadRequestResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}