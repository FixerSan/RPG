using UnityEngine;

public class Player
{
    public PlayerController playerController;
    public PlayerStatus status;
    public PlayerMove move;
    public PlayerAttack attack;
    public float moveSpeed;
    // 기타 데이터: 무기/방어구/스킬 등 확장 가능

    public Player(PlayerController _playerController,int maxHp = 100, int maxMp = 50, float moveSpeed = 5f)
    {
        playerController = _playerController;
        status = new PlayerStatus(maxHp, maxMp);
        this.moveSpeed = moveSpeed;
        move = new PlayerMove(this);
    }

    // 초기화
    public void Init(int maxHp, int maxMp, float moveSpeed)
    {
        status.Init(maxHp, maxMp);
        this.moveSpeed = moveSpeed;
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
