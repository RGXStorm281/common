namespace RobinEpple.Common.Forms.Test.Mocks;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

public class FalseMockCondition : IFormExpression<bool>
{
	/// <inhertidoc />
	public bool EvaluateOn(IFormNode node) => false;

	/// <inhertidoc />
	public Task<bool> EvaluateOnAsync(IFormNode node) => Task.FromResult(false);
}
