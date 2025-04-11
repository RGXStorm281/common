namespace RobinEpple.Common.Forms.Extensions;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This class provides an empty base implementation for all events in <see cref="IFormNodeExtension"/><br/>
/// so that only the needed event handlers may be overridden in inheriting classes.
/// </summary>
public abstract class FormNodeExtensionBase : IFormNodeExtension
{
	/// <inheritdoc />
	public virtual void OnBeforeReadonlyStateEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public virtual void OnAfterReadonlyStateEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public virtual void OnAfterValidation(IFormNode node) { }

	/// <inheritdoc />
	public virtual void OnAfterVisibilityEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public virtual void OnBeforeValidation(IFormNode node) { }

	/// <inheritdoc />
	public virtual void OnBeforeVisibilityEvaluation(IFormNode node) { }
}
