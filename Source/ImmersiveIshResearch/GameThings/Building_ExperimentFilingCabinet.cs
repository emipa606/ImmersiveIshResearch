using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace ImmersiveResearch
{
    public class Building_ExperimentFilingCabinet : Building
    {
        private List<string> experimentDefsForScribing;
        private Dictionary<string, Thing> experimentsInCabinet;
        private List<Thing> experimentThingsForScribing;

        private List<string>
            researchDefsInCabinet; // TODO add case for when Thing list cant get required thing, use def here instead

        public Building_ExperimentFilingCabinet()
        {
            researchDefsInCabinet = new List<string>();
            experimentsInCabinet = new Dictionary<string, Thing>();
        }

        public int ListCount => researchDefsInCabinet.Count;
        public Dictionary<string, Thing> CabinetThings => experimentsInCabinet;

        public override string GetInspectString()
        {
            var sBuilder = new StringBuilder();

            sBuilder.Append("Number of stored Experiments" + ": ");
            sBuilder.Append(experimentsInCabinet.Count);
            return sBuilder.ToString();
        }

        public void AddExperimentToCabinet(Thing newExp)
        {
            var experiment = (Thing_FinishedExperiment)newExp;
            var expComp = experiment.TryGetComp<ResearchThingComp>();

            if (expComp == null)
            {
                throw new NullReferenceException(
                    "finished experiment comp is null. Something must've gone wrong in crafting process. Or non crafted Thing is being used.");
            }

            if (researchDefsInCabinet.Count > 0)
            {
                foreach (var defNames in researchDefsInCabinet)
                {
                    if (expComp.researchDefName == defNames)
                    {
                        // Log.Error("research def already exists in cabinet");
                        return;
                    }
                }
            }

            researchDefsInCabinet.Add(expComp.researchDefName);
            experimentsInCabinet.Add(expComp.researchDefName, experiment);
        }

        public Thing TakeExperimentFromCabinet(string defToTake)
        {
            if (!experimentsInCabinet.ContainsKey(defToTake))
            {
                return null;
            }

            experimentsInCabinet[defToTake].def.GetModExtension<ResearchDefModExtension>().ExperimentHasBeenMade =
                true;
            experimentsInCabinet[defToTake].def.GetModExtension<ResearchDefModExtension>().ResearchSize =
                ResearchSizes.None;
            experimentsInCabinet[defToTake].def.GetModExtension<ResearchDefModExtension>().researchTypes.Clear();

            var newThing = ThingMaker.MakeThing(experimentsInCabinet[defToTake].def) as Thing_FinishedExperiment;

            var comp = newThing.TryGetComp<ResearchThingComp>();
            if (comp != null)
            {
                comp.AddPawnAuthor(experimentsInCabinet[defToTake].TryGetComp<ResearchThingComp>()
                    .pawnExperimentAuthorName);
                comp.AddResearch(experimentsInCabinet[defToTake].TryGetComp<ResearchThingComp>().researchDefName);
            }

            Thing result = newThing;

            experimentsInCabinet.Remove(defToTake);

            return result;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Collections.Look(ref researchDefsInCabinet, "researchDefsInCabinet", LookMode.Value);
            // find way to suppress 'destroyed thing scribe' warning or way to use lookmode reference with collections
            // stored things are technically destroyed 
            Scribe_Collections.Look(ref experimentsInCabinet, "experimentsInCabinet", LookMode.Value, LookMode.Deep,
                ref experimentDefsForScribing, ref experimentThingsForScribing);
        }
    }
}