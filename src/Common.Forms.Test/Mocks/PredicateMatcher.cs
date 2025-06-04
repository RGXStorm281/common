namespace RobinEpple.Common.Forms.Test.Mocks;

using RobinEpple.Common.Forms.Nodes;

/// <summary>
/// Sorts the nodes according to the predicate.
/// </summary>
/// <param name="predicate">The predicate.</param>
public class PredicateMatcher(Func<IFormNode, bool> predicate) : BreadthFirstTraversal
{
	private readonly Func<IFormNode, bool> _isMatch = predicate;

	private List<IFormNode> _notMatchingNodes = [];
	private List<IFormNode> _matchingNodes = [];

	public IEnumerable<IFormNode> NotMatchingNodes => _notMatchingNodes;
	public IEnumerable<IFormNode> MatchingNodes => _matchingNodes;

	protected override void ExecuteOnNode(IFormNode node, TraversalContext context)
	{
		if (_isMatch(node))
		{
			_matchingNodes.Add(node);
		}
		else
		{
			_notMatchingNodes.Add(node);
		}
	}
}
