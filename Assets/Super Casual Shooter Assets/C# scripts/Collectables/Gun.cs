using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Gun : Collectable
{
    [SerializeField]
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
    }

    protected abstract void shoot();

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
