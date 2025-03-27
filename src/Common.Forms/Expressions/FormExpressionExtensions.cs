namespace RobinEpple.Common.Forms.Expressions;

using RobinEpple.Common.Forms.Nodes;
using static FormExpression;

/// <summary>
/// This static class provides additional shortcut methods for common expression use cases.<br/>
/// These rely on the foundation in <see cref="FormExpression"/> and are NOT unit-tested.
/// </summary>
public static class FormExpressionExtensions
{
	#region field value access

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IBooleanNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<bool?> BooleanFieldValue(string name) =>
		GetNode<IBooleanNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file name.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file name of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<string?> FileFieldFileName(string name) =>
		GetNode<IFileNode>(name).Select(node => node.Value.FileName);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.IFileNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its file bytes.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The file bytes of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<byte[]?> FileFieldFileContent(string name) =>
		GetNode<IFileNode>(name).Select(node => node.Value.FileContents);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.INumberNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<decimal?> NumberFieldValue(string name) =>
		GetNode<INumberNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITextNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the direct parent of the target node, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<string?> TextFieldValue(string name) =>
		GetNode<ITextNode>(name).Select(node => node.Value);

	/// <summary>
	/// Locates a unique <see cref="RobinEpple.Common.Forms.Nodes.ITimestampNode"/> with the given <paramref name="name"/><br/>
	/// in the current scope (within the closest (parent) form, if not specified otherwise)<br/>
	/// and returns its value.
	/// </summary>
	/// <param name="name">The name of the node.</param>
	/// <returns>The value of the node, if it is found.</returns>
	/// <exception cref="NodeNotFoundException">When the field with the given name does not exist or is not of the desired type.</exception>
	public static IFormExpression<DateTime?> TimestampFieldValue(string name) =>
		GetNode<ITimestampNode>(name).Select(node => node.Value);

	#endregion
}
