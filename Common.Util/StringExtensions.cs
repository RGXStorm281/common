namespace RobinEpple.HomeSuite.Common.Util;

public static class StringExtensions
{
	/// <summary>
	/// Trims a string until it finds the specified char. The specified char is also trimmed.
	/// </summary>
	/// <param name="str">The string to trim.</param>
	/// <param name="ch">The char that marks the beginning of the desired content.</param>
	/// <returns>The desired content after the char.</returns>
	public static string? TrimStartUpToAndIncluding(this string? str, char ch)
	{
		if (string.IsNullOrWhiteSpace(str))
		{
			return null;
		}

		var pos = str!.IndexOf(ch);
		return pos >= 0 ? str[(pos + 1)..] : str;
	}

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

	/// <summary>
	/// Translates a string to an exact length, by either cutting it or padding it at the end.
	/// </summary>
	/// <param name="value">The string to fit to the given length.</param>
	/// <param name="length">The length to fit the string to.</param>
	/// <param name="paddingChar">Optional character, that is added to the end to fill the needed space.</param>
	/// <returns>The perfect sized string.</returns>
	public static string FitToLength(this string value, int length, char paddingChar = ' ')
	{
		if (value.Length < length)
		{
			return value.PadRight(length, paddingChar);
		}

		if (value.Length > length)
		{
			value.Truncate(length);
		}

		return value;
	}

	/// <inheritdoc cref="string.Format(string,object[])"/>
	public static string Format(this string format, params object?[] args)
		=> string.Format(format, args);
}