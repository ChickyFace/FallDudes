using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishing : MonoBehaviour
{
    public GameManager gameManager; // Reference to the GameManager

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameManager.OnPlayerFinished();
        }
        else
        {
            gameManager.OnOpponentFinished();
        }
    }
}
