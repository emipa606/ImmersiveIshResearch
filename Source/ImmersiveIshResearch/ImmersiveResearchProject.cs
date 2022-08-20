using System.Collections.Generic;
using Verse;

namespace ImmersiveResearch;

public class ImmersiveResearchProject
{
    private ResearchSizes _researchSize;

    public ImmersiveResearchProject(ResearchProjectDef projDef, bool discovered, float weight,
        List<ResearchTypes> rTypes, ResearchSizes rSize)
    {
        ProjectDef = projDef;
        IsDiscovered = discovered;
        Weighting = weight;
        ResearchTypes = rTypes;
        _researchSize = rSize;
    }

    public ImmersiveResearchProject(bool discovered, float weight, List<ResearchTypes> rTypes, ResearchSizes rSize)
    {
        IsDiscovered = discovered;
        Weighting = weight;
        ResearchTypes = rTypes;
        _researchSize = rSize;
    }

    public ImmersiveResearchProject(ResearchProjectDef projDef, bool discovered, float weight)
    {
        ProjectDef = projDef;
        IsDiscovered = discovered;
        Weighting = weight;
    }

    public ImmersiveResearchProject(ResearchProjectDef projDef)
    {
        ProjectDef = projDef;
    }

    public ResearchProjectDef ProjectDef { get; set; }

    public bool IsDiscovered { get; set; }

    public float Weighting { get; set; }

    public List<ResearchTypes> ResearchTypes { get; set; }

    public ResearchSizes ResearchSize { get; set; }
}