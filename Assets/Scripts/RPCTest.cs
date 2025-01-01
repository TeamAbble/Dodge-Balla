using Unity.Netcode;
using UnityEngine;

public class RPCTest : NetworkBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner)
        {
            ServerOnlyRpc(0, NetworkObjectId);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    void ClientHostRpc(int value,ulong sourceNetworkObjectId)
    {
        Debug.Log($"client recieved the RPC {value} on NetworkObject{sourceNetworkObjectId}");
        ServerOnlyRpc(value + 1, sourceNetworkObjectId);
    }


    [Rpc(SendTo.Server)]
    void ServerOnlyRpc(int value,ulong sourceNetworkObjectId)
    {
        Debug.Log($"Server Recieved RPC #{value} on NetworkObject {sourceNetworkObjectId}");

    }

}
