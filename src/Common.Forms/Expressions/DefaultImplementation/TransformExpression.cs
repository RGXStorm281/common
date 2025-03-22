namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class TransformExpression<TInput, TOutput>(IFormExpression<TInput> source, Func<TInput, TOutput> selector)
	: IFormExpression<TOutput>
{
	private readonly IFormExpression<TInput> _source = source;
	private readonly Func<TInput, TOutput> _selector = selector;

	/// <inheritdoc />
	public TOutput EvaluateOn(IFormNode node)
	{
		var sourceValue = _source.EvaluateOn(node);
		return _selector(sourceValue);
	}

	/// <inheritdoc />
	public async Task<TOutput> EvaluateOnAsync(IFormNode node)
	{
		var sourceValue = await _source.EvaluateOnAsync(node);
		return _selector(sourceValue);
	}
}
