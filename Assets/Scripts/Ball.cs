using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public bool isLive = false;
    private GameObject playerRef;
    private Collider col;
    private Rigidbody rb;
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = true;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Rpc(SendTo.Everyone)]
    public void OnGrabbed_Rpc(bool isGrabbed)
    {
        isLive = !isGrabbed;
        col.enabled = !isGrabbed;
        rb.isKinematic = isGrabbed;
    }
    
    
}
