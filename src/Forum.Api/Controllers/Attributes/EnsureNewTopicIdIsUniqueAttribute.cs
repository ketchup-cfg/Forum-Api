using Forum.Api.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.Attributes;

public class EnsureNewTopicIdIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureNewTopicIdIsUniqueAttribute() : base(typeof(EnsureNewTopicIdIsUniqueFilter))
    {
    }
}