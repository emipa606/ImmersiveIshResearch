using UnityEngine;

namespace ImmersiveResearch;

public struct ExperimentConfigUIElement
{
    public Texture2D UIIcon { get; }
    public string PartialDefName { get; }
    public string UIText { get; }

    public ExperimentConfigUIElement(Texture2D icon, string def, string text)
    {
        UIIcon = icon;
        PartialDefName = def;
        UIText = text;
    }
}