namespace GpxViewer2.Services.RecentlyOpened;

public class RecentlyOpenedFileOrDirectoryModel
{
    public string FullPath { get; init; } = string.Empty;

    public RecentlyOpenedType Type { get; init; }
}