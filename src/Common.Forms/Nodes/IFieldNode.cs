namespace RobinEpple.Common.Forms.Nodes;

using RobinEpple.Common.Forms.Binding;

public interface IFieldNode : IFormNode
{
	/// <summary>
	/// A flag, indicating whether the field has been touched by the user yet (for showing validation errors).
	/// </summary>
	public bool HasUserInteraction { get; set; }

	/// <summary>
	/// Optional binding to load the state from and save changes to.
	/// </summary>
	public IFieldNodeBinding? Binding { get; set; }

	/// <summary>
	/// Returns the value of this node in a string representation.
	/// </summary>
	/// <returns>The string representation of the inner value.</returns>
	public string? GetStringValue();

	/// <inheritdoc cref="GetStringValue"/>
	public Task<string?> GetStringValueAsync();

	/// <summary>
	/// Sets the value of this node by parsing it from this string.
	/// </summary>
	/// <param name="value">The new value in string representation.</param>
	public void SetStringValue(string? value);

	/// <inheritdoc cref="SetStringValue"/>
	public Task SetStringValueAsync(string? value);
}
