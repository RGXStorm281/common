namespace RobinEpple.Common.Util.Test.Comparison;

using System.Text.RegularExpressions;

/// <summary>
/// Ignores all Characters except A-z and 0-9.
/// </summary>
public class AlphanumericStringEqualityComparer : IEqualityComparer<string>
{
	private Regex? _cleanupRegex;

	private string CleanupString(string? target)
	{
		if(target == null){
			return string.Empty;
		}
		_cleanupRegex ??= new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);
		return _cleanupRegex.Replace(target, string.Empty);
	}

	/// <inheritdoc />
	public bool Equals(string? x, string? y)
	{
		var cleanedX = CleanupString(x);
		var cleanedY = CleanupString(y);

		return cleanedX == cleanedY;
	}

	/// <inheritdoc />
	public int GetHashCode(string obj)
		=> CleanupString(obj).GetHashCode();
}