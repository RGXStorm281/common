namespace RobinEpple.Common.Forms.Extensions;

using RobinEpple.Common.Forms.Nodes;

public interface IValueModelExtension<TModel>
{
	/// <summary>
	/// Checks if the current management extension is responsible for the given model.
	/// </summary>
	/// <param name="model">The model instance to check.</param>
	/// <returns><see langword="true"/>, if the model can be managed by this extension.</returns>
	public bool IsApplicableTo(TModel model);

	/// <summary>
	/// Loads the model into the instance.
	/// </summary>
	/// <param name="instance">The instance to load the model into.</param>
	/// <param name="model">The model that is provided externally.</param>
	public void LoadValue(IForm instance, TModel model);
}
