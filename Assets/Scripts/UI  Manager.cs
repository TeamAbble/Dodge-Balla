using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject hostJoinMenu;
    [SerializeField] private GameObject settingsMenu;
    private GameObject currentMenu;

    [Header("Network Configs")]
    [SerializeField] private NetworkManager networkManager;
    private void Start()
    {
        currentMenu = startMenu;
    }

    public void OpenHostJoinMenu()
    {
        currentMenu.SetActive(false);
        currentMenu=hostJoinMenu;
        currentMenu.SetActive(true);
        
    }
    public void OpenSettingsMenu()
    {
        currentMenu.SetActive(false);
        currentMenu = settingsMenu;
        currentMenu.SetActive(true);
    }
    public void OpenStartMenu()
    {
        currentMenu.SetActive(false);
        currentMenu= startMenu;
        currentMenu.SetActive(true);
    }
    public void StartHost()
    {
        networkManager.StartHost();
    }
    public void startClient()
    {
        networkManager.StartClient();
    }
}
