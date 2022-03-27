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
        // when a rabbit is created, the first frame, decide the gender of the rabbit by flipping a coin
        int coinflip = UnityEngine.Random.Range(1, 3);
        if (coinflip == 1) gender = Gender.Male;
        else gender = Gender.Female;
    }

    void OnDestroy()
    {
        // when the rabbit object is destroyed, remove it from the list of rabbits
        Debug.Log("destorying rabbit");
        RabbitManager.rabbits.Remove(this);
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
        RabbitManager.CreateRabbit(childGenes, 
            this.transform.position + new Vector3(5,0,5));
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.G))
        {
            if (gender == Gender.Female)
            {
                Rabbit r = RabbitManager.RandomRabbit(RabbitManager.GenderSearch.Male);
                Birth(r);
            }
        }
    }

    /* The majority of the calcuations are done on a 2 dimentional flat plane
     * this function converts a Vector3 to a vector2, by dropping the y component
     */
    Vector2 ToVector2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    void Movement( Vector3 target)
    {
        Vector2 vectorToTarget = ToVector2(target - this.transform.position );

        // do a dot product with this object's local forward vector.
        // if the answer is greater than 0, then the target is infront of this object
        // if the answer is < 0 then the target is behind this object
        if(Vector2.Dot(this.transform.forward,vectorToTarget) > 0)
        {
            // target is infront

        }
        else
        {
            // target is behind

        }

        // do the same with this object's local forward right vector.
        // if answer > 0 then target is right and the rabbit needs to turn right
        // if answer < 0 then target is left and rabbit needs to turn left
        if (Vector2.Dot(this.transform.right, vectorToTarget) > 0)
        {
            // target is right

        }
        else
        {
            // target is left

        }
    }
}
