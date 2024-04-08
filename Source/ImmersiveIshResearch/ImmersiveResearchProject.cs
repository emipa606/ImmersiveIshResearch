using System.Collections.Generic;
using Verse;

namespace ImmersiveResearch;

public class ImmersiveResearchProject(
    ResearchProjectDef projDef,
    bool discovered = false,
    float weight = 0,
    List<ResearchTypes> rTypes = null,
    ResearchSizes rSize = ResearchSizes.Small)
{
    private ResearchSizes _researchSize = rSize;

    public ImmersiveResearchProject(bool discovered, float weight, List<ResearchTypes> rTypes, ResearchSizes rSize) :
        this(null, discovered, weight, rTypes, rSize)
    {
    }

    public ResearchProjectDef ProjectDef { get; } = projDef;

    public bool IsDiscovered { get; set; } = discovered;

    public float Weighting { get; set; } = weight;

    public List<ResearchTypes> ResearchTypes { get; set; } = rTypes;

    public ResearchSizes ResearchSize { get; set; }
}