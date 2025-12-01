[System.Serializable]
public class PlayerStatus
{
    public int maxHp;
    public int hp;
    public int maxMp;
    public int mp;
    public int level;
    public int exp;

    public PlayerStatus(int _maxHp = 100, int _maxMp = 50)
    {
        maxHp = _maxHp;
        hp = _maxHp;
        maxMp = _maxMp;
        mp = _maxMp;
        level = 1;
        exp = 0;
    }

    public void Init(int _maxHp, int _maxMp)
    {
        maxHp = _maxHp;
        hp = _maxHp;
        maxMp = _maxMp;
        mp = _maxMp;
        level = 1;
        exp = 0;
    }
}
