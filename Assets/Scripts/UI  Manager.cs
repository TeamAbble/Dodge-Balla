using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject hostJoinMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject hostMenu;
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
        currentMenu.SetActive(false);
        currentMenu=hostMenu;
        currentMenu.SetActive(true);
    }
    public void startClient()
    {
        networkManager.StartClient();
    }
    public void StartBallaDesert()
    {
        networkManager.SceneManager.LoadScene("Balla Desert", LoadSceneMode.Single);
    }
}
