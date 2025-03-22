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
		var searchTarget = node is IForm form ? form : node.Parent;

		if (searchTarget == null)
		{
			throw new NodeNotFoundException(
				"No scope could be identified, since the node is neither a form itself nor has it a parent."
			);
		}

		var searchResult = searchTarget.FindNode(_name);
		if (searchResult == null)
		{
			throw new NodeNotFoundException(
				$"A node with the name '{_name}' could not be found in the scope of '{searchTarget.GetId()}'"
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
