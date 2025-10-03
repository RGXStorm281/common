namespace RobinEpple.Common.SourceGenerators.Test;

public class MarkedImplementationWithOverloads : IStaticFactoryMarker
{
	public MarkedImplementationWithOverloads(string text, int number)
	{
		Text = text;
		Number = number;
	}

	public MarkedImplementationWithOverloads(string text)
		: this(text, 0) { }

	public string Text { get; }
	public int Number { get; }
}
