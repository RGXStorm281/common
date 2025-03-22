namespace RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This interface defines an API that containers in a form have to provide to their children.
/// </summary>
public interface IParentNode : IFormNode
{
	/// <summary>
	/// Computes a unique id for the child.
	/// </summary>
	/// <param name="child">The child.</param>
	/// <returns>The id of the child.</returns>
	internal string GetChildId(IFormNode child);

	/// <summary>
	/// Checks if the given node appears somewhere in the parent hierarchy.
	/// </summary>
	/// <param name="node">The node to check for.</param>
	/// <param name="index">If <see langword="true"/>, this is the number of layers to go up. The direct parent has index 0.</param>
	/// <returns><see langword="true"/> if the <paramref name="node"/> is somewhere in the parent stack.</returns>
	public bool StackContains(IParentNode node, out int index);

	/// <summary>
	/// Travels up <paramref name="index"/> layers in the parent stack and returns that parent.
	/// </summary>
	/// <param name="index">The index of the parent layer. The direct parent is index 0.</param>
	/// <returns>The parent at the given index.</returns>
	/// <exception cref="InvalidOperationException">If the given <paramref name="index"/> is negative.</exception>
	/// <exception cref="IndexOutOfRangeException">If the given <paramref name="index"/> is greater than the actual parent stack.</exception>
	public IParentNode GetParentAt(int index);
}
