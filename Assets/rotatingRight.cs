using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatingRight : MonoBehaviour {

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 2000) * Time.deltaTime);
    }
}
