#nullable enable
namespace RobinEpple.Common.SourceGenerators.Test;

public partial class AsyncOverloadTestClass
{
	/// <inheritdoc cref="AsyncOverload_ShouldWorkOnGenericMethods{TItem}(TItem)"/>
	public System.Threading.Tasks.Task<TItem> AsyncOverload_ShouldWorkOnGenericMethodsAsync<TItem>(TItem item) =>
		System.Threading.Tasks.Task.FromResult<TItem>(item);
}
