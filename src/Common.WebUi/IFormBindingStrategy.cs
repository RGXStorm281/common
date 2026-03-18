namespace RobinEpple.Common.WebUi;

using Microsoft.Extensions.Primitives;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Defines a strategy for binding values from an HTTP request to a form model.
/// </summary>
public interface IFormBindingStrategy
{
	/// <summary>
	/// Tries to apply this strategy to bind the provided values to the given form node.
	/// </summary>
	/// <param name="node">The form node this value is supposed to be bound to.</param>
	/// <param name="values">The values to bind to the form node.</param>
	/// <returns><see langword="true"/> if the binding was successful; otherwise, <see langword="false"/>.</returns>
	public bool TryBind(IFormNode node, StringValues values);
}
