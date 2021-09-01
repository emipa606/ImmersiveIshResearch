using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    public class ExperimentStack : BillStack
    {
        private List<Experiment> _experiments = new List<Experiment>();

        public ExperimentStack(IBillGiver giver) : base(giver)
        {
            billGiver = giver;
        }

        public List<Experiment> Experiments => _experiments;

        public int ListCount => _experiments.Count;

        public void AddExperiment(Experiment exp)
        {
            exp.expStack = this;
            _experiments.Add(exp);
        }

        public void AddExperimentWithBill(Bill exp)
        {
            billGiver.BillStack.AddBill(exp);
        }

        public void Delete(Experiment exp)
        {
            var expBill = billGiver.BillStack.Bills[_experiments.IndexOf(exp)];

            if (expBill != null)
            {
                billGiver.BillStack.Delete(expBill);
            }
            else
            {
                Log.Error(
                    "Bill could not be found. Assuming that bill was deleted before it could be checked by ExperimentStack.");
            }

            exp.deleted = true;
            _experiments.Remove(exp);
        }

        public new void RemoveIncompletableBills()
        {
            for (var num = _experiments.Count - 1; num >= 0; num--)
            {
                if (!_experiments[num].CompletableEver)
                {
                    _experiments.Remove(_experiments[num]);
                }
            }
        }

        public new void Clear()
        {
            _experiments.Clear();
            billGiver.BillStack.Clear();
        }

        public void Reorder(Experiment exp, int offset)
        {
            var num = _experiments.IndexOf(exp);
            var expBill = billGiver.BillStack.Bills[num];

            num += offset;
            if (num >= 0)
            {
                _experiments.Remove(exp);
                _experiments.Insert(num, exp);
            }

            if (expBill != null)
            {
                billGiver.BillStack.Reorder(expBill, offset);
            }
        }

        public int IndexOf(Experiment exp)
        {
            return _experiments.IndexOf(exp);
        }

        public int IndexOfBillToExp(Bill bill)
        {
            return billGiver.BillStack.IndexOf(bill);
        }

        public void SetSuspended(Experiment exp, bool flag)
        {
            var index = _experiments.IndexOf(exp);
            var expBill = billGiver.BillStack.Bills[index];
            expBill.suspended = flag;
        }

        private void CheckIfExperimentCompleted(Experiment exp)
        {
            var expBill = billGiver.BillStack.Bills[_experiments.IndexOf(exp)];

            if (expBill == null)
            {
                Delete(exp);
            }
        }

        public new void ExposeData()
        {
            Scribe_Collections.Look(ref _experiments, "Experiments", LookMode.Deep);

            if (Scribe.mode != LoadSaveMode.ResolvingCrossRefs)
            {
                return;
            }

            if (_experiments.RemoveAll(x => x == null) != 0)
            {
                Log.Error("Some experiments were null after loading.");
            }

            foreach (var experiment in _experiments)
            {
                experiment.expStack = this;
            }
        }

        public Experiment DoExperimentListing(Rect rect, Building_ExperimentBench selTable, ref Vector2 scrollPosition,
            ref float viewHeight)
        {
            foreach (var experiment in _experiments)
            {
                CheckIfExperimentCompleted(experiment);
            }

            Experiment result = null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if (ListCount < 15)
            {
                var rect2 = new Rect(rect.width / 4, rect.y, 150f, 29f);
                rect2.x += 10f;
                if (Widgets.ButtonText(rect2, "Perform Experiment"))
                {
                    Find.WindowStack.Add(new Dialog_ExperimentConfig(selTable));
                }
            }

            // draw Experiment Entry In List
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            var outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
            var viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            var num = 5f;

            for (var i = 0; i < ListCount; i++)
            {
                var exp = _experiments[i];
                var rect3 = exp.DoInterface(0f, num, viewRect.width, i);
                if (!exp.DeletedOrDereferenced && Mouse.IsOver(rect3))
                {
                    result = exp;
                }

                num += rect3.height + 6f;
            }

            if (Event.current.type == EventType.Layout)
            {
                viewHeight = num + 60f;
            }

            Widgets.EndScrollView();
            GUI.EndGroup();
            return result;
        }

        public Experiment DoStudyListing(Rect rect, Building_StudyTable selTable, ref Vector2 scrollPosition,
            ref float viewHeight)
        {
            foreach (var experiment in _experiments)
            {
                CheckIfExperimentCompleted(experiment);
            }

            Experiment result = null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if (ListCount < 15)
            {
                var rect2 = new Rect(rect.width / 4, rect.y, 150f, 29f);
                rect2.x += 10f;
                if (Widgets.ButtonText(rect2, "Study Experiment"))
                {
                    Find.WindowStack.Add(new Dialog_StudyConfig(selTable));
                }
            }

            // draw Experiment Entry In List
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            var outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
            var viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            var num = 5f;

            for (var i = 0; i < ListCount; i++)
            {
                var exp = _experiments[i];
                var rect3 = exp.DoInterface(0f, num, viewRect.width, i);
                if (!exp.DeletedOrDereferenced && Mouse.IsOver(rect3))
                {
                    result = exp;
                }

                num += rect3.height + 6f;
            }

            if (Event.current.type == EventType.Layout)
            {
                viewHeight = num + 60f;
            }

            Widgets.EndScrollView();
            GUI.EndGroup();
            return result;
        }
    }
}