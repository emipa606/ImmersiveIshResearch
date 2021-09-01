using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    public class Dialog_StudyConfig : Window
    {
        private readonly ImmersiveResearchWindowDrawingUtility _colonyResearchers =
            new ImmersiveResearchWindowDrawingUtility();

        private readonly ImmersiveResearchWindowDrawingUtility _expsInColony =
            new ImmersiveResearchWindowDrawingUtility();

        private readonly List<Tuple<string, Thing>> _finishedExperimentList = new List<Tuple<string, Thing>>();
        private readonly List<Tuple<string, Thing>> _pawnsInColony = new List<Tuple<string, Thing>>();
        private readonly Building_StudyTable _selectedTable;

        public Dialog_StudyConfig(Building_StudyTable table)
        {
            _selectedTable = table;

            var pawns = Find.CurrentMap.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);

            foreach (var item2 in pawns)
            {
                if (!item2.IsColonist)
                {
                    continue;
                }

                // remove non research assigned pawns
                if (item2.workSettings.WorkIsActive(WorkTypeDefOf.Research) &&
                    !item2.WorkTypeIsDisabled(WorkTypeDefOf.Research))
                {
                    _pawnsInColony.Add(new Tuple<string, Thing>(item2.Name.ToString(), item2));
                }
            }


            if (!(from t in DefDatabase<ThingDef>.AllDefs where t.defName == "FinishedExperiment" select t)
                .TryRandomElement(out var finalDef))
            {
                Log.Error("Unable to locate finished experiment def in DefDatabase.");
            }
            else
            {
                var req = ThingRequest.ForDef(finalDef);
                var thingList = Find.CurrentMap.listerThings.ThingsMatching(req);

                if (thingList.Count == 0)
                {
                    // Log.Error("No finished exps in colony.");
                    return;
                }

                foreach (var thing in thingList)
                {
                    var unused = thing.TryGetComp<ResearchThingComp>();

                    _finishedExperimentList.Add(
                        new Tuple<string, Thing>(thing.TryGetComp<ResearchThingComp>().researchDefName, thing));
                }
                // Log.Error("Num of experiments in colony: " + thingList.Count);
            }
        }

        public override Vector2 InitialSize => new Vector2(760f, 760f);

        public override void DoWindowContents(Rect inRect)
        {
            Thing selectedExperiment = null;
            var selectedExperimentName = "";
            Pawn selectedPawn = null;
            var isExperimentSelected = false;

            var selectedRecipeDef = DefDatabase<RecipeDef>.AllDefsListForReading[0];

            //title
            var rect1 = new Rect(inRect.center.x - 70f, inRect.yMin + 35f, 200f, 74f);
            Text.Font = GameFont.Medium;
            Widgets.Label(rect1, "Study Setup");

            // Exit button
            var exitRect = new Rect(inRect.xMax - 50f, inRect.yMin + 5f, 50f, 30f);
            if (Widgets.ButtonText(exitRect, "Exit"))
            {
                Close();
            }

            // explain text
            var rect2 = new Rect(inRect)
            {
                yMin = rect1.yMax
            };
            rect2.yMax -= 38f;
            Text.Font = GameFont.Small;
            Widgets.Label(rect2,
                "Here you can choose an experiment for a colonist to study. Studying experiments will enable your colony to better retain its scientific knowledge. Any colonist can study a research project, however those with lower intellectual skills will take a much longer time to complete their studies.");

            // 'select experiment' list
            var AddExpRect = new Rect(rect2)
            {
                width = 550f //275f;
            };
            AddExpRect.height /= 2;
            AddExpRect.y += 70f;
            AddExpRect.x += 370f;


            if (_finishedExperimentList.Count == 0)
            {
                var list = new List<string> { "No experiments" };
                _expsInColony.DrawTextList(AddExpRect, list, "No experiments in Colony");
            }
            else
            {
                selectedExperiment = _expsInColony.SelectedEntry?.EntryAttachedThing;
                selectedExperimentName = _expsInColony.SelectedEntry?.EntryLabel;
                _expsInColony.DrawTextListWithAttachedThing(AddExpRect, _finishedExperimentList,
                    "All Experiments In Colony");
            }

            // 'select researcher' list
            var AddPawnRect = new Rect(AddExpRect);
            AddPawnRect.x -= 310f;
            //AddPawnRect.ContractedBy(30f);
            AddPawnRect.width = 550f; //275f;

            if (_pawnsInColony.Count == 0)
            {
                var list = new List<string> { "No researchers" };
                _expsInColony.DrawTextList(AddExpRect, list, "No researchers in Colony");
            }
            else
            {
                selectedPawn = (Pawn)_colonyResearchers.SelectedEntry?.EntryAttachedThing;
                _colonyResearchers.DrawTextListWithAttachedThing(AddPawnRect, _pawnsInColony, "Researchers In Colony");
            }


            // need to get defName of recipe from this point
            var _selectedOption = selectedPawn?.Name + " will study: " + selectedExperimentName;


            var warningText = "";
            if (selectedPawn != null)
            {
                warningText = selectedPawn.Name + " is a colony researcher with a skill of: " +
                              selectedPawn.skills.GetSkill(SkillDefOf.Intellectual).levelInt;
            }

            // text explaining selected pawn and topic
            var rect3 = new Rect(inRect.position, rect2.size)
            {
                x = inRect.center.x - 300f,
                y = inRect.yMax - 100f
            };
            Text.Font = GameFont.Medium;
            Widgets.Label(rect3, _selectedOption);

            // optional warning text for pawns with no research / already studied topic
            Text.Font = GameFont.Small;
            var rect4 = rect3;
            rect4.x = inRect.x;
            rect4.y = inRect.yMax - 210f;
            Widgets.Label(rect4, warningText);

            // get recipe def
            if (!(from t in DefDatabase<RecipeDef>.AllDefsListForReading
                where t.defName == "StudyFinishedExperiment"
                select t).TryRandomElement(out var finalDef))
            {
                //Log.Error("Def not found");
            }
            else
            {
                selectedRecipeDef = finalDef;
            }

            if (selectedPawn != null && selectedExperimentName != "")
            {
                isExperimentSelected = true;
            }

            var rect5 = rect4;
            rect5.x = inRect.x;
            rect5.y = inRect.yMax - 180f;

            // confirm button
            var rect6 = new Rect(inRect.center.x - 80f, inRect.yMax - 35f, 150f, 29f);
            if (!Widgets.ButtonText(rect6, "Confirm Study"))
            {
                return;
            }

            if (isExperimentSelected)
            {
                var newStudy = new Experiment(selectedRecipeDef);
                Bill newBill = (Bill_Production)selectedRecipeDef.MakeNewBill();

                newStudy.uniquePawnDoer = selectedPawn;
                newStudy.uniqueThingIng = selectedExperiment;
                _selectedTable.ExpStack.AddExperiment(newStudy);
                _selectedTable.ExpStack.AddExperimentWithBill(newBill);
                newBill.SetPawnRestriction(selectedPawn);
                if (selectedExperiment != null)
                {
                    selectedExperiment.def.GetModExtension<ResearchDefModExtension>()
                        .ResearchDefAttachedToExperiment = selectedExperimentName;
                }

                Close();
            }
            else
            {
                var window = Dialog_MessageBox.CreateConfirmation("No Study Option Selected", delegate { }, true);
                Find.WindowStack.Add(window);
            }
        }
    }
}