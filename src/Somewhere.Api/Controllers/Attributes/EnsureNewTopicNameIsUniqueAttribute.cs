using Microsoft.AspNetCore.Mvc;
using Somewhere.Api.Controllers.Filters;

namespace Somewhere.Api.Controllers.Attributes;

public class EnsureNewTopicNameIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureNewTopicNameIsUniqueAttribute() : base(typeof(EnsureNewTopicNameIsUniqueFilter))
    {
    }
}