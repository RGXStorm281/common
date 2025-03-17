namespace RobinEpple.Common.Forms;

using RobinEpple.Common.Forms.Nodes;

public interface IFileNodeBuilder : IFieldNodeBuilder<IFileNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="defaultValue">The default value to use.</param>
	public IFileNodeBuilder UseDefaultValue(FileValue defaultValue);
}
