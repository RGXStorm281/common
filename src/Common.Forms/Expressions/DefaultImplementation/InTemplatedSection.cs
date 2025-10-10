namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

internal partial class InTemplatedSection<TValue>(string name, IFormExpression<TValue> expression)
	: IFormExpression<TValue>
{
	private readonly string _name = name;
	private readonly IFormExpression<TValue> _expression = expression;

	/// <inheritdoc />
	[GenerateAsyncOverload]
	public TValue EvaluateOn(IFormNode node)
	{
		var targetSection = node.GetScope().FindFirst(_name) as ITemplateNode;

		if (targetSection == null)
		{
			throw new NodeNotFoundException(
				$"The templated section with the name '{_name}' does not exist in the current scope '{node.GetScope().GetId()}'."
			);
		}

		if (targetSection.Instance == null)
		{
			throw new NodeNotFoundException($"The section '{targetSection.GetId()}' is not instantiated.");
		}

		return _expression.EvaluateOn(targetSection.Instance);
	}
}
