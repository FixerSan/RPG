[System.Serializable]
public class PlayerStatus
{
    public float maxHp;
    public float hp;
    public float maxMp;
    public float mp;
    public int level;
    public int exp;
    public float moveSpeed;

    public PlayerStatus(float _maxHp, float _maxMp, float _moveSpeed)
    {
        maxHp = _maxHp;
        hp = _maxHp;
        maxMp = _maxMp;
        mp = _maxMp;
        level = 1;
        exp = 0;
        moveSpeed = _moveSpeed;
    }
}
