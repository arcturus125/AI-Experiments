using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private float timeBetweenSpawns = 5.0f;
    [SerializeField] private float mapSize = 10.0f;

    [SerializeField] private GameObject[] foodPrefabs;

    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenSpawns)
        {
            timer = 0;
            Vector3 randPos;
            Vector3 groundPos;
            do
            {
                float randX = Random.Range(-mapSize, mapSize);
                float randZ = Random.Range(-mapSize, mapSize);
                randPos = new Vector3(randX, 40, randZ);
            }
            while (!Eco.isValidGround(randPos, out groundPos));

            int rand = Random.Range(0, foodPrefabs.Length);
            GameObject food = Instantiate(foodPrefabs[rand], groundPos, Quaternion.identity);

        }
    }
}
