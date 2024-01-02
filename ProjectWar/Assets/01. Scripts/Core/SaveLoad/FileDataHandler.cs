using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
    public string directoryPath = "";
    public string filename = "";

    public bool isEncrypt;
    public bool isBase64;

    private CryptoModule cryptoModule;

    public FileDataHandler(string directoryPath, string filename, bool isEncrypt, bool isBase64 = false)
    {
        this.directoryPath = directoryPath;
        this.filename = filename;
        this.isEncrypt = isEncrypt;
        this.isBase64 = isBase64;

        cryptoModule = new CryptoModule();
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(directoryPath, filename);

        try
        {
            Directory.CreateDirectory(directoryPath);
            string dataToStore = JsonUtility.ToJson(data, true);

            if (isEncrypt)
                dataToStore = cryptoModule.AESEncrypt256(dataToStore);

            using (FileStream writeStrem = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(writeStrem))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(directoryPath, filename);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream readStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(readStream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (isEncrypt)
                    dataToLoad = cryptoModule.Decrypt(dataToLoad);

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error on trying to load data to file {fullPath} \n");
            }
        }

        return loadedData;
    }

    public void DeleteSaveData()
    {
        string fullPath = Path.Combine(directoryPath, filename);

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
                Debug.Log("delete save file");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error on trying to delete file {fullPath} \n");
            }
        }
    }

    private string codeWord = "ggm";

    private string EncryptAndDecryptData(string data)
    {
        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append((char)(data[i] ^ codeWord[i % codeWord.Length]));
        }

        return sBuilder.ToString();
    }

    private string Base64Process(string data, bool encoding)
    {
        if(encoding)
        {
            byte[] dataByteArr = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(dataByteArr);
        }
        else
        {
            byte[] dataByteArr = Convert.FromBase64String(data);
            return Encoding.UTF8.GetString(dataByteArr);
        }
    }
}
