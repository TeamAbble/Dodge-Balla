using Unity.Netcode;
using UnityEngine;

public class NetworkTransformScript : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            float theta = Time.frameCount / 10.0f;
            transform.position = new Vector3((float)Mathf.Cos(theta),0.0f, (float)Mathf.Sin(theta));
        }
    }
}
