namespace Forum.Services.Exceptions;

[Serializable]
public class DuplicateIdException : Exception
{
    public DuplicateIdException() { }
    public DuplicateIdException(string message) : base(message) { }
    public DuplicateIdException(string message, Exception inner) : base(message, inner) { }
        
    protected DuplicateIdException(System.Runtime.Serialization.SerializationInfo info, 
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}