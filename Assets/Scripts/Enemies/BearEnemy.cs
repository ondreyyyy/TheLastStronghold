using UnityEngine;

public class BearEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        health = 200;     // Очень высокое здоровье
        damage = 40;      // Очень высокий урон
        speed = 2f;       // Низкая скорость
        agent.speed = speed;
    }

    // Здесь можно реализовать приоритет атаки самых прочных сооружений
}
