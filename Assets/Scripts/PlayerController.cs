using Unity.Cinemachine;
using Unity.Netcode;
using UnityEngine;

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
   
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
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
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        controller.Move(move*Time.deltaTime*playerSpeed);

        if (move != Vector3.zero)
        {
            //rotates the player in the movement direction
            gameObject.transform.forward = move;
        }
        if (inputManager.PlayerJumped() && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        playerVelocity.y += gravity*Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


    }
}
