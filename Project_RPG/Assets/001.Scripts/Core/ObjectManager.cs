using JetBrains.Annotations;
using UnityEngine;

public class ObjectManager 
{
    public PlayerController PlayerController
    {
        get
        {
            if (playerController == null)
            {
                playerController = GameObject.Find($"@{PlayerController}").GetOrAddComponent<PlayerController>();
                if (playerController == null)
                    SpawnPlayerController();
            }
            return playerController;
        }
    }

    private PlayerController playerController;
    public ObjectManager()
    {

    }

    public PlayerController SpawnPlayerController()
    {
        playerController = Managers.Resource.Instantiate("PlayerController").GetOrAddComponent<PlayerController>();
        playerController.Init();
        return playerController;
    }

    public void SetPlayerController(PlayerController _playerController)
    {
        if (playerController == _playerController) return;
        if (playerController != null)
        {
            Managers.Resource.Destroy(_playerController.gameObject);
            return;
        }

        playerController = _playerController;
        playerController.Init();
    }
}
