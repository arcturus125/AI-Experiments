using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tracked float", menuName = "Graph/Track", order = 1)]
public class Track : ScriptableObject
{
    private float _newValue = 0;

    public float NewValue
    {
        get { return _newValue; }
        set { _newValue = value; }
    }
}
