using Microsoft.AspNetCore.Mvc;
using Somewhere.Api.Controllers.Filters;

namespace Somewhere.Api.Controllers.Attributes;

public class EnsureTopicNameIsUniqueAttribute : TypeFilterAttribute
{
    public EnsureTopicNameIsUniqueAttribute() : base(typeof(EnsureTopicNameIsUniqueFilter))
    {
    }
}