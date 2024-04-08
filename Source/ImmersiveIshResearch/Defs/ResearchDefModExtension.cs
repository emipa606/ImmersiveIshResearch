using System.Collections.Generic;
using Verse;

namespace ImmersiveResearch;

public class ResearchDefModExtension : DefModExtension
{
    public readonly List<ResearchTypes> researchTypes = [];
    public bool ExperimentHasBeenMade;

    public string ResearchDefAttachedToExperiment;

    public ResearchSizes ResearchSize;
}