namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

/// <summary>
/// Wraps a value property and adds the ability to reset it to some default value.
/// </summary>
/// <typeparam name="TValue">The value type of the property.</typeparam>
/// <param name="initialDefault">The value initially set as default and current value.</param>
internal class ResettableProperty<TValue>(TValue initialDefault) : ICloneable
{
	/// <summary>
	/// The default value the property can be reset to.
	/// </summary>
	public TValue Default { get; private set; } = initialDefault;

	/// <summary>
	/// The value the property currently holds.
	/// </summary>
	public TValue CurrentValue { get; set; } = initialDefault;

	/// <summary>
	/// Clones the property. If TValue is a reference type, only references are copied.
	/// </summary>
	/// <returns>The cloned property.</returns>
	public object Clone()
	{
		var clone = new ResettableProperty<TValue>(Default);
		clone.CurrentValue = CurrentValue;
		return clone;
	}

	/// <summary>
	/// Sets a new default value without changing the current value.
	/// </summary>
	/// <param name="newDefault">The new default value the property can be reset to.</param>
	public void ReplaceDefault(TValue newDefault) => Default = newDefault;

	/// <summary>
	/// Resets the current value to the configured default value.
	/// </summary>
	public void Reset() => CurrentValue = Default;
}
