namespace RobinEpple.Common.Forms;

public interface IFileNodeBuilder : IFieldNodeBuilder<IFileNodeBuilder>
{
	/// <summary>
	/// Sets a default value that the node starts with and is resetted to.
	/// </summary>
	/// <param name="fileName">The default file name to use.</param>
	/// <param name="fileContent">The default file content to use.</param>
	public void UseDefaultValue(string? fileName, byte[]? fileContent);
}
