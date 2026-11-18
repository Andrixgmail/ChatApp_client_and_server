using System.Drawing;

namespace ChatShared;

// I keep every colour both apps use in one place, so I can re-theme the client and the server together by changing a few lines.
// The designer files use the same numbers written out by hand, because the Visual Studio designer only understands plain values.
public static class Theme
{
    public static readonly Color Background = Color.FromArgb(24, 26, 31);
    public static readonly Color Surface = Color.FromArgb(34, 37, 44);
    public static readonly Color Text = Color.FromArgb(232, 234, 239);
    public static readonly Color MutedText = Color.FromArgb(148, 155, 168);
    public static readonly Color Accent = Color.FromArgb(47, 158, 143);
    public static readonly Color Danger = Color.FromArgb(196, 72, 72);
    public static readonly Color Success = Color.FromArgb(87, 196, 140);

    // I use this softer blue for my own name in the chat so I can spot my messages at a glance.
    public static readonly Color Self = Color.FromArgb(110, 168, 254);
}
