using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // 声明并获取Rigidbody2d
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Awake()
    {
        // 获取Rigidbody2d
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    // 创建投掷物的launch函数
    public void Launch(Vector2 direction, float speed)
    {
        // 设置投掷物的速度
        this.rigidBody.velocity = direction * speed;
    }

    //投掷物碰撞函数
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Debug.Log("Projectile Collision with " + other.gameObject);
        EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null) {
            enemy.takeDamage(-1);
        }
        Destroy(gameObject);
    }
}
