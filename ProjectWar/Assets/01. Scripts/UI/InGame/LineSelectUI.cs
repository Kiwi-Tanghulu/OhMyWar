using Unity.Netcode;
using UnityEngine;

public class LineSelectUI : PanelUI
{
	[SerializeField] InputReader inputReader;

    private Player player = null;
    private Player Player {
        get {
            if(player == null)
                player = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<Player>();
            return player;
        }
    }

    private int currentLine = 1;

    private void OnEnable()
    {
        inputReader.OnArrowKeyPressed += OnArrowKeyPressedHandle;
    }

    private void OnDisable()
    {
        inputReader.OnArrowKeyPressed -= OnArrowKeyPressedHandle;
    }

    public void ChangeLine(float value)
    {
        currentLine += (int)Mathf.Sign(value);
        currentLine %= 3;
        if(currentLine < 0)
            currentLine = 2;

        //IngameManager.Instance.ToggleCurrentSpawner(Player, currentLine);
    }

    private void OnArrowKeyPressedHandle(float value)
    {
        if(Player.IsHost == false)
            value *= -1;

        ChangeLine(value);
    }
}
