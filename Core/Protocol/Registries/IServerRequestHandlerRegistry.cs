namespace ProjectKongor.Protocol.Registries;

public interface IServerRequestHandlerRegistry
{
	IReadOnlyDictionary<string, IServerRequestHandler> Handlers { get; }
}