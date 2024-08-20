using RobinEpple.Common.Forms.BasicApi.Nodes;

namespace RobinEpple.Common.Forms.BindableApi;

/// <summary>
/// A collection that can load and store its items from and to a model.
/// </summary>
/// <inheritdoc cref="ICollectionNode{TItem}" />
/// <inheritdoc cref="IBindableFormNode{TModel}" />
public interface IBindableCollectionNode<out TItem, in TModel> : ICollectionNode<TItem>, IBindableFormNode<TModel>;