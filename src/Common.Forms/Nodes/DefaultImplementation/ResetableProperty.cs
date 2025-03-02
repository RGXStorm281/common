namespace RobinEpple.Common.Forms.Nodes.DefaultImplementation;

public class ResetableProperty<TValue>(TValue initialDefault)
{
	public TValue Default { get; private set; } = initialDefault;
	public TValue CurrentValue { get; set; } = initialDefault;

	public void ReplaceDefault(TValue newDefault) => Default = newDefault;

	public void Reset() => CurrentValue = Default;
}
