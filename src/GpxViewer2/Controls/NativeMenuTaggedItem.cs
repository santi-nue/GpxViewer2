using Avalonia.Controls;

namespace GpxViewer2.Controls;

public class NativeMenuTaggedItem : NativeMenuItem
{
    public string Tag { get; set; } = string.Empty;
}