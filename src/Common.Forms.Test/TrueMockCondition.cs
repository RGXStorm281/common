namespace RobinEpple.Common.Forms.Test;

using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;

public class TrueMockCondition : IFormExpression<bool>
{
	public bool EvaluateOn(IFormNode node) => true;
}
