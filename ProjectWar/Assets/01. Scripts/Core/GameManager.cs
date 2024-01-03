using UnityEngine;
using System.Threading.Tasks;
using Unity.Netcode;
using System;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;
    public static GameManager Instance {
        get {
            if(instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneLoader.Instance = new SceneLoader();
    }

    private async void Start()
    {
        bool authenticated = await SetNetwork();
        NetworkManager.Singleton.ConnectionApprovalCallback += HandleConnectionApproval;

        if(authenticated)
            SceneLoader.Instance.LoadSceneAsync("MenuScene");
    }

    private async Task<bool> SetNetwork()
    {
        ClientManager.Instance = new ClientManager();
        bool authenticated = await ClientManager.Instance.InitAsync();

        HostManager.Instance = new HostManager();

        return authenticated;
    }

    private void HandleConnectionApproval(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = false;
    }
}