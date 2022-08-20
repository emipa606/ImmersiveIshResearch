using Verse;

namespace ImmersiveResearch;

internal class ResearchThingComp : ThingComp
{
    public string pawnExperimentAuthorName;
    public ResearchProjectDef researchDef;
    public string researchDefName;
    public ResearchCompProperties Properties => Properties;

    public void AddPawnAuthor(string author)
    {
        pawnExperimentAuthorName = author;
    }

    public void AddResearch(string projDef)
    {
        //Properties.researchDef = projDef;
        researchDefName = projDef;
    }
}