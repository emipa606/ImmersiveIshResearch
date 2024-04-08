using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

internal class JobDriver_DecodeDataDisk : JobDriver
{
    public const TargetIndex
        LoreCompIndex = TargetIndex.A; // TargetIndex essentially holds specific info for the current Toil being

    public const TargetIndex
        DataDiskIndex =
            TargetIndex.B; // worked on, like a ref to a Thing in game, or a list of Things, or a map cell.

    public const TargetIndex HeldDiskIndex = TargetIndex.C;
    private Thing _heldDataDisk;
    private Thing _loreDataDisk; // ref to a data disk, either single instance or a stack
    private Building _loreComp => (Building)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        job.SetTarget(TargetIndex.A, _loreComp);
        return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
    }

    public override IEnumerable<Toil> MakeNewToils()
    {
        this.FailOnDespawnedNullOrForbidden(LoreCompIndex);

        var lorecomp = job.GetTarget(TargetIndex.A).Thing as Building_LoreComputer;
        _loreDataDisk = lorecomp?.GetLocationOfOwnedThing("LockedDatadisk");
        job.SetTarget(DataDiskIndex, _loreDataDisk);

        //haul the locked datadisk to the analyzer
        yield return Toils_Goto.GotoThing(DataDiskIndex, PathEndMode.Touch);

        pawn.CurJob.count = 1;
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

        // perform work toil (decode the datadisk)
        var decodeTheData = new Toil
        {
            initAction = delegate
            {
                _heldDataDisk.Destroy();
                //make a new unlocked datadisk based on weighting
                // show alert when complete
                var temp = ThingMaker.MakeThing(LoreComputerHarmonyPatches.ChooseDataDiskTypeOnDecrypt().def);

                GenSpawn.Spawn(temp.def, _loreComp.Position, _loreComp.Map);

                Find.LetterStack.ReceiveLetter("Datadisk decoded", "A datadisk has been decoded.",
                    LetterDefOf.PositiveEvent);
            }
        };

        yield return decodeTheData;
    }
}