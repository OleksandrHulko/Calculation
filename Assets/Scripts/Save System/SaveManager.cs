using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region Public Fields
    public static List<LevelInfo> levelInfos = new List<LevelInfo>();
    #endregion

    #region Private Fields
    private string _filePath = string.Empty;
    #endregion
    
    
    #region Unity Methods
    private void Awake()
    {
        _filePath = $"{Application.persistentDataPath}/SaveData.dat";
        LoadData();
    }

    private void OnDestroy()
    {
        SaveData();
    }

    private void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void SaveData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(_filePath);
        SaveData saveData = new SaveData()
        {
            levelInfos = levelInfos
        };
        binaryFormatter.Serialize(fileStream, saveData);
        fileStream.Close();
        Debug.Log("Save game data");
    }

    public void LoadData()
    {
        if (File.Exists(_filePath))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(_filePath, FileMode.Open);
            SaveData saveData = (SaveData) binaryFormatter.Deserialize(fileStream);
            levelInfos = saveData.levelInfos;
            fileStream.Close();
            Debug.Log("Load game data");
        }
        else
            Debug.Log("File not found");
    }
    #endregion
}
