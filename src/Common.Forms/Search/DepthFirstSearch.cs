namespace RobinEpple.Common.Forms.Search;

using RobinEpple.Common.Forms.Nodes;

public class DepthFirstSearch(Func<IFormNode, bool> predicate, bool stopOnFirstMatch) : DepthFirstTraversal
{
	private readonly Func<IFormNode, bool> _isMatch = predicate;
	private readonly bool _stopOnFirstMatch = stopOnFirstMatch;

	private List<IFormNode> _results = [];

	/// <summary>
	/// The list of nodes matching the predicate.
	/// </summary>
	public IEnumerable<IFormNode> Results => _results;

	/// <inheritdoc />
	protected override void Visit(IFormNode node, TraversalContext context)
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
