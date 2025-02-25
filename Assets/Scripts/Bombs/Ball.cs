using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEditor.UI;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public bool isLive = false;
    public PlayerController player;
    private Collider col;
    public Rigidbody rb;
    [SerializeField] private int maxNumberOfBounces = 2;
    [SerializeField] private Collider blastTrigger;
    [SerializeField] private ParticleSystem particles;
    private int numberOfBounces;
    public Podium podiumRef;
    public int damage = 1;
    public LayerMask layerMask;
    [SerializeField] private GameObject blast;
    public override void OnNetworkSpawn()
    {
        Debug.Log(OwnerClientId);
        col = GetComponent<SphereCollider>();
        col.enabled = true;
        rb = GetComponent<Rigidbody>();
        numberOfBounces = maxNumberOfBounces;
        blast.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
    [Rpc(SendTo.Everyone)]
    public void OnGrabbed_Rpc(bool isGrabbed)
    {

        Debug.Log("Ball's owner is : " + OwnerClientId);
        Debug.Log("got it");
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
                Invoke("Explode_Rpc", 10f);
            }
            Debug.Log("Ball Bounced, Bounces Remaining: " + numberOfBounces + " Is Ball Live: " + isLive);
            if (player == null) { return; }
            if (collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
            {
                isLive = false;
                Invoke("Explode_Rpc", 5f);
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
    [Rpc(SendTo.Everyone)]
    private void Explode_Rpc()
    {
        blast.SetActive(true);
        particles.Play();
        Invoke("ReturnToPodium", 0.5f);
    }
    public void ExplosiveHit(GameObject other)
    {
        RaycastHit playerHit;
        
        if (Physics.Raycast(origin:transform.position, direction:(other.transform.position - transform.position).normalized, out playerHit,Mathf.Infinity,layerMask:layerMask))
        {
            if (playerHit.collider.gameObject == other)
            {
                playerHit.collider.gameObject.GetComponent<PlayerController>().ChangeHealth(-damage);
                Debug.Log("ouchy");
            }
        }

    }
}
