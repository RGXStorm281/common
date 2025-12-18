namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class ForEachCollectionItem<TValue>(string name, IFormExpression<TValue> expression)
	: IFormExpression<IEnumerable<TValue>>
{
	private readonly string _name = name;
	private readonly IFormExpression<TValue> _expression = expression;

	/// <inheritdoc />
	public IEnumerable<TValue> EvaluateOn(IFormNode node)
	{
		var targetCollection = node.GetScope().FindFirst(_name) as ICollectionNode;

		if (targetCollection == null)
		{
			throw new NodeNotFoundException(
				$"The collection with the name '{_name}' does not exist in the current scope '{node.GetScope().GetId()}'."
			);
		}

		return targetCollection.Instances.Select(_expression.EvaluateOn);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<TValue>> EvaluateOnAsync(IFormNode node)
	{
		var targetCollection = node.GetScope().FindFirst(_name) as ICollectionNode;

		if (targetCollection == null)
		{
			throw new NodeNotFoundException(
				$"The collection with the name '{_name}' does not exist in the current scope '{node.GetScope().GetId()}'."
			);
		}

		var evaluationTasks = targetCollection.Instances.Select(_expression.EvaluateOnAsync);
		return await Task.WhenAll(evaluationTasks);
	}
}
