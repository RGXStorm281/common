namespace RobinEpple.Common.SourceGenerators.Test;

public class MarkedImplementationWithLimitedAccessibility : IStaticFactoryMarker
{
	private MarkedImplementationWithLimitedAccessibility(string text)
	{
		Text = text;
	}

	protected MarkedImplementationWithLimitedAccessibility(bool choice)
		: this(choice.ToString()) { }

	internal MarkedImplementationWithLimitedAccessibility(DateTime timestamp)
		: this(timestamp.ToString()) { }

	public MarkedImplementationWithLimitedAccessibility(int number)
		: this(number.ToString()) { }

	public string Text { get; }
}
