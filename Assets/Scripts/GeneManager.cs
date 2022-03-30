using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneManager : MonoBehaviour
{
    static Gene eyesight = new Gene("Eyesight", 7.0f, 21.0f, false);
    static Gene speed = new Gene("Speed", 5.0f, 10.0f, false);
    static SimpleGene hunger = new SimpleGene("Hunger", 1);
    static SimpleGene thirst = new SimpleGene("Thirst", 1);
    static SimpleGene reproductiveUrge = new SimpleGene("reproductiveUrge", 1);

    public static Gene[] GetViableGenes()
    {
        List<Gene> geneList = new List<Gene>();

        geneList.Add(eyesight);
        geneList.Add(speed);
        geneList.Add(hunger);
        geneList.Add(thirst);
        geneList.Add(reproductiveUrge);


        return geneList.ToArray();
    }
    public static Gene GetGeneFromName(Gene[] geneList, string name)
    {
        for (int i = 0; i < geneList.Length; i++)
        {
            if (geneList[i].name == name) return geneList[i];
        }
        return null;
    }

}
