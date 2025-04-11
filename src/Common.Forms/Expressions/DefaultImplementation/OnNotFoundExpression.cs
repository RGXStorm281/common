namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class OnNotFoundExpression<TValue>(IFormExpression<TValue> source, IFormExpression<TValue> fallbackValue)
	: IFormExpression<TValue>
{
	private readonly IFormExpression<TValue> _source = source;
	private readonly IFormExpression<TValue> _fallbackValue = fallbackValue;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		try
		{
			return _source.EvaluateOn(node);
		}
		catch (NodeNotFoundException)
		{
			return _fallbackValue.EvaluateOn(node);
		}
	}

	/// <inheritdoc />
	public async Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		try
		{
			return await _source.EvaluateOnAsync(node);
		}
		catch (NodeNotFoundException)
		{
			return await _fallbackValue.EvaluateOnAsync(node);
		}
	}
}
