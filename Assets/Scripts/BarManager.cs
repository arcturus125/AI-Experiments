using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarManager : MonoBehaviour
{
    public Rabbit rabbit;
    public Canvas barMenu;

    public Color hungerColour;
    public Color thirstColour;
    public Color reproductiveColour;
    public Color fearColour;

    public Slider hungerSldr;
    public Slider thirstSldr;
    public Slider reproSldr;
    public Slider fearSldr;

    public Text stateTxt;

    // Start is called before the first frame update
    void Start()
    {

        hungerSldr.value = 0.0f;
        thirstSldr.value = 0.0f;
        reproSldr.value = 0.0f;
        fearSldr.value = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = transform.position + (transform.position - Camera.main.transform.position);
        barMenu.transform.LookAt(target);

        stateTxt.text = rabbit.state;

        foreach(Gene g in rabbit.genes)
        {
            switch(g.name)
            {
                case "Hunger":
                    hungerSldr.value = g.value / g.cap;
                    break;
                case "Thirst":
                    thirstSldr.value = g.value / g.cap;
                    break;
                case "reproductiveUrge":
                    reproSldr.value = g.value / g.cap;
                    break;
                default:
                    break;
            }
        }
    }


}
