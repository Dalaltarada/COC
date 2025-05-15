using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Gun : Collectable
{
    [SerializeField]
    protected TMPro.TMP_Text bullet_display;

    [SerializeField]
    [Range(5, 5000)]
    protected int max_bullet;

    [SerializeField]
    protected Transform shoot_start_position;

    [SerializeField]
    [Range(10, 1000)]
    protected float gun_damage = 20;

    public int current_bullet { get; protected set; }

    protected virtual void Start()
    {
        current_bullet = max_bullet;
    }

    protected override void Update()
    {
        base.Update();

        shoot();

        if (is_in_players_pocket && gameObject.activeSelf)
        {
            showAimEffect();
        }

        displayBullet();
    }

    protected abstract void shoot();

    protected virtual void displayBullet()
    {
        if (bullet_display != null)
        {
            bullet_display.text = current_bullet + " / " + max_bullet;
        }
        else
        {
            Debug.LogWarning($"⚠ bullet_display not assigned on {gameObject.name}");
        }
    }

    private void showAimEffect()
    {
        if (shoot_start_position == null) return;

        RaycastHit hit;

        if (Physics.Raycast(shoot_start_position.position, shoot_start_position.forward, out hit))
        {
            if (AimTexture.Instance != null)
            {
                AimTexture.Instance.setPosition(Camera.main.WorldToScreenPoint(hit.point));
            }
        }
    }
}
