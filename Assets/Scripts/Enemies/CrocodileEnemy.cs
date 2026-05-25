using UnityEngine;

public class CrocodileEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        health = 80;      // Среднее здоровье
        damage = 60;      // Очень высокий урон
        speed = 3f;       // Средняя скорость
        agent.speed = speed;
    }

    // Здесь можно реализовать приоритет атаки самых слабых или важных сооружений
}
