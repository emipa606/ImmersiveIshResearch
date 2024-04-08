using System;
using Verse;

namespace ImmersiveResearch;

internal class ResearchCompProperties : CompProperties
{
    public ResearchCompProperties()
    {
        compClass = typeof(ResearchThingComp);
    }

    public ResearchCompProperties(Type compClass) : base(compClass)
    {
        this.compClass = compClass;
    }
}