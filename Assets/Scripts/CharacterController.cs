using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterController controller;
    public Transform cam;
    private Animator anim;

    public Transform LookAtTransform;
    public float speed = 4;
    public float jumpHeight = 1;
    public float gravity = -9.81f;

    public bool isGrounded;
    public Transform GroundSensor;
    public float sensorRadius;
    private Vector3 playerVelocity;

    //personaje
    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    public GameObject[] cameras;

    private void Start() 
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        anim.enabled = true;
    }

    void Update()
    {
        //Movement();
        Jump();
        MovementTPS();
    }

    void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            //rotacion
            transform.rotation = Quaternion.Euler(0, angle, 0);
            
            //direccion hacia donde mira PERSONAJE
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        }
    } 

    void MovementTPS()
    {
        float z = Input.GetAxisRaw("Vertical");
        anim.SetFloat("VelZ", z);
        float x = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("VelX", x);

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    void Jump()
    {
        anim.SetBool("Jump", !isGrounded);

        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        if(isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity); 
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
