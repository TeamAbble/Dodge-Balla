using Unity.Netcode; 
using UnityEngine;
using UnityEngine.UIElements;


namespace DodgeBalla
{
    public class DodgeBallaManager : MonoBehaviour
    {
        private NetworkManager m_networkManager;
        void Awake()
        {
            m_networkManager = GetComponent<NetworkManager>();
        }
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (m_networkManager.IsClient && !m_networkManager.IsServer)
            {
                StartButtons();
            }
        }

        void StartButtons()
        {
            if (GUILayout.Button("Host")) m_networkManager.StartHost();
            if (GUILayout.Button("Client")) m_networkManager.StartClient();
            if (GUILayout.Button("Server")) m_networkManager.StartServer();
        }
        void StatusLabels()
        {
            var mode = m_networkManager.IsHost ? "Host" : m_networkManager.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + m_networkManager.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: "+mode);
        }

        void SubmitNewPosition()
        {
            if (GUILayout.Button(m_networkManager.IsServer ? "move" : "request position change"))
            {
                if (m_networkManager.IsServer && !m_networkManager.IsClient)
                {
                    foreach(ulong uid in m_networkManager.ConnectedClientsIds) m_networkManager.SpawnManager.GetPlayerNetworkObject(uid).GetComponent<Player>().Move();
                }
                else
                {
                    var playerObject = m_networkManager.SpawnManager.GetLocalPlayerObject();
                    var player = playerObject.GetComponent<Player>();
                    player.Move();
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

