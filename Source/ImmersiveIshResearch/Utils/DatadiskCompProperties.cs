using System;
using Verse;

namespace ImmersiveResearch;

internal class DatadiskCompProperties : CompProperties
{
    public DatadiskCompProperties()
    {
        compClass = typeof(DatadiskCompProperties);
    }

    public DatadiskCompProperties(Type compClass) : base(compClass)
    {
        this.compClass = compClass;
    }
}