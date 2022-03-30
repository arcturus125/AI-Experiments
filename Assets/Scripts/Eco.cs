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
        if (pos.x > 100 || pos.x < -100) return false;
        if (pos.z > 100 || pos.z < -100) return false;

        pos.y = 50;

        RaycastHit hitInfo;

        if (Physics.Raycast(pos, Vector3.down, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag == "Land") return true;
            if (hitInfo.collider.gameObject.tag == "Water") return false;
        }
        return false;
    }
    public static bool isValidGround(Vector3 pos, out Vector3 hitPos)
    {
        pos.y = 50;

        RaycastHit hitInfo;

        if (Physics.Raycast(pos, Vector3.down, out hitInfo))
        {
            hitPos = hitInfo.point;
            if (hitInfo.collider.gameObject.tag == "Land") return true;
            if (hitInfo.collider.gameObject.tag == "Water") return false;
        }
        hitPos = Vector3.zero;
        return false;
    }
}
