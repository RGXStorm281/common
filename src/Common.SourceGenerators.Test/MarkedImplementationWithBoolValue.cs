namespace RobinEpple.Common.SourceGenerators.Test;

public class MarkedImplementationWithBoolValue : IStaticFactoryGenericMarker<bool>
{
	public bool GetValue() => true;
}
