
using Unity.Netcode;
using UnityEngine;

namespace DodgeBalla
{
    public class Player : NetworkBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();


        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
            Position.OnValueChanged += PositionUpdated;
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           // transform.position = Position.Value;
        }
        public void Move()
        {
            submitPositionRequestRpc();
        }
        [Rpc(SendTo.Server)]
        void submitPositionRequestRpc(RpcParams rpcParams = default)
        {
            var randomPosition = getRandomPositionOnPlane();
            transform.position += randomPosition;
            Position.Value = randomPosition;
        }


        Vector3 getRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }
        void PositionUpdated(Vector3 prev, Vector3 current)
        {
            transform.position = current;
        }
    }
}

