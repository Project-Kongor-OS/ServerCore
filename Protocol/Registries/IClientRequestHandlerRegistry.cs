namespace ProjectKongor.Protocol.Registries;

public interface IClientRequestHandlerRegistry
{
	IReadOnlyDictionary<string, IClientRequestHandler> Handlers { get; }
}