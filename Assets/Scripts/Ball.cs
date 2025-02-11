using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEditor.UI;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public bool isLive = false;
    public PlayerController player;
    private Collider col;
    private Rigidbody rb;
    [SerializeField] private int maxNumberOfBounces=2;
    private int numberOfBounces;
    public Podium podiumRef;
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = true;
        rb = GetComponent<Rigidbody>();
        numberOfBounces = maxNumberOfBounces;
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
        numberOfBounces = maxNumberOfBounces;
        CancelInvoke();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (isLive)
        {
            numberOfBounces--;

            
            if (numberOfBounces == 0)
            {
                isLive = false;
                Invoke("ReturnToPodium", 10f);
            }
            Debug.Log("Ball Bounced, Bounces Remaining: " + numberOfBounces + " Is Ball Live: "+ isLive);
            if (player = null) { return; }
            if (collision.gameObject.tag == "Player"&&collision.gameObject!=player.gameObject)
            {
                isLive = false;
                Invoke("ReturnToPodium", 5f);
                player.AddScore(10);
            }

        }
       
    }
    private void ReturnToPodium()
    {
        if (podiumRef != null)
        {
            podiumRef.SpawnBall();
            NetworkObject.Despawn(true);
        }
    }
}
