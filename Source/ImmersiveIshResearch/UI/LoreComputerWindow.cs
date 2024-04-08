using UnityEngine;
using Verse;

namespace ImmersiveResearch;
// UNUSED CLASS

public class LoreComputerWindow : Window
{
    public LoreComputerWindow()
    {
        forcePause = true;
        absorbInputAroundWindow = true;
        onlyOneOfTypeAllowed = false;

        InitialiseThingList();
    }

    // sets initial size of the window
    public override Vector2 InitialSize => new Vector2(950f, 760f);


    private void InitialiseThingList()
    {
    }

    private void CreateLoreWindow(Rect inRect)
    {
        var rect1 = new Rect(inRect).ContractedBy(18f);
        rect1.height = 74f;
        rect1.width = 100f;
        Text.Font = GameFont.Medium;
        Widgets.Label(rect1, "Lore Database Test");
        var rect2 = new Rect(inRect)
        {
            yMin = rect1.yMax
        };
        rect2.yMax -= 38f;
        var rect3 = rect2;
        rect3.yMin += 45f;

        CreateThingListWindow();
    }

    private void CreateThingListWindow()
    {
        //LoreWindowDrawingUtility.DrawLoreFullList(refRect, ThingList);
    }

    // UI-based update
    public override void DoWindowContents(Rect inRect)
    {
        CreateLoreWindow(inRect);
    }
}