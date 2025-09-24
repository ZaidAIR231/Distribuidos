using System;

namespace Anbucriminals.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public sealed class InvalidIdException : DomainException
{
    public InvalidIdException(Guid id) : base($"Invalid id '{id}'. Id must be a non-empty GUID.") { }
}

public sealed class NinjaNotFoundException : DomainException
{
    public Guid Id { get; }
    public NinjaNotFoundException(Guid id) : base($"Ninja with id '{id}' was not found.") => Id = id;
}

public sealed class UpstreamUnavailableException : DomainException
{
    public UpstreamUnavailableException(string? message = null)
        : base(message ?? "Upstream service is unavailable.") { }
}
