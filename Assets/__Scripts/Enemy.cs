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

    private BoundsCheck bndCheck;

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

    void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        if (otherGO.tag == "ProjectileHero") 
        {
            Destroy(otherGO); // Уничтожить снаряд
            Destroy(gameObject); // Уничтожить игровой объект Enemy
        }
    }

}
