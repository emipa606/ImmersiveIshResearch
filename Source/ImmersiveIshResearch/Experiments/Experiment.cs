using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace ImmersiveResearch;

public class Experiment : Bill_Production
{
    private readonly int loadID = -1;
    [Unsaved] public ExperimentStack expStack;

    public Pawn uniquePawnDoer;
    public Thing uniqueThingIng;

    public Experiment()
    {
    }

    public Experiment(RecipeDef recipe)
    {
        this.recipe = recipe;
    }

    public new bool DeletedOrDereferenced
    {
        get
        {
            if (deleted)
            {
                return true;
            }

            return expStack.billGiver is Thing { Destroyed: true };
        }
    }

    // unfortunate how it is not virtual in base class
    // TODO: create TexButton icons (RW UI textures are internal)
    public new Rect DoInterface(float x, float y, float width, int index)
    {
        var rect = new Rect(x, y, width, 53f);
        var num = 0f;
        if (!StatusString.NullOrEmpty())
        {
            num = Mathf.Max(17f, StatusLineMinHeight);
        }

        rect.height += num;
        var color = Color.white;

        GUI.color = color;
        Text.Font = GameFont.Small;
        if (index % 2 == 0)
        {
            Widgets.DrawAltRect(rect);
        }

        GUI.BeginGroup(rect);
        var rect2 = new Rect(0f, 0f, 24f, 24f);
        if (expStack.IndexOf(this) > 0)
        {
            if (Widgets.ButtonText(rect2, "U"))
            {
                expStack.Reorder(this, -1);
                SoundDefOf.Tick_High.PlayOneShotOnCamera();
            }

            TooltipHandler.TipRegion(rect2, "IR.ReorderBillUpTip".Translate());
        }

        if (expStack.IndexOf(this) < expStack.ListCount - 1)
        {
            var rect3 = new Rect(0f, 24f, 24f, 24f);
            if (Widgets.ButtonText(rect3, "D"))
            {
                expStack.Reorder(this, 1);
                SoundDefOf.Tick_Low.PlayOneShotOnCamera();
            }

            TooltipHandler.TipRegion(rect3, "IR.ReorderBillDownTip".Translate());
        }

        var rect4 = new Rect(28f, 0f, rect.width - 48f - 20f, 24f);
        Widgets.Label(rect4, LabelCap);

        if (uniquePawnDoer != null)
        {
            var pawnBillDoerRect = new Rect(28f, 20f, rect.width - 48f - 20f, 24f);
            Widgets.Label(pawnBillDoerRect, $"Reserved for: {uniquePawnDoer.Name}");
        }


        var rect5 = new Rect(300f, 0f, 70f, 24f);
        if (Widgets.ButtonText(rect5, "Delete"))
        {
            expStack.Delete(this);
            SoundDefOf.Click.PlayOneShotOnCamera();
        }

        TooltipHandler.TipRegion(rect5, "IR.DeleteBillTip".Translate());

        var rect6 = new Rect(300f, 26f, 70f, 24f);
        if (Widgets.ButtonText(rect6, "Suspend"))
        {
            // in RW, setting a bill as suspended will only take effect if you stop the pawn from doing the bill job,
            // for example, if a pawn is doing a bill and you suspend it, it will keep doing the bill until you manually stop them or they stop themselves.
            suspended = !suspended;
            expStack.SetSuspended(this, suspended);
        }

        TooltipHandler.TipRegion(rect6, "IR.SuspendBillTip".Translate());


        GUI.EndGroup();
        if (suspended)
        {
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            var rect9 = new Rect(rect.x + (rect.width / 2f) - 70f, rect.y + (rect.height / 2f) - 20f, 140f, 40f);
            GUI.DrawTexture(rect9, TexUI.GrayTextBG);
            Widgets.Label(rect9, "IR.SuspendedCaps".Translate());
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Small;
        }

        Text.Font = GameFont.Small;
        GUI.color = Color.white;
        return rect;
    }

    public override void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
    {
    }

    public new string GetUniqueLoadID()
    {
        return $"Experiment_{recipe.defName}_{loadID}";
    }

    public override void ExposeData()
    {
        Scribe_Defs.Look(ref recipe, "recipe");
        Scribe_Values.Look(ref suspended, "suspended");
    }
}