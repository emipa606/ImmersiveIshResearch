using RimWorld;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

public class WorkGiver_FilingCabinet : WorkGiver_Scanner
{
    public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
    {
        // get localtarget cabinet and search map for all finished exps;
        if (t.def.defName != "ExperimentFilingCabinet")
        {
            return null;
        }

        _ = JobMaker.MakeJob(TakeExperimentJobDefOf.CabinetTakeExperiment);
        return null;
    }
}