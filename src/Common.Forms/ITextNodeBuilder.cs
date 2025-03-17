namespace RobinEpple.Common.Forms;

public interface ITextNodeBuilder : IFieldNodeBuilder<ITextNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	public void UseDefaultValue(string? defaultValue);
}
