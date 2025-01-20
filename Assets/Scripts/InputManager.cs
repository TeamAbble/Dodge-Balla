using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();   
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Player.Walk.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta()
    {
        return playerControls.Player.Look.ReadValue<Vector2>(); 
    }
    public bool PlayerJumped()
    {
        if (playerControls.Player.Jump.ReadValue<float>() == 1)
        {
            return true;
        }
        else return false;
    }
    
}
