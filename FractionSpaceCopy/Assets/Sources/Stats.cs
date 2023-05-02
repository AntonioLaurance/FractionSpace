
public class Stats
{
    public float health;
    public float maxHealth;

    public int level;
    public float attack;
    public float deffense;
    public float spirit;
    public float speed;

    public Stats(float _maxhealth, float _attack, float _deffense, float _speed)
    {
        this.maxHealth = _maxhealth;
        this.health = _maxhealth;

        this.attack = _attack;
        this.deffense = _deffense;
        
        this.speed = _speed;
    }

    public Stats Clone()
    {
        return new Stats(this.maxHealth, this.attack, this.deffense,this.speed);
    }
}