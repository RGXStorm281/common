namespace RobinEpple.Common.SourceGenerators.Test;

/// <summary>
/// Some documentation.
/// </summary>
/// <typeparam name="TParam">Some documented type param.</typeparam>
/// <param name="parameter">Some generic parameter.</param>
public class MarkedImplementationWithTypeParams<TParam>(TParam parameter) : IStaticFactoryMarker
{
	public TParam Parameter { get; } = parameter;
}
