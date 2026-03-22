namespace RobinEpple.Common.WebUi.Test.Code.Models.FolderModel;

public class Folder : IStructureElement
{
	public string? Name { get; set; }
	public IEnumerable<IStructureElement> Items { get; set; } = [];
}
