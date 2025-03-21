namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class SelectExpression<TInput, TOutput>(
	IFormExpression<IEnumerable<TInput>> source,
	Func<TInput, TOutput> selector
) : IFormExpression<IEnumerable<TOutput>>
{
	private readonly IFormExpression<IEnumerable<TInput>> _source = source;
	private readonly Func<TInput, TOutput> _selector = selector;

	/// <inheritdoc />
	public IEnumerable<TOutput> EvaluateOn(IFormNode node)
	{
		var sourceItems = _source.EvaluateOn(node);
		foreach (var item in sourceItems)
		{
			yield return _selector(item);
		}
	}

	/// <inheritdoc />
	public async Task<IEnumerable<TOutput>> EvaluateOnAsync(IFormNode node)
	{
		var sourceItems = await _source.EvaluateOnAsync(node);
		return sourceItems.Select(_selector);
	}
}
