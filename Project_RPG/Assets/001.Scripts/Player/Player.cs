using UnityEngine;

public class Player
{
    public PlayerController playerController => Managers.Object.PlayerController;
    public PlayerData data;
    public PlayerStatus status;
    public PlayerMove move;
    public PlayerAttack attack;

    // 기타 데이터: 무기/방어구/스킬 등 확장 가능

    public Player(PlayerData _data)
    {
        data = _data;
        status = new PlayerStatus(data.maxHp, data.maxMp, data.moveSpeed);
        move = new PlayerMove(this);
    }


    public void AddExp(int gainedExp)
    {
        status.exp += gainedExp;
        // 레벨업 등 추가 구현 가능
    }

    public void TakeDamage(int damage)
    {
        status.hp -= damage;
        if (status.hp < 0) status.hp = 0;
    }

    public void Heal(int amount)
    {
        status.hp += amount;
        if (status.hp > status.maxHp) status.hp = status.maxHp;
    }
    
}
