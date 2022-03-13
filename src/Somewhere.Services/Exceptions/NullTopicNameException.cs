namespace Somewhere.Services.Exceptions;

[Serializable]
public class NullTopicNameException : Exception
{
    public NullTopicNameException() { }
    public NullTopicNameException(string message) : base(message) { }
    public NullTopicNameException(string message, Exception inner) : base(message, inner) { }
        
    protected NullTopicNameException(System.Runtime.Serialization.SerializationInfo info, 
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}