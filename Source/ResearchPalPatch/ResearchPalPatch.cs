using System.Reflection;
using HarmonyLib;
using Verse;

namespace ImmersiveIshResearch;

[StaticConstructorOnStartup]
public class ResearchPalPatch
{
    static ResearchPalPatch()
    {
        var harmony = new Harmony("Mlie.ImmersiveIshResearch.ResearchPal");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Log.Message("[ImmersiveishResearch]: Patched for ResearchPal");
    }
}