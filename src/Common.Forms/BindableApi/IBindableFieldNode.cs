namespace RobinEpple.Common.Forms.BindableApi;

using RobinEpple.Common.Forms.BasicApi.Nodes;

/// <summary>
/// A field that can load and store its value from and to a model.
/// </summary>
/// <inheritdoc cref="IFieldNode{TValue}" />
/// <inheritdoc cref="IBindableFormNode{TModel}" />
public interface IBindableFieldNode<TValue, in TModel> : IFieldNode<TValue>, IBindableFormNode<TModel>;
