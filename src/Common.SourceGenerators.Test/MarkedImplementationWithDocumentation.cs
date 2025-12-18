namespace RobinEpple.Common.SourceGenerators.Test;

public class MarkedImplementationWithDocumentation : IStaticFactoryMarker
{
	/// <summary>
	/// This is some documentation, that should be referenced in the static factory.
	/// </summary>
	/// <param name="textParameter">This is a documented parameter.</param>
	public MarkedImplementationWithDocumentation(string textParameter)
	{
		TextParameter = textParameter;
	}

	public string TextParameter { get; }
}
