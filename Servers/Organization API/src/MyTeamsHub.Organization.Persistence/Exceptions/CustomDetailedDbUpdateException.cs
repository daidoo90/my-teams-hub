using System.Runtime.Serialization;

namespace MyTeamsHub.Organization.Persistence.Exceptions;


[Serializable]
public class CustomDetailedDbUpdateException : Exception, ISerializable
{
    public CustomDetailedDbUpdateException()
    {
    }

    public CustomDetailedDbUpdateException(string message)
        : base(message)
    {
    }

    public CustomDetailedDbUpdateException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected CustomDetailedDbUpdateException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
