using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;

    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f; // Отступ для позиционирования

    public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck bndCheck;

    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();

        // Вызвать SpawnEnemy() один раз (в 2 секунды при значениях по умолчанию)
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);

        // Словарь с ключами типа WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }
    }

    public void SpawnEnemy() 
    {
        // Выбрать случайный шаблон Enemy для создания 
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        // Разместить вражеский корабль над экраном в случайной позиции Х
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null) 
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        // Установить начальные координаты созданного вражеского корабля
        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        // Снова вызвать SpawnEnemy()
        Invoke("SpawnEnemy", 1f/enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay) 
    {
        Invoke("Restart", delay);
    }

    public void Restart() 
    {
        SceneManager.LoadScene("_Scene_0");
    }

///<summary>
/// Статическая функция, возвращающая WeaponDefinition из статического защищенного поля WEAP_DICT класса Main
///</summary>
///<returns> Экземпляр WeaponDefinition или, если нет такого определения для указанного WeaponType, 
/// возвращает новый экземпляр WeaponDefinition с типом none. </returns>
///<param name = "wt"> Тип оружия WeaponType, для которого требуется получить WeaponDefinition </param>

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt) 
    {
        // Проверить наличие указанного ключа в словаре
        // Попытка извлечь значение по отсутствующему ключу вызовет ошибку, след. инструкция важна!
        if (WEAP_DICT.ContainsKey(wt)) 
        {
            return(WEAP_DICT[wt]);
        }

        // Следующая инструкция возвращает новый экземпляр WeaponDefinition с типом оружия WeaponDefinition.none, 
        // что означает неудачную попытку найти требуемое определение WeaponDefinition.
        return (new WeaponDefinition());
    }
}
