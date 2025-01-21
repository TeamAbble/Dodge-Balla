using System;
using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
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
    [SerializeField] private Transform holdLocation;
    private GameObject heldBall;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        inputManager.playerControls.Player.Grab.performed += OnGrab;
        inputManager.playerControls.Player.Throw.performed += OnThrow;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            cam.enabled = false;   
        }
   
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        LookAround();
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = head.transform.forward*move.z+ head.transform.right*move.x;
        move.y = 0;
        controller.Move(move*Time.deltaTime*playerSpeed);

        playerVelocity.y += gravity*Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
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
        RaycastHit hit;
        if (Physics.Raycast(head.transform.position, head.transform.forward,out hit, grabDistance))
        {
            if (hit.collider.gameObject.tag == "ball"){
                heldBall = hit.collider.gameObject;
                heldBall.GetComponent<Ball>().OnGrabbed();
            }
            
        }
    }
    void OnThrow(InputAction.CallbackContext context)
    {
        if (heldBall != null)
        {
            heldBall.GetComponent<Ball>().OnThrow();
            heldBall.GetComponent<Rigidbody>().AddForce(head.transform.forward * throwForce);
            heldBall = null;
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
}
