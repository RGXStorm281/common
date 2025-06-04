namespace RobinEpple.Common.Forms.Search;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Searches the tree for nodes that match the <paramref name="predicate"/>.
/// </summary>
/// <param name="predicate">The predicate defining a matching node.</param>
/// <param name="stopOnFirstMatch">Whether the search should stop on the first match.</param>
public class BreadthFirstSearch(Func<IFormNode, bool> predicate, bool stopOnFirstMatch) : BreadthFirstTraversal
{
	private readonly Func<IFormNode, bool> _isMatch = predicate;
	private readonly bool _stopOnFirstMatch = stopOnFirstMatch;

	private List<IFormNode> _results = [];

	/// <summary>
	/// The list of nodes matching the predicate.
	/// </summary>
	public IEnumerable<IFormNode> Results => _results;

	/// <inheritdoc />
	protected override void ExecuteOnNode(IFormNode node, TraversalContext context)
	{
		if (!_isMatch(node))
		{
			return;
		}

		_results.Add(node);

		if (_stopOnFirstMatch)
		{
			context.Quit = true;
		}
	}
}
