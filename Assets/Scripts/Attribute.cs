using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attribute", menuName = "Genes/Attribute", order = 1)]
public class Attribute : ScriptableObject
{
    public string name;
    public float cap = 100;
    public float startValue;
    [HideInInspector]
    public float value = 0;

    public enum Objective { HigherIsBetter, LowerIsBetter};
    public Objective objective;

    public virtual Attribute Duplicate()
    {
        Attribute a = new Attribute();
        a.name = name;
        a.cap = cap;
        a.startValue = startValue;
        a.value = 0;
        a.objective = objective;

        return a;
    }

}
