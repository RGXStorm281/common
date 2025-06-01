namespace RobinEpple.Common.Forms;

public interface IBooleanNodeBuilder : IFieldNodeBuilder<IBooleanNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	/// <returns>The node builder for further configurations.</returns>
	public IBooleanNodeBuilder UseDefaultValue(bool? defaultValue);
}
