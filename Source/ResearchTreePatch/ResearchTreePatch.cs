using System.Reflection;
using HarmonyLib;
using Verse;

namespace ImmersiveIshResearch;

[StaticConstructorOnStartup]
public class ResearchTreePatch
{
    static ResearchTreePatch()
    {
        var harmony = new Harmony("Mlie.ImmersiveIshResearch.ResearchTree");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        Log.Message("[ImmersiveishResearch]: Patched for ResearchTree");
    }
}