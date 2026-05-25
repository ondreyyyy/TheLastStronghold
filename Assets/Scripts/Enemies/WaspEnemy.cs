using UnityEngine;

public class WaspEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        health = 20;      // Низкое здоровье
        damage = 10;      // Низкий урон
        speed = 7f;       // Очень высокая скорость
        agent.speed = speed;
    }

    // Здесь можно реализовать приоритет атаки тыловых сооружений
}
