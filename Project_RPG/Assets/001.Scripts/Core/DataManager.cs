using JetBrains.Annotations;
using System;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class DataManager 
{
    public PlayerData playerData = new PlayerData();
    public string savePath = Application.persistentDataPath;

    public void GetPlayerData(Action<PlayerData> _callback)
    {
        //일단 new 로 테스트  
        playerData = new PlayerData();
        _callback?.Invoke(playerData);
    }

    public void LoadPlayerData()
    {
        //나중에 로컬 DB or 서버에서 불러오
        playerData = JsonUtility.FromJson<PlayerData>(Managers.Resource.Load<TextAsset>("PlayerData").text);
    }

    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(savePath, json);
        Debug.Log("데이터 저장 성공!");
    }
}


[System.Serializable]
public class Data
{

}

[System.Serializable]
public class PlayerData : Data
{
    public string playerName;
    public int level;
    public float maxHp;
    public float maxMp;
    public float moveSpeed;
}
