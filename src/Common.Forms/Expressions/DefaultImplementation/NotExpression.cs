namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class NotExpression(IFormExpression<bool> source) : IFormExpression<bool>
{
	private readonly IFormExpression<bool> _source = source;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public bool EvaluateOn(IFormNode node)
	{
		var innerResult = _source.EvaluateOn(node);
		return !innerResult;
	}
}
