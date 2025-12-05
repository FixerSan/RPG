using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Player player;

    public void Awake()
    {
        SetPlayer();
    }

    public void SetPlayer()
    {
        Managers.Data.GetPlayerData((_playerData) => 
        {
            player = new Player(_playerData);
        });
    }
}
