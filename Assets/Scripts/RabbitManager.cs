using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitManager : MonoBehaviour
{
    public Rabbit rabbitPrefab;
    public static RabbitManager singleton;

    public int StartingNoOfRabbits;

    // Start is called before the first frame update
    void Start()
    {
        singleton = this;

        // create x rabbits at random positions on the map (x = StartingNoOfRabbits)
        // raycast down as to spawn them on the ground
          // each rabbit has 3 modified genes: +2, -1, -1
          // all other genes are set to their default value

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void CreateRabbit(Rabbit.Gene[] genes, Vector3 pos)
    {
        // instantiate a new rabit prefab at position pos
        Rabbit r = Instantiate(singleton.rabbitPrefab, pos, Quaternion.identity);
        r.NewRabbit(genes);
    }
}
