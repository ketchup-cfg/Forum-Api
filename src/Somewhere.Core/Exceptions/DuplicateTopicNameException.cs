namespace Somewhere.Core.Exceptions;

[Serializable]
public class DuplicateTopicNameException : Exception
{
    public DuplicateTopicNameException()
    {
    }

    public DuplicateTopicNameException(string message) : base(message)
    {
    }

    public DuplicateTopicNameException(string message, Exception inner) : base(message, inner)
    {
    }

    protected DuplicateTopicNameException(System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
}