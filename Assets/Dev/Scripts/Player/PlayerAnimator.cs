using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;


    private const string IS_WALKING = "IsWalking";
    private const string JUMP_PRESSED = "JumpPressed";
    private const string IS_LANDING = "IsLanding";
    private const string IS_MID_AIR = "IsMidAir";
    private const string INPUT_MAGNITUDE = "Input Magnitude";



    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, playerController.IsWalking());
        animator.SetBool(JUMP_PRESSED, playerController.JumpPressed());
        animator.SetBool(IS_LANDING, playerController.IsLanding());
        animator.SetBool(IS_MID_AIR, playerController.IsMidAir());

        animator.SetFloat(INPUT_MAGNITUDE, playerController.InputMagnitude() , 0.05f, Time.deltaTime);  //inputMagnitude'ler arası damp verildi, animasyon akıcı geçiyor
        //Debug.Log("input magnitude = " + playerController.InputMagnitude());

    }

}
