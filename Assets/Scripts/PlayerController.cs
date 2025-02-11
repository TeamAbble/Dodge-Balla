using System;
using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    [SerializeField]private bool isGrounded;
    [Header("Movement Settings")]
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravity=-9.81f;
    private InputManager inputManager;

    [Header("Camera Settings")]
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private GameObject head;
    [SerializeField] private float clampAngle = 80f;
    [SerializeField] private float lookSpeed = 300f;
    private Vector3 startRotation;
    [Header("Combat Settings")]
    [SerializeField] private float grabDistance = 10f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float throwArc = 1f;
    [SerializeField] private Transform holdLocation;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityTimeInSeconds = 2;

    [SerializeField]private CapsuleCollider col;
    [SerializeField]private float groundCheckDistance = 0.2f;
    private Ball heldBall;

    private int score = 0; 
    void Start()
    {
        
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            cam.enabled = false;   
        }
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        inputManager.playerControls.Player.Grab.performed += OnGrab;
        inputManager.playerControls.Player.Throw.performed += OnThrow;
        inputManager.playerControls.Player.Jump.performed += OnJump;
        

    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        LookAround();
        GroundCheck();
        if (!isGrounded)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        else if (playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        if(playerVelocity.y != 0)
        {
            controller.Move(playerVelocity * Time.deltaTime);
        }
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = head.transform.forward*move.z+ head.transform.right*move.x;
        move.y = 0;
        controller.Move(move*Time.deltaTime*playerSpeed);

        
        if (heldBall != null)
        {
            heldBall.transform.position = holdLocation.position;
            
        }


    }

    void LookAround()
    {
        if (startRotation == null) startRotation = head.transform.localRotation.eulerAngles;
        Vector2 mouseInput=inputManager.GetMouseDelta();
        startRotation.x += mouseInput.x * Time.deltaTime*lookSpeed;
        startRotation.y += mouseInput.y * Time.deltaTime*lookSpeed;
        startRotation.y = Mathf.Clamp(startRotation.y, -clampAngle, clampAngle);
        head.transform.rotation = Quaternion.Euler(-startRotation.y,startRotation.x,0f );

    }
    void OnGrab(InputAction.CallbackContext context)
    {
        if (!IsOwner)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, head.transform.forward,out hit, grabDistance))
        {
            
               if (hit.rigidbody!=null&&hit.rigidbody.TryGetComponent(out Ball ball))
                {
                    ball.OnGrabbed_Rpc(true);
                    PickUpBall_Rpc(ball);
                    heldBall = ball;
                    ball.player = this;
                Debug.Log("grabbed");
                }
            
            
        }
    }


    void OnJump(InputAction.CallbackContext context)
    {

        if (!IsOwner)
        {
            return;
        }
        if (isGrounded)
        {
            playerVelocity.y = 10;
        }
        
    }
    void OnThrow(InputAction.CallbackContext context)
    {
        if (heldBall != null)
        {
            heldBall.transform.position=new Vector3(heldBall.transform.position.x,head.transform.position.y,heldBall.transform.position.z);
            heldBall.GetComponent<Ball>().OnGrabbed_Rpc(false);
            heldBall.GetComponent<Rigidbody>().AddForce(head.transform.forward * throwForce);
            heldBall.GetComponent<Rigidbody>().AddForce(new Vector3(0,throwArc,0), ForceMode.Impulse);

            heldBall = null;
        }
    }
    [Rpc(SendTo.Server)]
    void PickUpBall_Rpc( NetworkBehaviourReference ball)
    {
        if (ball.TryGet(out Ball ballComp)&&ballComp.NetworkObject.OwnerClientId != OwnerClientId) { 
            ballComp.NetworkObject.ChangeOwnership(OwnerClientId);}
    }
    [Rpc(SendTo.Server)]
    void ThrowBall_Rpc(NetworkBehaviourReference ball)
    {
        if (ball.TryGet(out Ball ballComp)&&ballComp.NetworkObject.OwnerClientId == OwnerClientId)
        {
            ballComp.NetworkObject.RemoveOwnership();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collided");
        if (other.gameObject.tag == "ball")
        {
            Ball ball = other.gameObject.GetComponent<Ball>();
            Debug.Log("Ball");
            if (ball.isLive)
            {
                Debug.Log("Live Ball");
            }
        }
    }
    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), Vector3.down, out hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded= false;  
        }
       
    }
    public void AddScore(int scoreAdded) {
        if (!IsOwner)
        {
            return;
        }
        score += scoreAdded;
        Debug.Log("Score added, new score is: " + score);
    }
    
}










