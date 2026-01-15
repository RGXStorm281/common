namespace RobinEpple.Common.Forms.Wrappers;

using Microsoft.CodeAnalysis;

internal class AvailableParentNode(FormWrapperNode parentNode, IParameterSymbol parameterSymbol)
{
	public FormWrapperNode ParentNode { get; } = parentNode;
	public IParameterSymbol ParameterSymbol { get; } = parameterSymbol;
}
