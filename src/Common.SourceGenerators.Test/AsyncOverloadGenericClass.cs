namespace RobinEpple.Common.SourceGenerators.Test;

using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class AsyncOverloadGenericClass<TValue>
{
	private TValue? _value;

	[GenerateAsyncOverload]
	public TValue? GetValue() => _value;

	[GenerateAsyncOverload]
	public void SetValue(TValue value) => _value = value;
}
