using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] private string filename;
    [SerializeField] private bool isEncrypt;
    [SerializeField] private bool isBase64;
    private GameData gameData;
    private List<ISaveManager> saveManagerList;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename, isEncrypt, isBase64);
        saveManagerList = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();

        if(gameData == null )
        {
            Debug.Log("No save data found");
            NewGame();
        }

        foreach(ISaveManager saveManager in saveManagerList)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagerList)
        {
            saveManager.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    public List<ISaveManager> FindAllSaveManagers()
    {
        return FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>().ToList();
    }

    [ContextMenu("Delete save file")]
    public void DeleteSaveData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, filename, isEncrypt);
        fileDataHandler.DeleteSaveData();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
