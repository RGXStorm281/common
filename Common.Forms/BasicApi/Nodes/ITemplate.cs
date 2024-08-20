namespace RobinEpple.Common.Forms.BasicApi.Nodes;

internal interface ITemplate
{
	/// <summary>
	/// The id of the template.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Instantiates the form node of the template.
	/// </summary>
	/// <returns>The form node.</returns>
	public IContainerNode CreateInstance(IParentNode parent);
}