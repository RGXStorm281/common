namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class GetNodeExpression<TNode>(string name) : IFormExpression<TNode>
	where TNode : IFormNode
{
	private readonly string _name = name;

	/// <inheritdoc />
	public TNode EvaluateOn(IFormNode node)
	{
		var scope = node.GetScope();

		var searchResult = scope.FindNode(_name);
		if (searchResult == null)
		{
			throw new NodeNotFoundException(
				$"A node with the name '{_name}' could not be found in the scope of '{scope.GetId()}'"
			);
		}

		if (searchResult is not TNode target)
		{
			throw new NodeNotFoundException(
				$"The found node '{searchResult.GetId()}' is of type '{searchResult.GetType().FullName}' instead of '{typeof(TNode).FullName}'."
			);
		}

		return target;
	}

	/// <inheritdoc />
	public Task<TNode> EvaluateOnAsync(IFormNode node)
	{
		var result = EvaluateOn(node);
		return Task.FromResult(result);
	}
}
