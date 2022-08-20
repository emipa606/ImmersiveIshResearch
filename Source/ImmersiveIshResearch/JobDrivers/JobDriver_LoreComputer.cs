using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

// DEPRECATED 
/// <summary>
///     This is the RimWorld equivalent to a behaviour tree
///     Essentially makes a selected pawn attempt to interact with the lore computer object in the
///     world.
/// </summary>
internal class JobDriver_LoreComputer : JobDriver
{
    private Building _loreComp => (Building)TargetThingA;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(_loreComp, job);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        // move whatever is attempting this job to the target (lore computer)
        this.FailOnDespawnedOrNull(TargetIndex.A);
        yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);

        // create a new database window when pawn is at destination
        var accessDatabase = new Toil();
        accessDatabase.initAction = delegate
        {
            var unused = accessDatabase.actor;
            if (!_loreComp.IsBrokenDown())
            {
                // Log.Error("Attempting to create lore window from job driver", false);
                Find.WindowStack.Add(new LoreComputerWindow());
            }
        };

        yield return accessDatabase;
    }
}