using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class HealthCollectible : MonoBehaviour
{

    private float timer = 0;

    [Header("设置")]
    [Tooltip("交互后不消除对象")]
    public bool consistently = false;
    // [HideInInspector]
    [Tooltip("交互间隔时间")]
    public float gap = 0f;
    [Tooltip("生命影响值")]
    public int changeValue = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        this.timer = 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (this.timer > 0f) {
            this.timer -= Time.deltaTime;
            return;
        } else {
            this.timer = this.gap;
        }
        RubyController rubyController = other.GetComponent<RubyController>();

        if (rubyController != null && ((rubyController.max_health_point > rubyController.health_point && this.changeValue > 0)|| (this.changeValue < 0)))
        {
            rubyController.ChangeHealth(this.changeValue);
            if (!this.consistently) Destroy(this.gameObject);
        }
    }
}


// [CustomEditor(typeof(HealthCollectible))]
// public class HealthCollectibleEditor : Editor {

//     HealthCollectible healthCollectible;

//     private void OnEnable() {
//         this.healthCollectible = target as HealthCollectible;
//     }

//     public override void OnInspectorGUI() {
//         base.OnInspectorGUI();
//         if (healthCollectible.consistently)
//         {
//             // 绘制新的属性字段
//             healthCollectible.gap = EditorGUILayout.FloatField("Gap", healthCollectible.gap);
//         }
//     }
// }