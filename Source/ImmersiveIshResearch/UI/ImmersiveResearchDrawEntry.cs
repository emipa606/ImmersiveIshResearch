using System;
using UnityEngine;
using Verse;

namespace ImmersiveResearch
{
    public class ImmersiveResearchDrawEntry
    {
        private Pawn _EntryAttachedPawn;

        public ImmersiveResearchDrawEntry(string label, string basicDescription)
        {
            EntryLabel = label;
            EntryBasicDesc = basicDescription;
        }

        public ImmersiveResearchDrawEntry(string label, string basicDesc, Texture2D image)
        {
            EntryLabel = label;
            EntryBasicDesc = basicDesc;
            EntryImage = image;
        }

        public ImmersiveResearchDrawEntry(string label, Thing thing)
        {
            EntryLabel = label;
            EntryAttachedThing = thing;
        }

        public string EntryLabel { get; }

        public string EntryBasicDesc { get; }

        public Texture2D EntryImage { get; }

        public Thing EntryAttachedThing { get; }

        public float DrawImage(float x, float y, bool selected, float width, Texture2D image,
            Action<ImmersiveResearchDrawEntry> clickedCallback, Action<ImmersiveResearchDrawEntry> mousedOverCallback,
            Vector2 scrollPosition, Rect scrollOutRect)
        {
            var width1 = width * 0.45f;
            var height1 = y - 22;
            var labelRect = new Rect(x, height1, width, Text.CalcHeight("test", width1));
            var imgRect = new Rect(x, y, image.width / 3, image.height / 3);
            //imgRect.y += labelRect.height;
            //Widgets.ButtonImage(imgRect, image, false); 

            // check for mouse pos and input 
            if (!(y - (double)scrollPosition.y + imgRect.height >= 0.0) ||
                !(y - (double)scrollPosition.y <= scrollOutRect.height))
            {
                return imgRect.height;
            }

            if (selected)
            {
                Widgets.DrawHighlightSelected(imgRect);
            }
            else if (Mouse.IsOver(imgRect))
            {
                Widgets.DrawHighlight(imgRect);
            }

            var rect2 = labelRect;
            rect2.width -= width1;
            Widgets.Label(rect2, EntryBasicDesc);

            if (Widgets.ButtonImage(imgRect, image))
            {
                clickedCallback(this);
            }

            if (Mouse.IsOver(imgRect))
            {
                mousedOverCallback(this);
            }

            // imgRect.height += labelRect.height;
            return imgRect.height;
        }

        public float Draw(float x, float y, float width, bool selected,
            Action<ImmersiveResearchDrawEntry> clickedCallback, Action<ImmersiveResearchDrawEntry> mousedOverCallback,
            Vector2 scrollPosition, Rect scrollOutRect)
        {
            var width1 = width * 0.45f;
            var rect1 = new Rect(x, y, width, Text.CalcHeight("test", width1));
            if (!(y - (double)scrollPosition.y + rect1.height >= 0.0) ||
                !(y - (double)scrollPosition.y <= scrollOutRect.height))
            {
                return rect1.height;
            }

            if (selected)
            {
                Widgets.DrawHighlightSelected(rect1);
            }
            else if (Mouse.IsOver(rect1))
            {
                Widgets.DrawHighlight(rect1);
            }

            var rect2 = rect1;
            rect2.width -= width1;
            Widgets.Label(rect2, EntryLabel);

            // this 3rd rect is used for entry specific statistics like percentages and in game values.
            /*Rect rect3 = rect1;
                rect3.x = rect2.xMax;
                rect3.width = width1;
                Widgets.Label(rect3, EntryBasicDesc);*/


            if (Widgets.ButtonInvisible(rect1, false))
            {
                clickedCallback(this);
            }

            if (Mouse.IsOver(rect1))
            {
                mousedOverCallback(this);
            }

            return rect1.height;
        }
    }
}