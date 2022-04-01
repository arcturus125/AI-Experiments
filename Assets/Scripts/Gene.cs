using System;
using UnityEngine;

[Serializable]
public class Gene
{
    public string name;
    public float value;
    public float cap;
    public bool decisionFactor;

    [SerializeField] private float modfier;
    private float originalValue;

    public enum Objective { HigherIsBetter, LowerIsBetter };
    public Objective objective;

    public enum Genetype { Gene, SimpleGene, Fear};
    public Genetype type;
    public Gene(string geneName, float defaultValue, float valueCap, bool isDecicionFactor)
    {
        type = Genetype.Gene;

        name = geneName;
        value = defaultValue;
        originalValue = defaultValue;
        cap = valueCap;
        decisionFactor = isDecicionFactor;
    }
    //copy constructor
    public Gene(Gene prevGene)
    {
        type = Genetype.Gene;

        name = prevGene.name;
        value = prevGene.originalValue;
        originalValue = prevGene.originalValue;
        cap = prevGene.cap;
        decisionFactor = prevGene.decisionFactor;
    }

    /* +4 modifier = value * 5
     * +3 modifier = value * 4
     * +2 modifier = value * 3
     * +1 modifier = value * 2
     * +0 modifier = value
     * -1 modifier = value / 2
     * -2 modifier = value / 3
     * -3 modifier = value / 4
     * -4 modifier = value / 5
     */
    public virtual void SetModifier(float newModifier) 
    {
        if (newModifier < 0)
            newModifier = 1 / (-newModifier + 1);
        modfier = newModifier;
        value = originalValue * (modfier+1);
    }
    public float GetModifier() { return modfier; }

    public virtual void Update()
    {

    }

    public static Gene Copy(Gene oldGene)
    {
        Gene g = new Gene(oldGene);
        if (oldGene.type == Gene.Genetype.SimpleGene)
            g = new SimpleGene(oldGene as SimpleGene);
        else if (oldGene.type == Gene.Genetype.Fear)
            g = new Fear(oldGene as Fear);
        return g;
    }

}
public class SimpleGene : Gene
{
    public float rate;

    public SimpleGene(string geneName, float defaultRate, float cap = 100) : base(geneName, 0,cap, true)
    {
        type = Genetype.SimpleGene;

        rate = defaultRate;
    }
    //copy constructor
    public SimpleGene(SimpleGene prevGene) : base(prevGene)
    {
        type = Genetype.SimpleGene;

        rate = prevGene.rate;
    }


    public override void Update()
    {
        if (value < cap)
            value += rate * Time.deltaTime;
        else if(value > cap)
            value = cap;
    }

    
}
public class Fear : Gene
{

    public Fear(string geneName, float valueCap) : base(geneName, 0, valueCap, true)
    {
    }
    public Fear(Fear prevGene) : base(prevGene)
    {

    }

    public override void Update()
    {
        // as a fox gets closer, the fear increases
        // this is multiplied by the modifer /10
        // the count starts at 0 when a fox is within eyesight.
        // the count reaches cap when the fox and rabbit occupy the same space.



    }
}
