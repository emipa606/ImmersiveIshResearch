using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace ImmersiveResearch
{
    public class JobDriver_TakeExperimentFromCabinet : JobDriver
    {
        public const TargetIndex CabinetIndex = TargetIndex.A;
        public const TargetIndex ExperimentIndex = TargetIndex.B;
        public const TargetIndex HeldExperimentIndex = TargetIndex.C;
        private Thing _currentFinishedExperiment;
        private Thing _heldThing = null;

        public Thing RequestedExperiment;
        private Building _cabinet => (Building)TargetThingA;

        public void SetRequestedExperiment(Thing thing)
        {
            RequestedExperiment = thing;
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            job.SetTarget(TargetIndex.A, _cabinet);
            return pawn.Reserve(job.GetTarget(TargetIndex.A), job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDespawnedNullOrForbidden(CabinetIndex);

            var cabinet = job.GetTarget(TargetIndex.A).Thing as Building_ExperimentFilingCabinet;

            var thing = LoreComputerHarmonyPatches.TempRequestedExp;

            var curExp = thing as Thing_FinishedExperiment;
            _currentFinishedExperiment = curExp;

            yield return Toils_Goto.GotoThing(CabinetIndex, PathEndMode.Touch);

            pawn.CurJob.count = 1;
            pawn.CurJob.haulMode = HaulMode.ToCellStorage;

            // perform work toil
            var TakeExperiment = new Toil
            {
                initAction = delegate
                {
                    var newThing =
                        cabinet?.TakeExperimentFromCabinet(curExp.TryGetComp<ResearchThingComp>().researchDefName);

                    var finalThing = GenSpawn.Spawn(newThing?.def, _cabinet.Position, _cabinet.Map);
                    finalThing.TryGetComp<ResearchThingComp>().pawnExperimentAuthorName =
                        newThing.TryGetComp<ResearchThingComp>().pawnExperimentAuthorName;
                    finalThing.TryGetComp<ResearchThingComp>().researchDefName =
                        newThing.TryGetComp<ResearchThingComp>().researchDefName;
                }
            };

            yield return TakeExperiment;
        }
    }
}