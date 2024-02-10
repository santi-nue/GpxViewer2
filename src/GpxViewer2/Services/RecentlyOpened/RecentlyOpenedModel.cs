using System.Collections.Generic;

namespace GpxViewer2.Services.RecentlyOpened;

internal class RecentlyOpenedModel
{
    public List<RecentlyOpenedFileModel> Files { get; set; } = new();
}