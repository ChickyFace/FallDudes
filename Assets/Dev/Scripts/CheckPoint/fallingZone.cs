using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingZone : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerLocomotion>().LoadCheckPoint();
        }
    }
}
