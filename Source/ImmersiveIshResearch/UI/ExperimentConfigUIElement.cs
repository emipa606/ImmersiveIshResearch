using UnityEngine;

namespace ImmersiveResearch;

public struct ExperimentConfigUIElement(Texture2D icon, string def, string text)
{
    public Texture2D UIIcon { get; } = icon;
    public string PartialDefName { get; } = def;
    public string UIText { get; } = text;
}