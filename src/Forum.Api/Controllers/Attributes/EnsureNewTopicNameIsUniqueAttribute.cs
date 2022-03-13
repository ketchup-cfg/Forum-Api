using Forum.Api.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.Attributes;

public class EnsureNewTopicNameIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureNewTopicNameIsUniqueAttribute() : base(typeof(EnsureNewTopicNameIsUniqueFilter))
    {
    }
}