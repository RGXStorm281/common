#nullable enable
namespace RobinEpple.Common.SourceGenerators.Test;

using System.Threading.Tasks;
using RobinEpple.Common.SourceGenerators.Abstractions;
using RobinEpple.Common.SourceGenerators.Test.ClassScopedExtensions;

public partial class AsyncOverloadTestClass
{
	/// <inheritdoc cref="AsyncOverload_ShouldWorkOnGenericMethods{TItem}(TItem)"/>
	public Task<TItem> AsyncOverload_ShouldWorkOnGenericMethodsAsync<TItem>(TItem item) => Task.FromResult<TItem>(item);
}
