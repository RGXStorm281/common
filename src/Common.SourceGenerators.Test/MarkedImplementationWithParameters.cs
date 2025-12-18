namespace RobinEpple.Common.SourceGenerators.Test;

public class MarkedImplementationWithParameters(DateTime timestamp, string text, int number) : IStaticFactoryMarker
{
	public DateTime Timestamp { get; } = timestamp;
	public string Text { get; } = text;
	public int Number { get; } = number;
}
