using System.Reflection;
using HarmonyLib;
using Verse;

namespace ImmersiveIshResearch;

[StaticConstructorOnStartup]
public class ResearchPalPatch
{
    static ResearchPalPatch()
    {
        new Harmony("Mlie.ImmersiveIshResearch.ResearchPal").PatchAll(Assembly.GetExecutingAssembly());
        Log.Message("[ImmersiveishResearch]: Patched for ResearchPal");
    }
}