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
            Managers.Object.PlayerController.ChangeState(Define.PlayerState.MOVE);
        }
    }

    public void Move()
    {

        Quaternion targetRotation = Quaternion.LookRotation(Managers.Input.MoveDirection);
        Managers.Object.PlayerController.transform.rotation = Quaternion.Slerp(Managers.Object.PlayerController.transform.rotation, targetRotation, Managers.Object.PlayerController.rotationSpeed * Time.deltaTime);

        Managers.Object.PlayerController.transform.position += Managers.Input.MoveDirection * player.status.moveSpeed * Time.deltaTime;
        
    }

    public void CheckStop()
    {
        if (!Managers.Input.IsMoveInput)
        {
            Managers.Object.PlayerController.ChangeState(Define.PlayerState.IDLE);
        }
    }
}
