namespace RobinEpple.Common.Util;

using System;
using System.Diagnostics.CodeAnalysis;

public static class NullableUnwrappingTypeConverter
{
	/// <summary>
	/// Tries to convert the given object into the target type.
	/// </summary>
	/// <typeparam name="TTarget">The target type.</typeparam>
	/// <param name="input">The object to convert.</param>
	/// <param name="result">The result, if the conversion was successful.</param>
	/// <returns><see langword="true"/>, if the conversion was successful.</returns>
	public static bool TryConvert<TTarget>(object input, [NotNullWhen(true)] out TTarget? result)
	{
		result = default;

		// Unwrap nullable types.
		var targetType = typeof(TTarget);
		targetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

		// Null is not accepted.
		if (input == null || input == DBNull.Value)
		{
			return false;
		}

		// If already assignable.
		if (targetType.IsInstanceOfType(input))
		{
			result = (TTarget)input;
			return true;
		}

		try
		{
			// Attempt conversion
			result = (TTarget)Convert.ChangeType(input, targetType);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
