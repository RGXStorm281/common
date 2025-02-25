namespace RobinEpple.Common.Forms.BindableApi;

/// <summary>
/// The interface objects, that can bind to an object of type <typeparamref name="TModel" />.
/// </summary>
/// <typeparam name="TModel">The type of the model this node can bind to.</typeparam>
public interface IBindable<in TModel>
{
	/// <summary>
	/// Loads the values from the model into the form.
	/// </summary>
	/// <param name="model">The model to load the values from.</param>
	public void LoadFrom(TModel model);

	/// <summary>
	/// Saves the values in the form to the model.
	/// </summary>
	/// <param name="model">The model to save the values to.</param>
	public void SaveTo(TModel model);
}
