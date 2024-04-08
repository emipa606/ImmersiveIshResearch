using Verse;

namespace ImmersiveResearch;

internal class ResearchThingComp : ThingComp
{
    public string pawnExperimentAuthorName;
    public ResearchProjectDef researchDef;
    public string researchDefName;
    public ResearchCompProperties Props => (ResearchCompProperties)props;

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