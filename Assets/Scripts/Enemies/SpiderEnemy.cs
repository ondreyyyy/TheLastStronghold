using UnityEngine;

public class SpiderEnemy : EnemyBase
{
    protected override void Awake()
    {
        base.Awake();
        health = 30;      // Низкое здоровье
        damage = 15;      // Средний урон
        speed = 3.5f;     // Средняя скорость
        agent.speed = speed;
    }

    // Здесь можно реализовать поведение "расползания" по нескольким целям
}
