using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public float timeToGrow = 1.0f;
    public int numUses = 1;
    int uses;


    float growth = 0;


    // Start is called before the first frame update
    void Start()
    {
        uses = numUses;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (growth < timeToGrow)
        {
            growth += Time.deltaTime;
            float percent = growth / timeToGrow;
            transform.localScale = new Vector3(percent,percent,percent);
        }
        else
        {
            float percent = uses / numUses;
            transform.localScale = new Vector3(percent, percent, percent);
        }




    }

    public void Eat()
    {
        numUses--;
        if (numUses <= 0)
            Destroy(this.gameObject);
    }
}
