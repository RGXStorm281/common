namespace RobinEpple.Common.WebUi.Test.Code.Models;

public class ErrorViewModel(string? requestId, Exception? error)
{
	public string? RequestId { get; set; } = requestId;
	public Exception? Error { get; } = error;

	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
