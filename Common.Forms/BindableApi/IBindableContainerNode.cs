using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BindableApi;

/// <summary>
/// A complex node that can load and store its values from and to a model.
/// </summary>
/// <inheritdoc cref="IContainerNode" />
/// <inheritdoc cref="IBindableFormNode{TModel}" />
public interface IBindableContainerNode<in TModel> : IContainerNode, IBindableFormNode<TModel>;