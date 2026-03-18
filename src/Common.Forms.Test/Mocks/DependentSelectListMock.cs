namespace RobinEpple.Common.Forms.Test.Mocks;

using System.Collections.Generic;
using RobinEpple.Common.Forms.Expressions;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;
using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class DependentSelectListMock<TParentValue, TValue>(
	IFormExpression<TParentValue> valueDependency,
	IDictionary<TParentValue, ISelectListSource<TValue>> listByParentValue
) : ISelectListSource<TValue>
{
	private readonly IFormExpression<TParentValue> _valueDependency = valueDependency;
	private readonly IDictionary<TParentValue, ISelectListSource<TValue>> _listByParentValue = listByParentValue;

	[GenerateAsyncOverload]
	public IEnumerable<ISelectListItem<TValue>> LoadFor(IFormNode node)
	{
		var parentValue = _valueDependency.EvaluateOn(node);
		if (_listByParentValue.TryGetValue(parentValue, out var selectList))
		{
			return selectList.LoadFor(node);
		}
		return Enumerable.Empty<ISelectListItem<TValue>>();
	}
}
