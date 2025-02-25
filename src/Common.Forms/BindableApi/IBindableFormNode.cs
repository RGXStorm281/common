namespace RobinEpple.Common.Forms.BindableApi;

using RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// A node that can load and store its state from and to a model.
/// </summary>
/// <inheritdoc cref="IFormNode" />
/// <inheritdoc cref="IBindable{TModel}" />
public interface IBindableFormNode<in TModel> : IFormNode, IBindable<TModel>;
