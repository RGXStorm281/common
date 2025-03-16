namespace RobinEpple.Common.Forms.Expressions;

using RobinEpple.Common.Forms.Nodes;

public interface IFormExpression<TValue>
{
	public TValue EvaluateOn(IFormNode node);
}
