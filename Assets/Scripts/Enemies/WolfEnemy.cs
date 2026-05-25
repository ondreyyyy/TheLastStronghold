using UnityEngine;

public class WolfEnemy : EnemyBase
{
    protected override void Awake() 
    { 
        base.Awake();
        health = 40;
        damage = 10;
        speed = 5.5f;
        agent.speed = speed;
    }
}