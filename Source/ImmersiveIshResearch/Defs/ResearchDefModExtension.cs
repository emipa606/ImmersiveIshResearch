using System.Collections.Generic;
using Verse;

namespace ImmersiveResearch;

public class ResearchDefModExtension : DefModExtension
{
    public bool ExperimentHasBeenMade;

    public string ResearchDefAttachedToExperiment;

    public ResearchSizes ResearchSize;
    public List<ResearchTypes> researchTypes = new List<ResearchTypes>();
}