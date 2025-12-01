using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Player player;
    public PlayerMove(Player _player)
    {
        player = _player;
    }

    public void CheckMove()
    {
        if (Managers.Input.IsMoveInput)
        {
            player.playerController.ChangeState(Define.PlayerState.MOVE);
        }
    }

    public void Move()
    {

        Quaternion targetRotation = Quaternion.LookRotation(Managers.Input.MoveDirection);
        player.playerController.transform.rotation = Quaternion.Slerp(player.playerController.transform.rotation, targetRotation, player.playerController.rotationSpeed * Time.deltaTime);

        player.playerController.transform.position += Managers.Input.MoveDirection * player.moveSpeed * Time.deltaTime;
        
    }

    public void CheckStop()
    {
        if (!Managers.Input.IsMoveInput)
        {
            player.playerController.ChangeState(Define.PlayerState.IDLE);
        }
    }
}
