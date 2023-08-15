using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheckPoint : MonoBehaviour
{
    public Transform checkPoint;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerLocomotion>().checkPoint = checkPoint.position;
        }
    }
}
