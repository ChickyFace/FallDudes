using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacleHit : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    private CharacterController characterController;

    private Vector3 moveDirection;

    //private bool isStumbling;

    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();    
        characterController = GetComponent<CharacterController>();
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("DynamicObstacle"))
        {
            animator.SetBool("IsStumbling", true);
            //isStumbling = true;

            Vector3 forceDirection = transform.position - hit.gameObject.transform.position;

            forceDirection.Normalize();
            forceDirection.y = 0f;

            StartCoroutine(ForceEffect(forceDirection * forceMagnitude, 0.5f)); // Adjust duration as needed
            //selfRigidbody.AddForce(forceDirection * forceMagnitude/** Time.deltaTime*/, ForceMode.Impulse);
        }
        if (hit.gameObject.layer == LayerMask.NameToLayer("RotatingPlatform"))
        {
            //

        }
    }
    private IEnumerator ForceEffect(Vector3 force, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            moveDirection = force * (1f - elapsed / duration);        // Gradually reduce the force effect
            characterController.Move(moveDirection * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("IsStumbling", false);
        //isStumbling = false;
    }

    


}
