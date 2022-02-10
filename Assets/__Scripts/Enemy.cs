using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f; // Секунды между выстрелами (не используется)
    public float health = 10;
    public int score = 100; // Очки за уничтожение этого корабля

    protected BoundsCheck bndCheck;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // Это свойство: метод, действующий как поле
    public Vector3 pos 
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }

    private void Update()
    {
        Move();

        if (bndCheck != null && bndCheck.offDown) 
        {
            // Корабль за нижней границей, поэтому его нужно уничтожить
            Destroy(gameObject);
        }
    }

    public virtual void Move() 
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    // void OnCollisionEnter(Collision coll)
    // {
    //     GameObject otherGO = coll.gameObject;
    //     if (otherGO.tag == "ProjectileHero") 
    //     {
    //         Destroy(otherGO); // Уничтожить снаряд
    //         Destroy(gameObject); // Уничтожить игровой объект Enemy
    //     }
    // }

    void OnCollisionEnter(Collision coll) 
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag) 
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();

                // Если вражеский корабль за границами экрана, не наносить ему повреждений
                if (!bndCheck.isOnScreen) 
                {
                    Destroy(otherGO);
                    break;
                }

                // Поразить вражеский корабль
                // Получить разрушающую силу из WEAP_DICT в классе Main
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            
            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

}
