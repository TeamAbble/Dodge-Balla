using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class NetworkClientTransform : NetworkTransform
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
