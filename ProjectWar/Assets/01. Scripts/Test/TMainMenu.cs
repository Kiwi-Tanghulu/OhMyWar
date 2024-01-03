using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TMainMenu : MonoBehaviour
{
	[SerializeField] TMP_InputField joinCodeField;
    
    private void Start()
    {
        HostManager.Instance.OnRoomCreatedEvent += HandleRoomCreated;
    }

    private void OnDestroy()
    {
        HostManager.Instance.OnRoomCreatedEvent -= HandleRoomCreated;
    }

    public async void CreateRoom()
    {
        await HostManager.Instance.CreateRoomAsync();
    }

    public async void JoinRoom()
    {
        await ClientManager.Instance.JoinRoomAsync(joinCodeField.text);
    }

    private void HandleRoomCreated()
    {
        // SceneLoader.Instance.LoadSceneAsync("LobbyScene");
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
