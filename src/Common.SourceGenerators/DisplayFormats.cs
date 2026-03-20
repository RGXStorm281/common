namespace RobinEpple.Common.SourceGenerators;

using Microsoft.CodeAnalysis;

internal class DisplayFormats
{
	public static SymbolDisplayFormat FullyQualifiedTypeFormat =>
		new SymbolDisplayFormat(
			globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
			typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
			genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
			miscellaneousOptions: SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
				| SymbolDisplayMiscellaneousOptions.UseSpecialTypes
		);

	public static SymbolDisplayFormat FullyQualifiedInheritdocFormat =>
		new SymbolDisplayFormat(
			globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
			typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
			genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
			memberOptions: SymbolDisplayMemberOptions.IncludeParameters,
			parameterOptions: SymbolDisplayParameterOptions.IncludeType,
			miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
		);

	public static SymbolDisplayFormat FullyQualifiedMethodNameFormat =>
		new SymbolDisplayFormat(
			globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
			typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
			genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
			memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
			parameterOptions: SymbolDisplayParameterOptions.None,
			miscellaneousOptions: SymbolDisplayMiscellaneousOptions.UseSpecialTypes
		);
}
