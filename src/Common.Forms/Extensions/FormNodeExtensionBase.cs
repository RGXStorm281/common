namespace RobinEpple.Common.Forms.Extensions;

using System.Threading.Tasks;
using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// This class provides an empty base implementation for all events in <see cref="IFormNodeExtension"/><br/>
/// so that only the needed event handlers may be overridden in inheriting classes.
/// </summary>
public abstract class FormNodeExtensionBase : IFormNodeExtension
{
	/// <inheritdoc />
	public virtual void OnInitialize(IFormNode node) { }

	/// <inheritdoc />
	public Task OnInitializeAsync(IFormNode node)
	{
		OnInitialize(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnBeforeReadonlyStateEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnBeforeReadonlyStateEvaluationAsync(IFormNode node)
	{
		OnBeforeReadonlyStateEvaluation(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnAfterReadonlyStateEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnAfterReadonlyStateEvaluationAsync(IFormNode node)
	{
		OnAfterReadonlyStateEvaluation(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnAfterValidation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnBeforeVisibilityEvaluationAsync(IFormNode node)
	{
		OnAfterValidation(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnAfterVisibilityEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnAfterVisibilityEvaluationAsync(IFormNode node)
	{
		OnAfterVisibilityEvaluation(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnBeforeValidation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnBeforeValidationAsync(IFormNode node)
	{
		OnBeforeValidation(node);
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public virtual void OnBeforeVisibilityEvaluation(IFormNode node) { }

	/// <inheritdoc />
	public Task OnAfterValidationAsync(IFormNode node)
	{
		OnBeforeVisibilityEvaluation(node);
		return Task.CompletedTask;
	}
}
