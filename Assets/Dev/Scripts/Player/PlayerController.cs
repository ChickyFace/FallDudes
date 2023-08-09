using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;



public class PlayerController : MonoBehaviour
{

    //HandleMovement
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumoButtonGracePeriod;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;

    //[SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Transform cameraTransform;

    //IsGround
   // public LayerMask groundLayer; //gerek kalmadı Character Controller otomatik görüyor.
   // [SerializeField] private float groundCheckDistance;


    //animasyon için
    private bool isWalking; 
    private bool jumpPressed;
    private bool isLanding ;
    private bool isMidAir;
    private float inputMagnitude;
    public bool IsWalking()
    {
        return isWalking;
    }
    public bool JumpPressed()
    {
        return jumpPressed;
    }
    public bool IsMidAir()
    {
        return isMidAir;
    }
    public bool IsLanding()
    {
        return isLanding;
    }
    public float InputMagnitude()  //BlendTree inputMagnitude'u PlayerAnimator'e yolluyoruz
    {
        return inputMagnitude;
    }


    //ObstacleHit
    //public bool CanMove { get; set; }

    
   
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset =characterController.stepOffset;
        //RatingManager.instance.SetOpponentData(transform.position.z, "Player");
    }

    private void Update()
    {

      
            HandleMovement();

        

    }
   
    private void HandleMovement()
    {

        //InputSystem
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        inputMagnitude = Mathf.Clamp01(moveDir.magnitude)/2;


        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 2;
        }
        float speed = maxSpeed * inputMagnitude;

        //Debug.Log("Player speed is " + speed);
        moveDir = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
        moveDir.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;

        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;  
        }
        if (Time.time - lastGroundTime <= jumoButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            isLanding = true;
            jumpPressed = false;   
            isMidAir = false;
            

            if (Time.time - jumpButtonPressedTime <= jumoButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpPressed = true;
                jumpButtonPressedTime = null;
                lastGroundTime = null;

            }
        }
        else
        {
            characterController.stepOffset = 0f;
            isLanding = false;

            if((jumpPressed && ySpeed < 0) || ySpeed < -2)
            {
                isMidAir=true;
            }


        }
        

        Vector3 velocity = moveDir * speed;
        velocity.y = ySpeed;
       
       
         characterController.Move(velocity * Time.deltaTime);


        if(moveDir !=  Vector3.zero)
        {
            isWalking = true;
            Quaternion toRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            isWalking = false;
        }

    }

 

    /* private bool IsGrounded()  // GroundCheck method
    {

         Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius);
         Vector3 point2 = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius);


         RaycastHit hit;
         bool isHit = Physics.CapsuleCast(point1, point2, capsuleCollider.radius/3, Vector3.down,out hit, groundCheckDistance , groundLayer);

        //if (hit.transform.tag == "Slide")
        //{
        //    slide = true;
        //}
        //else
        //{
        //    slide = false;
        //}

        return isHit;
     } */
    /* private void OnDrawGizmosSelected()  //GroundCheckGizmos
    {
         // Color to be used for the capsule-shaped gizmos
         Color gizmosColor = IsGrounded() ? Color.green : Color.red;

         // Draw capsule-shaped gizmos using the specified color
         Gizmos.color = gizmosColor;

         Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius);
         Vector3 point2 = transform.position + capsuleCollider.center - (Vector3.up * (capsuleCollider.height * 0.5f - capsuleCollider.radius)) ;

         Gizmos.DrawWireSphere(point1, capsuleCollider.radius);
         Gizmos.DrawWireSphere(point2, capsuleCollider.radius);
         Gizmos.DrawLine(point1 + Vector3.up * capsuleCollider.radius, point2 - Vector3.up * capsuleCollider.radius);
     } */



}