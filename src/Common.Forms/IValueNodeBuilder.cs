namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.Forms.SelectLists;

public interface IValueNodeBuilder<TValue, TSpecificNodeBuilder>
{
	/// <summary>
	/// Configures the <see cref="IValueNode.SelectList"/> for the value node.
	/// If <paramref name="validate"/> is <see langword="false"/>, the select items from the <paramref name="source"/> are only named value suggestions for the node that may or may not be used.
	/// If <paramref name="validate"/> is <see langword="true"/>, the node will require the value to be in the list provided by the <paramref name="source"/>.
	/// The validation will NOT raise errors for empty values, if this is wanted an additional required validation is needed.
	/// </summary>
	/// <param name="source">The source to load select list items from.</param>
	/// <param name="validate">If set to <see langword="true"/>, the node will require the value to be in the list provided by the <paramref name="source"/>.</param>
	/// <param name="errorMessageTemplate">Optional custom error message. May contain the placeholder {0} for the invalid value and {1} for the field name.</param>
	/// <returns>The node builder to chain further configuration calls.</returns>
	TSpecificNodeBuilder UseSelectList(
		ISelectListSource<TValue> source,
		bool validate,
		string? errorMessageTemplate = null
	);
}
