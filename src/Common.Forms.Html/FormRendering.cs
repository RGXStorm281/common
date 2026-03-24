namespace RobinEpple.Common.Forms.Html;

using Microsoft.AspNetCore.Html;
using RobinEpple.Common.Forms.Nodes;
using RobinEpple.Common.SourceGenerators.Abstractions;

/// <summary>
/// A collection of factory functions, that allow form rendering in a declarative style.
/// </summary>
[StaticFactory(typeof(IHtmlContent))]
public static partial class FormRendering
{
	/// <summary>
	/// Checks if the node is invalid and has user interaction.
	/// </summary>
	/// <param name="node">The node to check.</param>
	/// <returns><see langword="true"/> if errors for the node should be rendered.</returns>
	public static bool ShouldRenderErrorsFor(IFieldNode node) => !node.IsValid && node.HasUserInteraction;

	/// <summary>
	/// Checks if the node is valid and has user interaction.
	/// </summary>
	/// <param name="node">The node to check.</param>
	/// <returns><see langword="true"/> if the node should be rendered as valid.</returns>
	public static bool ShouldRenderValidFor(IFieldNode node) => node.IsValid && node.HasUserInteraction;
}
