#nullable enable
namespace RobinEpple.Common.SourceGenerators.Test;

using System.Threading.Tasks;
using RobinEpple.Common.SourceGenerators.Abstractions;

public partial class AsyncOverloadTestClass
{
	/// <inheritdoc cref="AsyncOverload_ShouldWorkOnGenericMethods{TItem}(TItem)"/>
	public Task<TItem> AsyncOverload_ShouldWorkOnGenericMethodsAsync<TItem>(TItem item) => Task.FromResult<TItem>(item);
}
