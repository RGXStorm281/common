namespace RobinEpple.Common.Forms;

/// <summary>
/// An exception thrown when a node is required but not found.
/// </summary>
/// <param name="message">The message with details.</param>
public class NodeNotFoundException(string? message = null) : Exception(message);
