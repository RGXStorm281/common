namespace RobinEpple.Common.Util;

public static class StringExtensions
{
	/// <summary>
	/// Cuts the string at the maximum length if necessary.
	/// </summary>
	/// <param name="value">The string.</param>
	/// <param name="maxLength">Its maximum length.</param>
	/// <returns>The truncated string.</returns>
	public static string Truncate(this string value, int maxLength)
	{
		if (value.Length < maxLength)
		{
			return value;
		}

		return value[..maxLength];
	}

	/// <inheritdoc cref="string.Format(string,object[])"/>
	public static string Format(this string format, params object?[] args) => string.Format(format, args);
}
