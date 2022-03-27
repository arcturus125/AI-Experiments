using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gene
{
    public string name;
    public float value;
    public float modfier;

    public enum Objective { HigherIsBetter, LowerIsBetter };
    public Objective objective;

    public virtual void Update()
    {

    }

}
