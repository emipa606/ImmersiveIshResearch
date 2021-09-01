using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ImmersiveResearch
{
    public class WorkGiver_StudyExperiment : WorkGiver_DoBill
    {
        // check if work table is usable in any way
        // for each bill check if current pawn is assigned to the bill
        // if it is get the unique Thing from the experiment and pass it to the work bill
        public override Job JobOnThing(Pawn pawn, Thing thing, bool forced = false)
        {
            if (thing is not IBillGiver billGiver || !ThingIsUsableBillGiver(thing) ||
                !billGiver.BillStack.AnyShouldDoNow ||
                !billGiver.UsableForBillsAfterFueling() || !pawn.CanReserve(thing, 1, -1, null, forced) ||
                thing.IsBurning() || thing.IsForbidden(pawn))
            {
                return null;
            }

            foreach (var bill1 in billGiver.BillStack)
            {
                var table = (Building_StudyTable)billGiver;
                var exp = table.ExpStack.Experiments[table.ExpStack.IndexOfBillToExp(bill1)];

                if (!bill1.PawnAllowedToStartAnew(pawn))
                {
                    return null;
                }

                var temp = new List<ThingCount>();
                var uniqueExpThing = exp.uniqueThingIng;

                // Get unique exp thing per bill
                if (billGiver.BillStack.Bills.Count <= 0)
                {
                    return null;
                }

                // TODO need workaround for pausing a study in progress
                // trystartnewbill resets progress
                // probs just multiply/divide recipe cost based on intellectual level
                temp.Add(new ThingCount(uniqueExpThing, 1));
                return TryStartNewDoBillJob(pawn, bill1, billGiver, temp, out _);
            }


            return null;
        }
    }
}