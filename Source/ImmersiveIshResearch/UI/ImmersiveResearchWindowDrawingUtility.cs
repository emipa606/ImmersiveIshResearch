using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    /// <summary>
    ///     Custom implementation of RimWorld class 'StatsReportUtility'.
    ///     We are using the previously mentioned class' functionality to allow the player to click on list entries.
    /// </summary>
    public class ImmersiveResearchWindowDrawingUtility
    {
        private readonly List<ImmersiveResearchDrawEntry> _cachedDrawEntries = new List<ImmersiveResearchDrawEntry>();
        private readonly List<Rect> testRectList = new List<Rect>();
        private float _listHeight;
        private ImmersiveResearchDrawEntry _mousedOverEntry;

        private Vector2 _scrollPosition;

        public ImmersiveResearchDrawEntry SelectedEntry { get; private set; }

        public void InitImageList(List<ExperimentConfigUIElement> images)
        {
            // setup table layout
            float rowHeight = images[0].UIIcon.height / 3;
            float columnWidth = images[0].UIIcon.width / 3;
            var curY = 22.0f; // offset for text labels 

            for (var index = 0; index < images.Count; index += 2)
            {
                var r1 = new Rect(15f, curY, rowHeight, columnWidth);
                testRectList.Add(r1);
                var r2 = new Rect(r1.xMax + 35f, curY, rowHeight, columnWidth);
                testRectList.Add(r2);

                curY += r1.height + 22f;
            }

            if (!_cachedDrawEntries.NullOrEmpty())
            {
                return;
            }

            foreach (var experimentConfigUiElement in images)
            {
                var newEntry =
                    new ImmersiveResearchDrawEntry(experimentConfigUiElement.PartialDefName,
                        experimentConfigUiElement.UIText, experimentConfigUiElement.UIIcon);
                _cachedDrawEntries.Add(newEntry);
            }
        }

        public void DrawTextList(Rect inRect, List<string> thingList, string listTitle)
        {
            if (_cachedDrawEntries.NullOrEmpty())
            {
                foreach (var str in thingList)
                {
                    var desc = "";
                    var newEntry = new ImmersiveResearchDrawEntry(str, desc);
                    _cachedDrawEntries.Add(newEntry);
                }
            }

            DrawListWorker(inRect, listTitle, false);
        }

        public void DrawTextListWithAttachedThing(Rect inRect, List<Tuple<string, Thing>> thingList, string listTitle)
        {
            if (_cachedDrawEntries.NullOrEmpty())
            {
                foreach (var str in thingList)
                {
                    var label = str.Item1;
                    var newEntry = new ImmersiveResearchDrawEntry(label, str.Item2);
                    _cachedDrawEntries.Add(newEntry);
                }
            }

            DrawListWorker(inRect, listTitle, false);
        }

        public void DrawImageList(Rect inRect, string listTitle)
        {
            DrawListWorker(inRect, listTitle, true);
        }


        // Draw our list to the UI
        // mostly just ripped from original source code, jsut changed in a few areas for my needs
        private void DrawListWorker(Rect refRect, string listTitle, bool hasImage)
        {
            var titleRect = new Rect(refRect)
            {
                x = refRect.xMin + 50f,
                y = refRect.yMin
            };
            Widgets.Label(titleRect, listTitle);

            var rect1 = new Rect(refRect);
            rect1.width *= 0.5f;
            rect1.y += 25f;
            var rect2 = new Rect(refRect)
            {
                x = rect1.xMax
            };
            rect2.width = refRect.xMax - rect2.x;
            Text.Font = GameFont.Small;
            var viewRect = new Rect(0.0f, 0.0f, rect1.width - 16f, _listHeight);
            Widgets.DrawMenuSection(rect1);
            Widgets.BeginScrollView(rect1, ref _scrollPosition, viewRect);
            var curY = 0.0f;

            _mousedOverEntry = null;

            if (hasImage)
            {
                for (var i = 0; i < _cachedDrawEntries.Count; ++i)
                {
                    Action<ImmersiveResearchDrawEntry> mouseClickEvent = MouseClickCallBackEvent;
                    Action<ImmersiveResearchDrawEntry> mouseOverEvent = MouseOverCallBackEvent;
                    var drawEntry = _cachedDrawEntries[i];
                    curY += drawEntry.DrawImage(testRectList[i].x, testRectList[i].y,
                        drawEntry.EntryLabel == SelectedEntry?.EntryLabel,
                        viewRect.width - 8f, drawEntry.EntryImage, mouseClickEvent, mouseOverEvent,
                        _scrollPosition, rect1);
                }
            }
            else
            {
                foreach (var immersiveResearchDrawEntry in _cachedDrawEntries)
                {
                    Action<ImmersiveResearchDrawEntry> mouseClickEvent = MouseClickCallBackEvent;
                    Action<ImmersiveResearchDrawEntry> mouseOverEvent = MouseOverCallBackEvent;

                    curY += immersiveResearchDrawEntry.Draw(8f, curY, viewRect.width - 8f,
                        immersiveResearchDrawEntry.EntryLabel == SelectedEntry?.EntryLabel, mouseClickEvent,
                        mouseOverEvent, _scrollPosition, rect1);
                }
            }


            _listHeight = curY;
            Widgets.EndScrollView();
            // Rect rect3 = rect2.ContractedBy(10f);
            //GUI.BeginGroup(rect3);
            var loreEntry = SelectedEntry ?? _mousedOverEntry ?? _cachedDrawEntries.FirstOrDefault();

            if (loreEntry != null)
            {
                // draws text on the right side 
                //Widgets.Label(rect3.AtZero(), loreEntry.EntryBasicDesc);
            }
            //GUI.EndGroup();
        }


        private void SelectEntry(ImmersiveResearchDrawEntry entry, bool playSound = true)
        {
            SelectedEntry = entry;
            if (!playSound)
            {
            }
        }


        // Mouse events 
        private void MouseClickCallBackEvent(ImmersiveResearchDrawEntry r)
        {
            SelectEntry(r);
        }

        private void MouseOverCallBackEvent(ImmersiveResearchDrawEntry r)
        {
            _mousedOverEntry = r;
        }
    }
}