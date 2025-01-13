using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    [Header("Movement Configs")]
    [SerializeField] private float playerSpeed = 2.0f,jumpHeight = 1.0f,gravity=-9.81f;
    private InputManager inputManager;
   
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
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
