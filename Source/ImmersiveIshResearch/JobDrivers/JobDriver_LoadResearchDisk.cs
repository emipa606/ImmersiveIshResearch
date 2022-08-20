using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

internal class JobDriver_LoadResearchDisk : JobDriver
{
    public const TargetIndex LoreCompIndex = TargetIndex.A;
    public const TargetIndex DataDiskIndex = TargetIndex.B;
    public const TargetIndex HeldDiskIndex = TargetIndex.C;
    private Thing _heldDataDisk;
    private Thing _researchDisk;
    private Building _loreComp => (Building)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        job.SetTarget(TargetIndex.A, _loreComp);
        return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(LoreCompIndex);

        var lorecomp = job.GetTarget(TargetIndex.A).Thing as Building_LoreComputer;
        _researchDisk = lorecomp?.GetLocationOfOwnedThing("ResearchDatadisk");
        job.SetTarget(DataDiskIndex, _researchDisk);

        yield return Toils_Goto.GotoThing(DataDiskIndex, PathEndMode.Touch);

        pawn.CurJob.count =
            1; // this controls the num of times the pawn will do the toils, e.g count of 50 will make pawn do the entire job 50 times
        pawn.CurJob.haulMode = HaulMode.ToCellStorage;

        yield return Toils_Haul.StartCarryThing(DataDiskIndex);

        var GetHeldDisk = new Toil
        {
            initAction = delegate
            {
                job.SetTarget(HeldDiskIndex, pawn.carryTracker.CarriedThing);
                _heldDataDisk = job.GetTarget(HeldDiskIndex).Thing;
            }
        };
        yield return GetHeldDisk;

        yield return Toils_Goto.GotoThing(LoreCompIndex, PathEndMode.Touch);
        yield return Toils_Haul.PlaceHauledThingInCell(LoreCompIndex,
            Toils_Goto.GotoThing(LoreCompIndex, PathEndMode.Touch), false);

        // perform work toil
        var loadResearchDisk = new Toil
        {
            initAction = delegate
            {
                _heldDataDisk.Destroy();
                // get a random research from entire list
                var result = LoreComputerHarmonyPatches.SelectResearchByUniformCumulativeProb(
                    LoreComputerHarmonyPatches
                        .UndiscoveredResearchList.MainResearchDict.Values.ToList());
                LoreComputerHarmonyPatches.AddNewResearch(result);

                Find.LetterStack.ReceiveLetter("Research Disk Loaded",
                    "A Research disk has been loaded, and it's research data is now usable.",
                    LetterDefOf.PositiveEvent);
            }
        };

        yield return loadResearchDisk;
    }
}