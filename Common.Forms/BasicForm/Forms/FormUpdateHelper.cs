using RobinEpple.Common.Forms.BasicApi.DataContainers;
using RobinEpple.Common.Forms.BasicApi.Nodes;
using RobinEpple.Common.Forms.BasicForm.Collections;
using RobinEpple.Common.Forms.BasicForm.Fields;

namespace RobinEpple.Common.Forms.BasicForm.Forms;

public static class FormUpdateHelper
{
	/// <summary>
	/// Updates the nodes in the dictionary from the context.
	/// </summary>
	/// <param name="nodesById">The node dictionary.</param>
	/// <param name="context">The context.</param>
	public static void UpdateFrom(this IDictionary<string, IFormNode> nodesById, IContainerDataContainer context)
	{
		foreach (var (id, valueContainer) in context.ChildDataContainersByChildId)
		{
			if (!nodesById.TryGetValue(id, out var node))
			{
				// Node not found, skip this container.
				continue;
			}

			// Use pattern matching to update each node with its typed container.
			switch (node)
			{
				// fields.
				case BoolField field when valueContainer is IFieldDataContainer<bool?> fieldContainer:
				{
					field.SetData(fieldContainer);
					break;
				}
				case ByteField field when valueContainer is IFieldDataContainer<byte[]?> fieldContainer:
				{
					field.SetData(fieldContainer);
					break;
				}
				case DoubleField field when valueContainer is IFieldDataContainer<double?> fieldContainer:
				{
					field.SetData(fieldContainer);
					break;
				}
				case IntField field when valueContainer is IFieldDataContainer<int?> fieldContainer:
				{
					field.SetData(fieldContainer);
					break;
				}
				case StringField field when valueContainer is IFieldDataContainer<string?> fieldContainer:
				{
					field.SetData(fieldContainer);
					break;
				}

				// collections.
				case BoolCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}
				case ByteCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}
				case DoubleCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}
				case IntCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}
				case StringCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}
				case TemplateCollection collection when valueContainer is ICollectionDataContainer collectionContainer:
				{
					collection.SetData(collectionContainer);
					break;
				}

				// sub forms.
				case SubForm subForm when valueContainer is IContainerDataContainer subFormContainer:
				{
					subForm.SetData(subFormContainer);
					break;
				}

				// If the container did not match the required type of the node -> skip.
			}
		}
	}

	/// <summary>
	/// Iterates over the nodes and adds each data container to a dictionary, keyed by the node id.
	/// </summary>
	/// <param name="nodesById">The nodes.</param>
	/// <returns>The data container dictionary.</returns>
	public static IDictionary<string, IFormDataContainer> GetDataContainers(this IDictionary<string, IFormNode> nodesById)
	{
		var dataContainersByChildId = new Dictionary<string, IFormDataContainer>();

		foreach (var (id, node) in nodesById)
		{
			// Get data container for each type.
			switch (node)
			{
				// fields.
				case BoolField field:
				{
					dataContainersByChildId.Add(id, field.GetData());
					break;
				}
				case ByteField field:
				{
					dataContainersByChildId.Add(id, field.GetData());
					break;
				}
				case DoubleField field:
				{
					dataContainersByChildId.Add(id, field.GetData());
					break;
				}
				case IntField field:
				{
					dataContainersByChildId.Add(id, field.GetData());
					break;
				}
				case StringField field:
				{
					dataContainersByChildId.Add(id, field.GetData());
					break;
				}

				// collections.
				case BoolCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}
				case ByteCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}
				case DoubleCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}
				case IntCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}
				case StringCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}
				case TemplateCollection collection:
				{
					dataContainersByChildId.Add(id, collection.GetData());
					break;
				}

				// sub forms.
				case SubForm subForm:
				{
					dataContainersByChildId.Add(id, subForm.GetData());
					break;
				}

				// Unhandled child type -> Skip.
			}
		}

		return dataContainersByChildId;
	}

	/// <summary>
	/// Find the first node that matches the <paramref name="predicate" /> and supersedes the <paramref name="currentNode" />.<br />
	/// This method recursively traverses up the parents until it reaches the root.
	/// </summary>
	/// <param name="nodes">The nodes.</param>
	/// <param name="currentNode">Optional starting position for the search. If <see langword="null" /> all nodes starting from the last one are searched.</param>
	/// <param name="predicate">The predicate, that the node has to match.</param>
	/// <returns>The first node that matches the predicate.</returns>
	/// <exception cref="NodeNotFoundException">When no node matched the <paramref name="predicate" />.</exception>
	public static IFormNode FindClosestBefore(this IList<IFormNode> nodes, IFormNode currentNode, Func<IFormNode, bool> predicate)
	{
		// Find the child node first.
		var startSearching = false;

		// Iterate through the child list backwards.
		for (var childIndex = nodes.Count - 1; childIndex >= 0; childIndex--)
		{
			// Get the current child.
			var child = nodes[childIndex];

			// If the loop is still looking for the node to start searching: compare child to current node.
			if (startSearching == false)
			{
				startSearching = child.Equals(currentNode);

				// If true, start with the next one.
				continue;
			}

			// Skip the child if the current node is yet to be found.
			if (!startSearching)
			{
				continue;
			}

			// If the search is on: match the child against the predicate.
			if (predicate(child))
			{
				return child;
			}

			// If the child is a container, search the whole container.
			if (child is IParentNode container
				&& container.FindFirstBackwards(predicate) is
				{
				} result)
			{
				return result;
			}
		}

		if (startSearching == false)
		{
			// Current node never found.
			throw new NodeNotFoundException($"The 'currentNode' #{currentNode.Id} was not found in the children of this form.");
		}

		throw new NodeNotFoundException("No node could be found for this predicate.");
	}

	/// <summary>
	/// Iterates backwards over all children and returns the first node that matches the <paramref name="predicate" />.<br />
	/// This method does NOT search through parent nodes.
	/// </summary>
	/// <param name="nodes">The nodes.</param>
	/// <param name="predicate">The predicate, that the node has to match.</param>
	/// <returns>The first node that matches the predicate or <see langword="null" />.</returns>
	public static IFormNode? FindFirstBackwards(this IList<IFormNode> nodes, Func<IFormNode, bool> predicate)
	{
		// Iterate through the child list backwards.
		for (var childIndex = nodes.Count - 1; childIndex >= 0; childIndex--)
		{
			// Get the current child.
			var child = nodes[childIndex];

			// Match the child against the predicate.
			if (predicate(child))
			{
				return child;
			}

			// If the child is a container, search the whole container.
			if (child is IParentNode container
				&& container.FindFirstBackwards(predicate) is
				{
				} result)
			{
				return result;
			}
		}

		// Not found in the children.
		return null;
	}
}