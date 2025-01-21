using System.Runtime.CompilerServices;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isLive = false;
    private GameObject playerRef;
    private Collider col;
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnGrabbed()
    {
        col.enabled = false;
    }
    public void OnThrow()
    {
        col.enabled = true;
        isLive = true;
    }
    
}
