using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

public class JobDriver_AddExperimentToCabinet : JobDriver
{
    public const TargetIndex CabinetIndex = TargetIndex.A;
    public const TargetIndex ExperimentIndex = TargetIndex.B;
    public const TargetIndex HeldExperimentIndex = TargetIndex.C;
    private Thing _currentFinishedExperiment;
    private Thing _heldThing;
    private Building _cabinet => (Building)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        job.SetTarget(TargetIndex.A, _cabinet);
        return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(CabinetIndex);

        var cabinet = job.GetTarget(TargetIndex.A).Thing as Building_ExperimentFilingCabinet;

        var thingList = LoreComputerHarmonyPatches.GetAllOfThingsOnMap("FinishedExperiment");

        if (thingList.Count == 0)
        {
            yield break;
        }

        var curExp = thingList[thingList.Count - 1] as Thing_FinishedExperiment;

        if (thingList.Count > 0)
        {
            // this appears to be only way to enqueue new custom jobs without using work/jobgiver
            foreach (var thing in thingList)
            {
                if (thing == curExp)
                {
                    break;
                }

                var newJob = JobMaker.MakeJob(AddExperimentJobDefOf.CabinetAddExperiment, cabinet);
                newJob.count = 1;
                pawn.jobs.jobQueue.EnqueueFirst(newJob);
            }
        }


        _currentFinishedExperiment = curExp;
        job.SetTarget(ExperimentIndex, _currentFinishedExperiment);

        yield return Toils_Goto.GotoThing(ExperimentIndex, PathEndMode.Touch);
        pawn.CurJob.haulMode = HaulMode.ToCellStorage;

        yield return Toils_Haul.StartCarryThing(ExperimentIndex);

        var GetHeldThing = new Toil
        {
            initAction = delegate
            {
                job.SetTarget(HeldExperimentIndex, pawn.carryTracker.CarriedThing);
                _heldThing = job.GetTarget(HeldExperimentIndex).Thing;
            }
        };
        yield return GetHeldThing;

        yield return Toils_Goto.GotoThing(CabinetIndex, PathEndMode.Touch);
        yield return Toils_Haul.PlaceHauledThingInCell(CabinetIndex,
            Toils_Goto.GotoThing(CabinetIndex, PathEndMode.Touch), false);

        // perform work toil
        var AddExperiment = new Toil
        {
            initAction = delegate
            {
                cabinet?.AddExperimentToCabinet(_heldThing);
                _heldThing.Destroy();
            }
        };

        yield return AddExperiment;
    }
}