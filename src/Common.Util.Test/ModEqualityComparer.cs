namespace RobinEpple.Common.Util.Test;

public class ModEqualityComparer(int moduloGroup) : IEqualityComparer<int>
{
	public bool Equals(int x, int y) => x % moduloGroup == y % moduloGroup;

	public int GetHashCode(int obj) => (obj % moduloGroup).GetHashCode();
}
