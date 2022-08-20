using RimWorld;
using Verse;

namespace ImmersiveResearch;

public class Building_ExperimentBench : Building_WorkTable
{
    public ExperimentStack ExpStack;

    public Building_ExperimentBench()
    {
        ExpStack = new ExperimentStack(this);
    }


    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref ExpStack, "experimentStack", this);
    }
}