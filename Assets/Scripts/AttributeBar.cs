using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttributeBar", menuName = "Genes/AttributeBar", order = 1)]
public class AttributeBar : Attribute
{
    public float rate = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (value < cap)
            value += rate * Time.deltaTime;

    }

    public override Attribute Duplicate()
    {
        AttributeBar a = new AttributeBar();
        a.name = name;
        a.cap = cap;
        a.startValue = startValue;
        a.value = 0;
        a.objective = objective;
        a.rate = rate;

        return a;
    }
}
