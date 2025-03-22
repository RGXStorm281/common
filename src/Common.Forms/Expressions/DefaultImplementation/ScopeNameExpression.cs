namespace RobinEpple.Common.Forms.Expressions.DefaultImplementation;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

public class ScopeNameExpression : IFormExpression<string>
{
	/// <inheritdoc />
	public string EvaluateOn(IFormNode node)
	{
		var scope = node.GetScope();
		return scope.Name;
	}

	/// <inheritdoc />
	public Task<string> EvaluateOnAsync(IFormNode node)
	{
		var scope = node.GetScope();
		return Task.FromResult(scope.Name);
	}
}
