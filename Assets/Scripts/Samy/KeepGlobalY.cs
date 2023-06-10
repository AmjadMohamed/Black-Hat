using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepGlobalY : MonoBehaviour
{
    public float yValue = 0;

    void Update()
    {
        gameObject.transform.position = new Vector3()
        {
            x = gameObject.transform.position.x,
            y = yValue,
            z = gameObject.transform.position.z
        };
    }
}
