namespace ProjectKongor.Protocol.Handlers;

public interface IRequestHandler
{
    /// <summary>
    ///    Handles a PHP request with the given `formData` and returns the appropriate result.
    /// </summary>
    public Task<IActionResult> HandleRequest(Dictionary<string, string> formData);
}