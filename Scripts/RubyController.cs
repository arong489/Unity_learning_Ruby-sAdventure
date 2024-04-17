using System.Text.RegularExpressions;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private GameObject projectilePrefab;
    private float horizontal, vertical;
    private float launchTimer;
    private Vector2 lookDirection;

    private UIHealthBar health_bar;

    [Header("角色数值")]
    [Tooltip("最大生命值")]
    public int max_health_point = 10;
    [HideInInspector]
    public int health_point { private set; get; }
    [Tooltip("移速")]
    public float speed = 3.0f;
    [Tooltip("子弹发射速度")]
    public float launchSpeed = 5.0f;
    [Tooltip("子弹发射间隔")]
    public float launchGap = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.health_bar = GameObject.Find("Canvas/Health/Health Bar").GetComponent<UIHealthBar>();

        this.rigidBody = GetComponent<Rigidbody2D>();
        this.health_point = this.max_health_point;
        this.animator = GetComponent<Animator>();
        // 绝对路径加载prefab
        this.projectilePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/CogBullet.prefab");
        // Debug.Log(this.projectilePrefab);
        this.launchTimer = 0f;

        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        this.horizontal = Input.GetAxis("Horizontal");
        // Debug.Log("horizontal" + horizontal);
        this.vertical = Input.GetAxis("Vertical");
        // Debug.Log("vertical" + vertical);
        Vector2 move = new Vector2(horizontal, vertical);

        if (this.launchTimer > 0f)
        {
            this.launchTimer -= Time.deltaTime;
        }

        this.animator.SetFloat("Speed", move.magnitude);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            move.Normalize();
            animator.SetFloat("Look X", move.x);
            animator.SetFloat("Look Y", move.y);
            this.lookDirection = move;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Launch(this.lookDirection);
        }

    }

    void FixedUpdate()
    {
        transform.position += new Vector3(horizontal, vertical, 0) * this.speed * Time.fixedDeltaTime;

    }

    public void ChangeHealth(int amount)
    {

        this.health_point = Mathf.Clamp(this.health_point + amount, 0, this.max_health_point);
        this.health_bar.setFillAmount((float)this.health_point / this.max_health_point);
        Debug.Log("health_point changes to" + this.health_point);
        if (this.health_point == 0)
        {
            Debug.Log("you die");
        }
        if (amount < 0)
            this.animator.SetTrigger("Hit");
    }

    private void Launch(Vector2 lookDirection)
    {
        if (this.launchTimer <= 0f)
        {
            this.launchTimer = this.launchGap;
            Vector2 offset = new Vector2(0, 0.7f);
            if (!Mathf.Approximately(lookDirection.x, 0f))
            {
                offset.x = lookDirection.x < 0f ? -0.7f : 0.7f;
            }
            else
            {
                offset.y += lookDirection.y < 0f ? -0.2f : 0.2f;
            }

            GameObject projectileObject = Instantiate(this.projectilePrefab, this.rigidBody.position + offset, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, this.launchSpeed);

            animator.SetTrigger("Launch");
        }
    }
}