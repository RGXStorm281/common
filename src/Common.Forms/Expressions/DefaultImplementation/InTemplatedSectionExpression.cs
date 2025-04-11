namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

internal class InTemplatedSectionExpression<TValue>(string name, IFormExpression<TValue> expression)
	: IFormExpression<TValue>
{
	private readonly string _name = name;
	private readonly IFormExpression<TValue> _expression = expression;

	/// <inheritdoc />
	public TValue EvaluateOn(IFormNode node)
	{
		var targetSection = node.GetScope().FindNode(_name) as ITemplateNode;

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

	/// <inheritdoc />
	public Task<TValue> EvaluateOnAsync(IFormNode node)
	{
		var targetSection = node.GetScope().FindNode(_name) as ITemplateNode;

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

		return _expression.EvaluateOnAsync(targetSection.Instance);
	}
}
