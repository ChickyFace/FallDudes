using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerLocomotion : MonoBehaviour
{
    AnimatorManager animatorManager;
    PlayerManager playerManager;
    InputManager inputManager;
    Vector3 moveDirection;
    Rigidbody playerRigidbody;
    Transform cameraObject;
    private Vector3 cylinderContactPoint;


    [Header("Falling")]
    private float inAirTimer;
   // public float leapingVelocity;
    public float fallingVelocity;
    private float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;
    public Vector3 checkPoint;

    [Header("Movement Flags")]
    public bool isGrounded;
    public bool isJumping;
    //public bool isOnAir;
    public bool hitCylinderPlatform = false;
    public LayerMask rotatingPlatformLayer;

    [Header("Movement Speeds")]
    public float walkingSpeed ;
    public float runningSpeed ;
    public float rotationSpeed ;

    [Header("Jump Speeds")]
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    [Header("ObstacleHit")]
    [SerializeField] private float knockbackForceMagnitude;
    public bool isStuned;
    private Vector3 ObstacleHitPosition;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();  
        inputManager= GetComponent<InputManager>();
        playerRigidbody= GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
    public void HandleAllMovement()
    {
        HandleRotatingPlatformEffect();
        HandleKnockBack();
        if (isStuned)  //stun ise herşeyi kitliyor.
            return;
        HandleFallingAndLanding();
        if (playerManager.isInteracting) //diger hareketleri kitliyor
            return;
        if (isJumping) //havadayken Movement and Rotation locking.
            return;
        HandleMovement();
        HandleRotation();


       

    }
    private void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (inputManager.moveAmount >= 0.5f)
        {
            moveDirection = moveDirection * runningSpeed;
        }
        else
        {
            moveDirection = moveDirection * walkingSpeed;
        }

        
        Vector3 movementVelocity = moveDirection ;
        movementVelocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = movementVelocity;
        // playerRigidbody.AddForce(movementVelocity, ForceMode.VelocityChange);
        
    }
    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation ,rotationSpeed*Time.deltaTime);
        transform.rotation = playerRotation;

    }
    private void HandleFallingAndLanding ()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

     
        if (!isGrounded && !isJumping)
        {
            if(!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling",true);
                
            }

            inAirTimer = inAirTimer + Time.deltaTime;
           // playerRigidbody.AddForce(transform.forward * leapingVelocity);
            playerRigidbody.AddForce(Vector3.down * fallingVelocity * inAirTimer);
            
        }

       

        if (Physics.SphereCast( rayCastOrigin, 0.2f, Vector3.down, out hit , 2f, groundLayer))
        {
            
            if (!isGrounded && playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);
            }

       


            inAirTimer = 0; 
            isGrounded = true;
            

        }
        else
        {
            isGrounded = false;
        }

    }
    private void HandleRotatingPlatformEffect()
    {

        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

       // int rotatingPlatformLayer = LayerMask.GetMask("RotatingPlatform");
        RaycastHit hit;
        if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, 1f, rotatingPlatformLayer))
        {
            hitCylinderPlatform = true;
            cylinderContactPoint = hit.point; // Store the point of contact

        }
        else { hitCylinderPlatform = false; }

       
        if (hitCylinderPlatform)
        {
            
            float rotationSpeed = hit.collider.GetComponent<Rotator>().rotationSpeed;
            
            float direction = rotationSpeed > 0 ? 1f : -1f;

            float directionFromCenter = (cylinderContactPoint - hit.collider.transform.position).magnitude;
           


            Vector3 forceToAdd = new Vector3((1f * direction)*directionFromCenter* rotationSpeed, 0f, 0f);

         
            playerRigidbody.AddForce(forceToAdd, ForceMode.VelocityChange);
            //Vector3 playerVelocity = playerRigidbody.velocity;
            //playerVelocity.x += 1f * direction; 
            //playerRigidbody.velocity = playerVelocity;

        }
    }
    public void HandleJumping()
    {
        Vector3 playerVelocity = moveDirection;
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            playerVelocity.y = jumpingVelocity;
            playerRigidbody.velocity = playerVelocity;
            
        }
      
    }
    public void LoadCheckPoint()
    {
        transform.position = checkPoint;
    }

    //ObstacleHit
    public void HandleKnockBack()
    {
        if (isStuned)
        {
            playerRigidbody.AddForce(ObstacleHitPosition * knockbackForceMagnitude, ForceMode.Impulse);
            StartCoroutine(DecreaseForce());
        }
    }
    private IEnumerator DecreaseForce()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 originalForce = ObstacleHitPosition * knockbackForceMagnitude;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Vector3 currentForce = Vector3.Lerp(originalForce, Vector3.zero, t);
            playerRigidbody.AddForce(currentForce, ForceMode.Force);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isStuned = false;
        animatorManager.animator.SetBool("isStuned", false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("DynamicObstacle") 
                /*|| collision.gameObject.layer == LayerMask.NameToLayer("PendulumBall")*/)
            {
                animatorManager.animator.SetBool("isStuned", true);
                Debug.Log("hit");
                isStuned = true;
                Vector3 forceDirection = transform.position - contact.point ;
                forceDirection.Normalize();
                forceDirection.y = 0f;
                ObstacleHitPosition = forceDirection;
            }
        }
    }



}
