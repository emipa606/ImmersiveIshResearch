﻿using Verse;

namespace ImmersiveResearch;

internal class Hediff_ResearchComp : HediffWithComps
{
    // public ResearchHediffCompProperties Props => (ResearchHediffCompProperties)props;

    public override void Notify_PawnDied()
    {
        var temp = Current.Game.GetComponent<GameComponent_ImmersiveResearch>();
        if (temp.CheckResearcherHasPublishedExperiments(pawn.Name.ToString()))
        {
            //Log.Error("Colony Researcher has died. Removing projects discovered/researched by colonist.");
            temp.ColonyResearcherDeath(pawn.Name.ToString());
        }
        else
        {
            Log.Error("pawn has no attached research defNames.");
        }

        base.Notify_PawnDied();
    }
}