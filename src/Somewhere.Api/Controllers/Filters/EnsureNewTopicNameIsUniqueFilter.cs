using Somewhere.Data.Models;
using Somewhere.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Somewhere.Api.Controllers.Filters;

public class EnsureNewTopicNameIsUniqueFilter : IActionFilter
{
    private readonly ITopicsService _topics;

    public EnsureNewTopicNameIsUniqueFilter(ITopicsService topics)
    {
        _topics = topics;
    }

    public async void OnActionExecuting(ActionExecutingContext context)
    {
        var id = (int) (context.ActionArguments["id"] ?? throw new InvalidOperationException());
        var topic = (Topic) (context.ActionArguments["topic"] ?? throw new InvalidOperationException());
        if (!await _topics.NewNameIsUnique(id, topic.Name)) context.Result = new BadRequestResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

}