namespace RobinEpple.Common.Forms.Binding;

using RobinEpple.Common.Forms.Nodes;

public interface IFormModel
{
	/// <summary>
	/// Checks, whether this node can accept the given model.
	/// </summary>
	/// <param name="model">The model instance to check.</param>
	/// <returns><see langword="true"/>, if the model is accepted.</returns>
	public bool Accepts(object? model);

	/// <summary>
	/// Retrieves the model from the instance.
	/// </summary>
	/// <param name="instance">The instance to get the model from.</param>
	/// <returns>The model instance.</returns>
	public object? GetInstance(IForm instance);

	/// <summary>
	/// Loads the model into the instance.
	/// </summary>
	/// <param name="instance">The instance the model is loaded into.</param>
	/// <param name="model">The model.</param>
	public void SetInstance(IForm instance, object? model);
}
