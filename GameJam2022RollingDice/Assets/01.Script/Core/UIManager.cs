using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public Text PlayerHPText;
    public Text PlayerShiledText;

    public Text EnemyHPText;

    public Text CurrentDiceNumText;

    public GameObject player;
    public GameObject enemy;


    public void GetPlayerAndEnemy()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetUI()
    {
        PlayerHPText.text = player.GetComponent<Player>().CurrentHp.ToString();
        PlayerShiledText.text = player.GetComponent<Player>().ShiledGauge.ToString();
        EnemyHPText.text = enemy.GetComponent<Enemy>().CurrentHp.ToString();
    }
}
