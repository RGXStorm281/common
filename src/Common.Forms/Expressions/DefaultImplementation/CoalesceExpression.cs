namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class CoalesceExpression<TValue>(IFormExpression<TValue?> source, IFormExpression<TValue> fallbackValue)
	: IFormExpression<TValue>
{
	private readonly IFormExpression<TValue?> _source = source;
	private readonly IFormExpression<TValue> _fallbackValue = fallbackValue;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		if (sourceValue == null)
		{
			return _fallbackValue.EvaluateOn(node);
		}
		else
		{
			return sourceValue;
		}
	}

	/// <inheritdoc />
	public async Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		var sourceValue = await _source.EvaluateOnAsync(node);
		if (sourceValue == null)
		{
			return await _fallbackValue.EvaluateOnAsync(node);
		}
		else
		{
			return sourceValue;
		}
	}
}
