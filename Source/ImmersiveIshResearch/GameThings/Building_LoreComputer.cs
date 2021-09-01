using RimWorld;
using Verse;

namespace ImmersiveResearch
{
    internal class Building_LoreComputer : Building_WorkTable
    {
        private CompGlower _glowerComp;

        public CompPowerTrader PowerComponent { get; private set; }


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);

            PowerComponent = GetComp<CompPowerTrader>();
            _glowerComp = GetComp<CompGlower>();
        }

        // deprecated
        public Thing GetLocationOfOwnedThing(string defOfThing)
        {
            var thingList = Map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
            foreach (var currentThing in thingList)
            {
                if (currentThing.def.defName != defOfThing)
                {
                    continue;
                }

                if (currentThing.IsForbidden(Find.FactionManager.OfPlayer))
                {
                    continue;
                }

                return currentThing;
            }

            return null;
        }


        public bool CheckIfLinkedToResearchBench()
        {
            // just loop through all potential linkable objects and check that they are connected (by checking the linked objects list of linked objects).
            var props = def.GetCompProperties<CompProperties_Facility>();
            if (props.linkableBuildings == null)
            {
                return false;
            }

            foreach (var thingDef in props.linkableBuildings)
            {
                foreach (var item in Map.listerThings.ThingsOfDef(thingDef))
                {
                    var affectedComp = item.TryGetComp<CompAffectedByFacilities>();
                    foreach (var thing in affectedComp.LinkedFacilitiesListForReading)
                    {
                        if (thing == this)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}