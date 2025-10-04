namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class GetNodeExpression<TNode>(string name) : IFormExpression<TNode>
	where TNode : IFormNode
{
	private readonly string _name = name;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TNode EvaluateOn(IFormNode node)
	{
		var scope = node.GetScope();

		var searchResult = scope.FindFirst(_name);
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
}
