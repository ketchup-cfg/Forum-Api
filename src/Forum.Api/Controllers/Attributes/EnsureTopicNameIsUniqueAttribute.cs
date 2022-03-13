using Forum.Api.Controllers.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Api.Controllers.Attributes;

public class EnsureTopicNameIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureTopicNameIsUniqueAttribute() : base(typeof(EnsureTopicNameIsUniqueFilter))
    {
    }
}