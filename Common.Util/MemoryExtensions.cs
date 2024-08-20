namespace RobinEpple.HomeSuite.Common.Util;

public static class MemoryExtensions
{
	/// <summary>
	/// Returns a memory stream, that the content can be read from.
	/// </summary>
	/// <param name="content">The content.</param>
	/// <returns>The stream.</returns>
	public static MemoryStream GetMemoryStream(this byte[] content)
	{
		var stream = new MemoryStream(content);
		stream.Seek(0, SeekOrigin.Begin);
		return stream;
	}
}