using RimWorld;
using Verse;

namespace ImmersiveResearch;

public class Building_StudyTable : Building_WorkTable
{
    public ExperimentStack ExpStack;

    public Building_StudyTable()
    {
        ExpStack = new ExperimentStack(this);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Deep.Look(ref ExpStack, "studyStack", this);
    }
}