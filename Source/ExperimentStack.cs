﻿using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    public class ExperimentStack : IExposable
    {
        [Unsaved]
        public IBillGiver BillGiver;

        public ExperimentStack(IBillGiver giver)
        {
            BillGiver = giver;
        }

        private List<Experiment> _experiments = new List<Experiment>();

        private const float _topAreaHeight = 35f;

        private const float _expInterfaceSpacing = 6f;

        private const float _extraViewHeight = 60f;

        public List<Experiment> Experiments => _experiments;

        public Experiment this[int index] => _experiments[index];

        public const int MaxCount = 15;

        public int ListCount => _experiments.Count;

        public void AddExperiment(Experiment exp)
        {
            exp.expStack = this;
            _experiments.Add(exp);           
        }

        public void Delete(Experiment exp)
        {
            Bill expBill = BillGiver.BillStack.Bills[_experiments.IndexOf(exp)];

            if (expBill != null)
            {
                BillGiver.BillStack.Delete(expBill);
            }
            else
            {
                Log.Error("bill is null");
            }
            exp.deleted = true;
            _experiments.Remove(exp);
        }

        public void Clear()
        {
            _experiments.Clear();
            BillGiver.BillStack.Clear();
        }

        public void Reorder(Experiment exp, int offset)
        {
            int num = _experiments.IndexOf(exp);
            num += offset;
            if(num >= 0)
            {
                _experiments.Remove(exp);
                _experiments.Insert(num, exp);
            }

            Bill expBill = BillGiver.BillStack.Bills[num];

            if (expBill != null)
            {
                BillGiver.BillStack.Reorder(expBill, offset);
            }
        }

        public int IndexOf(Experiment exp)
        {
            return _experiments.IndexOf(exp);
        }

        public int IndexOfBillToExp(Bill bill)
        {
            return BillGiver.BillStack.IndexOf(bill);
        }

        public void SetSuspended(Experiment exp, bool flag)
        {
            int index = _experiments.IndexOf(exp);
            Bill expBill = BillGiver.BillStack.Bills[index];
            expBill.suspended = flag;
        }

        private void CheckIfExperimentCompleted(Experiment exp)
        {
            Bill expBill = BillGiver.BillStack.Bills[_experiments.IndexOf(exp)];

            if(expBill == null)
            {
               // Log.Error("exp is completed");
                Delete(exp);
            }
            else
            {
               // Log.Error("exp is not completed");
            }
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref _experiments, "Experiments", LookMode.Deep);

            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                if (_experiments.RemoveAll((Experiment x) => x == null) != 0)
                {
                    Log.Error("Some experiments were null after loading.");
                }
                for (int i = 0; i < _experiments.Count; i++)
                {
                    _experiments[i].expStack = this;
                }
            }

            if (Scribe.mode == LoadSaveMode.LoadingVars)
            {
            }
        }

        public Experiment DoListing(Rect rect, Building_ExperimentBench selTable, ref Vector2 scrollPosition, ref float viewHeight)
        {
            for(int i = 0; i < _experiments.Count; i++)
            {
                CheckIfExperimentCompleted(_experiments[i]);
            }

            Experiment result = null;
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            if(ListCount < 15)
            {
                Rect rect2 = new Rect(rect.width / 4, rect.y, 150f, 29f);
                if (Widgets.ButtonText(rect2, "Perform Experiment"))
                {
                    Find.WindowStack.Add(new Dialog_ExperimentConfig(selTable));
                }
            }
            // draw Experiment Entry In List
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            float num = 5f;
            for(int i = 0; i < ListCount; i++)
            {
                Experiment exp = _experiments[i];
                Rect rect3 = exp.DoInterface(0f, num, viewRect.width, i);
                if(!exp.DeletedOrDereferenced && Mouse.IsOver(rect3))
                {
                    result = exp;
                }
                num += rect3.height + 6f;
            }
            if(Event.current.type == EventType.Layout)
            {
                viewHeight = num + 60f;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();
            return result;
        }



    }
}
