using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public static RabbitManager singleton;

    public static List<Rabbit> rabbits = new List<Rabbit>();

    [SerializeField] private Rabbit rabbitPrefab;
    [SerializeField] private float mapSize = 10.0f;
    [SerializeField] private int numberOfRabbitsOnStartup = 5;
    [SerializeField] private int ySpawnOffset = 10;
    [SerializeField] private Rabbit.Gene[] viableGenes;


    void Start()
    {
        singleton = this;

        // create x rabbits at random positions on the map (x = StartingNoOfRabbits)
        // raycast down as to spawn them on the ground
        // each rabbit has 3 modified genes: +2, -1, -1
        // all other genes are set to their default value
        for (int i = 0; i < numberOfRabbitsOnStartup; i++)
        {
            Rabbit.Gene[] copyGenes = new Rabbit.Gene[viableGenes.Length];
            for (int g = 0; g < viableGenes.Length; g++)
            {
                copyGenes[g].attribute = viableGenes[g].attribute.Duplicate();
                copyGenes[g].attribute.value = viableGenes[g].attribute.startValue;
            }

            Vector3 randPos;
            do
            {
                float randX = Random.Range(-mapSize, mapSize);
                float randZ = Random.Range(-mapSize, mapSize);
                randPos = new Vector3(randX, ySpawnOffset, randZ);
            }
            while (!Eco.isValidGround(randPos));

            // select 3 different indexes randomly
            int attribute1 = RandomIndex(new int[0]);
            int attribute2 = RandomIndex( new int[] { attribute1 });
            int attribute3 = RandomIndex( new int[] { attribute1 , attribute2});

            // edit the modifiers of the randomly selected genes
            copyGenes[attribute1].modifier = 2;
            copyGenes[attribute2].modifier = -1;
            copyGenes[attribute3].modifier = -1;

            // spawn the rabbit with the genes and position defiend above
            Rabbit r = Instantiate(singleton.rabbitPrefab, randPos, Quaternion.identity);
            r.NewRabbit(copyGenes);
            rabbits.Add(r);
        }
    }


    void Update()
    {

    }

    public enum GenderSearch
    {
        Male, Female, NoPreference
    };
    public static Rabbit RandomRabbit(GenderSearch genderPreference)
    {
        for (int i = 0; i < 100; i++)
        {
            if(genderPreference == GenderSearch.NoPreference)
                // select and return a random rabbit from rabbits list
                return rabbits[Random.Range(0, rabbits.Count)];
            else
            {
                Rabbit r = rabbits[Random.Range(0, rabbits.Count)];
                if(    r.gender == Rabbit.Gender.Male
                    && genderPreference == GenderSearch.Male)
                {
                    return r;
                }
                else if (r.gender == Rabbit.Gender.Female
                    && genderPreference == GenderSearch.Female)
                {
                    return r;
                }
            }
        }
        // if after 100 tries, no match was foun, return null
        return null;
    }

    int RandomIndex(int[] avoidedNumbers )
    {
        int error = 0;
        int rand;
        do
        {
            error++;
            rand = Random.Range(0, viableGenes.Length);

            if (error > 100) return error;
        }
        while (Contains(avoidedNumbers, rand));
        return rand;
    }

    bool Contains(int[] array, int value)
    {
        foreach(int item in array)
        {
            if (item == value) return true;
        }
        return false;
    }


    public static void CreateRabbit(Rabbit.Gene[] genes, Vector3 pos)
    {
        // instantiate a new rabit prefab at position pos
        Rabbit r = Instantiate(singleton.rabbitPrefab, pos, Quaternion.identity);
        r.NewRabbit(genes);
    }
}
