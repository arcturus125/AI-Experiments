using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public enum Gender { Male, Female};
    public Gender gender;

    public float lifetime;

    [Serializable]
    public struct Gene
    {
        public Attribute attribute;
        public float modifier;
    }
    public Gene[] genes;


    // Start is called before the first frame update
    void Start()
    {
        //// creates copy attributes to use, instead of altering the files directly
        //for (int i = 0; i < genes.Length; i++)
        //{
        //    genes[i].attribute = genes[i].attribute.Duplicate();
        //    genes[i].attribute.value = genes[i].attribute.startValue;
        //}
    }

    // run once when this rabbit is created to pass down the genes of the parents
    public void NewRabbit(Gene[] newGenes)
    {
        genes = newGenes;
    }

    void Birth(Rabbit otherParent)
    {
        // calculate child's genes
        List<Gene> newGenes = new List<Gene>();

        for (int i = 0; i < genes.Length; i++)
        {
            for (int J = 0; J < otherParent.genes.Length; J++)
            {
                if( genes[i].attribute.name == otherParent.genes[J].attribute.name)
                {
                    Gene g = new Gene();
                    g.attribute = genes[i].attribute.Duplicate();
                    g.modifier = genes[i].modifier + otherParent.genes[J].modifier;
                    newGenes.Add(g);
                }
            }
        }
        Gene[] childGenes = newGenes.ToArray();

        // create new rabbit with these genes
        RabbitManager.CreateRabbit(childGenes, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
    }
}
