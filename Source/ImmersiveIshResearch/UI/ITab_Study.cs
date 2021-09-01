using RimWorld;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    public class ITab_Study : ITab_Bills
    {
        private static readonly Vector2 _winSize = new Vector2(420f, 480f);

        private Experiment _mouseoverBill;

        private Vector2 _scrollPosition;
        private float _viewHeight = 1000f;

        public ITab_Study()
        {
            size = _winSize;
            labelKey = "TabBills";
            tutorTag = "Study";
        }

        protected Building_StudyTable SelectedStudyTable => (Building_StudyTable)SelThing;

        protected override void FillTab()
        {
            var windowSize = _winSize;
            var rect1 = new Rect(0f, 0f, windowSize.x, windowSize.y).ContractedBy(10f);
            _mouseoverBill =
                SelectedStudyTable.ExpStack.DoStudyListing(rect1, SelectedStudyTable, ref _scrollPosition,
                    ref _viewHeight);
        }
    }
}