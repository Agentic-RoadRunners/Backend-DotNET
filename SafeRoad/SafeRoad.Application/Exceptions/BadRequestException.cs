
namespace SafeRoad.Core.Exceptions;

public class BadRequestException : Exception
{
    public List<string> Errors { get; }
    public BadRequestException(string message) : base(message) { Errors = new(); }
    public BadRequestException(List<string> errors) : base("One or more validation errors occurred.") { Errors = errors; }
}