namespace RobinEpple.Common.Forms.Wrappers;

using Microsoft.CodeAnalysis;

public class MessageLogger(SourceProductionContext context)
{
	private readonly SourceProductionContext _context = context;

	private static readonly DiagnosticDescriptor _debugMessage = new DiagnosticDescriptor(
		id: "REFW01",
		title: $"Debug Message",
		messageFormat: "{0}",
		category: "FormWrapperGenerator",
		DiagnosticSeverity.Hidden,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor _infoMessage = new DiagnosticDescriptor(
		id: "REFW02",
		title: $"Info Message",
		messageFormat: "{0}",
		category: "FormWrapperGenerator",
		DiagnosticSeverity.Info,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor _warnMessage = new DiagnosticDescriptor(
		id: "REFW03",
		title: $"Warn Message",
		messageFormat: "{0}",
		category: "FormWrapperGenerator",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true
	);

	private static readonly DiagnosticDescriptor _errorMessage = new DiagnosticDescriptor(
		id: "REFW04",
		title: $"Error Message",
		messageFormat: "{0}",
		category: "FormWrapperGenerator",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true
	);

	public void Debug(string message, Location? location = null)
	{
		_context.ReportDiagnostic(Diagnostic.Create(_debugMessage, location ?? Location.None, message));
	}

	public void Info(string message, Location? location = null)
	{
		_context.ReportDiagnostic(Diagnostic.Create(_infoMessage, location ?? Location.None, message));
	}

	public void Warn(string message, Location? location = null)
	{
		_context.ReportDiagnostic(Diagnostic.Create(_warnMessage, location ?? Location.None, message));
	}

	public void Error(string message, Location? location = null)
	{
		_context.ReportDiagnostic(Diagnostic.Create(_errorMessage, location ?? Location.None, message));
	}
}
