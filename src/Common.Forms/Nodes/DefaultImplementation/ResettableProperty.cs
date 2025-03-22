namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

internal class ResettableProperty<TValue>(TValue initialDefault) : ICloneable
{
	public TValue Default { get; private set; } = initialDefault;
	public TValue CurrentValue { get; set; } = initialDefault;

	public object Clone()
	{
		var clone = new ResettableProperty<TValue>(Default);
		clone.CurrentValue = CurrentValue;
		return clone;
	}

	public void ReplaceDefault(TValue newDefault) => Default = newDefault;

	public void Reset() => CurrentValue = Default;
}
