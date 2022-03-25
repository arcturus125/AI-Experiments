using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eco : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static bool isValidGround(Vector3 pos)
    {
        pos.y = 50;

        RaycastHit hitInfo;

        if(Physics.Raycast(pos, Vector3.down, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag == "Land") return true;
            if (hitInfo.collider.gameObject.tag == "Water") return false;
        }
        return false;
    }
}
