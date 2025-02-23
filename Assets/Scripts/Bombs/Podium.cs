using UnityEngine;
using Unity.Netcode;
public class Podium : NetworkBehaviour
{
    [SerializeField] private NetworkObject ballRef;
    [SerializeField] private Transform spawnPoint;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer)
        {
            return;
        }
        Invoke("SpawnBall",5f);
    }
    public void SpawnBall()
    {
      NetworkObject spawnedBall =  NetworkManager.SpawnManager.InstantiateAndSpawn(ballRef,position:spawnPoint.position);
      spawnedBall.GetComponent<Ball>().podiumRef=this;
    }
}
