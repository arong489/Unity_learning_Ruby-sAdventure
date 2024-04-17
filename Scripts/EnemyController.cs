using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float returnTimer;
    private float damageTimer;
    private Rigidbody2D rigidBody;
    private Vector2 move;
    private Animator animator;
    private int health_point;
    // private int direction;

    [Header("角色数值")]
    [Tooltip("最大生命值")]
    public int max_health_point = 5;
    [Tooltip("移速")]
    public float speed = 3.0f;

    [Header("设定")]
    [Tooltip("是否垂直移动")]
    public bool verticalMode = false;
    [Tooltip("换向时间")]
    public float changeTime = 3f;
    [Tooltip("伤害间隔")]
    public float damageGap = 3f;

    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.health_point = this.max_health_point;
        this.returnTimer = this.changeTime;
        // this.direction = 1;
        this.move = this.verticalMode ? new Vector2(0, this.speed) : new Vector2(this.speed, 0);

        this.animator.SetFloat("MoveX", this.move.x);
        this.animator.SetFloat("MoveY", this.move.y);

    }

    private void Update()
    {
        if (this.health_point == 0)
        {
            return;
        }

        returnTimer -= Time.deltaTime;

        if (returnTimer < 0)
        {
            this.move = -this.move;
            returnTimer = changeTime;
            this.animator.SetFloat("MoveX", this.move.x);
            this.animator.SetFloat("MoveY", this.move.y);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (this.health_point == 0)
        {
            return;
        }

        this.rigidBody.position += this.move * Time.fixedDeltaTime;
    }

    public void takeDamage(int damage)
    {
        if (this.health_point != 0)
        {
            this.health_point += damage;
            if (this.health_point < 0) this.health_point = 0;
            if (this.health_point > this.max_health_point) this.health_point = this.max_health_point;
            // Debug.Log("robot take damage, now it is " + this.health_point);
            if (this.health_point == 0)
            {
                ParticleSystem smoke = GetComponentInChildren<ParticleSystem>();
                smoke.Stop();//停止冒烟
                Destroy(smoke, 3f);// 销毁实例化的烟雾

                Destroy(GetComponent<CapsuleCollider2D>());//立即删除碰撞体
                Destroy(GetComponent<Rigidbody2D>());//立即删除物理引擎

                this.animator.SetTrigger("Fixed");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<RubyController>() != null)
        {

            this.damageTimer = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        RubyController ruby = other.gameObject.GetComponent<RubyController>();
        if (ruby != null)
        {
            this.damageTimer -= Time.deltaTime;
            if (this.damageTimer <= 0f)
            {
                ruby.ChangeHealth(-1);
                this.damageTimer = this.damageGap;
            }
        }
    }
}
