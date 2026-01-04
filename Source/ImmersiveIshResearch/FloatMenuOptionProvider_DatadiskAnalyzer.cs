using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace ImmersiveResearch;

public class FloatMenuOptionProvider_DatadiskAnalyzer : FloatMenuOptionProvider
{
    protected override bool Drafted => true;

    protected override bool Undrafted => false;

    protected override bool Multiselect => false;

    protected override bool RequiresManipulation => true;

    protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
    {
        // Check if we have any mod that already adds execution of downed (not asleep) targets
        // Let me know if I need to add more mods to this list
        var modsToCheck = new List<string> { "unlimitedhugs.allowtool", "scorpio.deathsentence" };
        var canTargetDownedAwake = !ModLister.AnyFromListActive(modsToCheck);

        // Clutter the menu ONLY if the target is asleep
        // ...or if the target is downed and we don't already have a mod adding execution of downed pawns.
        if (!clickedPawn.Awake() || clickedPawn.Downed && canTargetDownedAwake)
        {
            // Check animal vs not
            if (clickedPawn.RaceProps.Animal)
            {
                // animal target, use "slaughter" language
                if (context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Violent))
                {
                    return new FloatMenuOption(
                        "HSCannotSlaughter".Translate(clickedPawn.LabelShortCap) + ": " +
                        "IsIncapableOfViolenceLower".Translate(context.FirstSelectedPawn.LabelShort,
                            context.FirstSelectedPawn), null);
                }

                // If the animal is bonded, mimic Allow Tool behavior to finish off friendly downed targets
                var isBonded = clickedPawn.relations.DirectRelations.Any(rel => rel.def == PawnRelationDefOf.Bond);
                if (isBonded && !ShiftIsHeld())
                {
                    return new FloatMenuOption("HSSlaughterBonded".Translate(clickedPawn.LabelShortCap), null);
                }

                return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(
                    "HSSlaughter".Translate(clickedPawn.LabelShortCap), delegate
                    {
                        var job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("HSSlaughter"), clickedPawn);
                        context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                    }), context.FirstSelectedPawn, clickedPawn);
            }

            // non-animal target, use "assassinate" language
            if (context.FirstSelectedPawn.WorkTagIsDisabled(WorkTags.Violent))
            {
                return new FloatMenuOption(
                    "HSCannotAssassinate".Translate(clickedPawn.LabelShortCap) + ": " +
                    "IsIncapableOfViolenceLower".Translate(context.FirstSelectedPawn.LabelShort,
                        context.FirstSelectedPawn), null);
            }

            // If the not-animal is friendly, mimic Allow Tool behavior to finish off friendly downed targets
            var isFriendly = clickedPawn.Faction == context.FirstSelectedPawn.Faction || clickedPawn.Faction != null &&
                clickedPawn.Faction.RelationKindWith(context.FirstSelectedPawn.Faction) == FactionRelationKind.Ally;
            if (isFriendly && !ShiftIsHeld())
            {
                return new FloatMenuOption("HSAssassinateFriendly".Translate(clickedPawn.LabelShortCap), null);
            }

            return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(
                "HSAssassinate".Translate(clickedPawn.LabelShortCap), delegate
                {
                    var job = JobMaker.MakeJob(DefDatabase<JobDef>.GetNamed("HSAssassinate"), clickedPawn);
                    context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }), context.FirstSelectedPawn, clickedPawn);
        }

        return null;
    }

    private static bool ShiftIsHeld()
    {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
}