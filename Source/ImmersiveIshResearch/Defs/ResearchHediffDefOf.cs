using RimWorld;
using Verse;

namespace ImmersiveResearch;

[DefOf]
public static class ResearchHediffDefOf
{
    public static HediffDef ResearchHediff;

    static ResearchHediffDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(ResearchHediffDefOf));
    }
}