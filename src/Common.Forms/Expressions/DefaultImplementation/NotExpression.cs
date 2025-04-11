namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class NotExpression(IFormExpression<bool> source) : IFormExpression<bool>
{
	private readonly IFormExpression<bool> _source = source;

	/// <inheritdoc />
	public bool EvaluateOn(IFormNode node)
	{
		var innerResult = _source.EvaluateOn(node);
		return !innerResult;
	}

	/// <inheritdoc />
	public async Task<bool> EvaluateOnAsync(IFormNode node)
	{
		var innerResult = await _source.EvaluateOnAsync(node);
		return !innerResult;
	}
}
