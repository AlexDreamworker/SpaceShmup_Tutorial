using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;

    [Header("Set Dynamically")]
    [SerializeField] private float _shieldLevel = 1;

    // Переменная хранит ссылку на последний столкнувшийся игровой объект
    private GameObject lastTriggerGo = null;

    private void Awake()
    {
        if (S == null) 
        {
            S = this;
        }
        else 
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
    }

    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        // Гарантировать невозможность повторного столкновения с тем же объектом
        if (go == lastTriggerGo) 
        {
            return;
        }
        lastTriggerGo = go;

        // Если защитное поле столкнулось с вражеским кораблем...
        if (go.tag == "Enemy") 
        {
            shieldLevel--; // Уменьшить уровень защиты на 1
            Destroy(go); // Уничножить врага
        }
    }

    public float shieldLevel 
    {
        get { return(_shieldLevel); }
        set 
        { 
            _shieldLevel = Mathf.Min(value, 4);

            // Если уровень поля упал до нуля или ниже
            if (value < 0) 
            {
                Destroy(this.gameObject);

                // Сообщить объекту Main.S о необходимости перезагрузить игру
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
