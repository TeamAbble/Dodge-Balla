
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DodgeBalla
{
    public class Player : NetworkBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        private Vector2 moveDir = Vector2.zero;
        [Header("Health Settings")]
        public float moveSpeed = 100;
        public float acceleration = 100;
        private Rigidbody rb;


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
            rb=GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsOwner)
            {
                return;
            }
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

        public void MoveInput(InputAction.CallbackContext context)
        {
            if (!IsOwner)
            {
                return;
            }
            moveDir = context.ReadValue<Vector2>();
        }
    }
}

