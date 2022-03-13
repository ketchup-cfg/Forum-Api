using Microsoft.AspNetCore.Mvc;
using Somewhere.Api.Controllers.Filters;

namespace Somewhere.Api.Controllers.Attributes;

public class EnsureNewTopicIdIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureNewTopicIdIsUniqueAttribute() : base(typeof(EnsureNewTopicIdIsUniqueFilter))
    {
    }
}