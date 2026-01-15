namespace RobinEpple.Common.Forms.Wrappers.Abstractions;

/// <summary>
/// Mark your form construction method with this attribute, to generate a statically typed wrapper object for accessing the
/// fields, collections etc. that are created using the form builder flow api within the method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class WrapFormStructureAttribute : Attribute
{
	/// <summary>
	/// The name of the member containing the IForm, that will be wrapped by the generated code.
	/// </summary>
	public string FormMemberName { get; }

	/// <summary>
	/// Optional name of the generated member containing the wrapper. Default is <see cref="FormMemberName"/>Wrapper.
	/// </summary>
	public string? WrapperMemberName { get; }

	/// <inheritdoc cref="WrapFormStructureAttribute"/>
	/// <param name="formMemberName">The name of the member containing the IForm, that will be wrapped by the generated code.</param>
	/// <param name="wrapperMemberName">Optional name of the generated member containing the wrapper. Default is <paramref name="formMemberName"/>Wrapper.</param>
	public WrapFormStructureAttribute(string formMemberName, string? wrapperMemberName = null)
	{
		FormMemberName = formMemberName;
		WrapperMemberName = wrapperMemberName;
	}
}
