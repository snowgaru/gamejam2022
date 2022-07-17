using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Text PlayerSkillText;

    public GameObject player;
    public GameObject enemy;

    public GameObject RestartPanel;
    public GameObject NextScenePanel;


    public GameObject PlayerHpBar;

    public void GetPlayerAndEnemy()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public void SetUI()
    {
        //PlayerSkillText.text = " ";
        PlayerHPText.text = player.GetComponent<Player>().CurrentHp.ToString();
        PlayerShiledText.text = player.GetComponent<Player>().ShiledGauge.ToString();
        EnemyHPText.text = enemy.GetComponent<Enemy>().CurrentHp.ToString();
    }

    public void RestartPanelOn()
    {
        RestartPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void NextScenePanelOn()
    {
        NextScenePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void SceneStage1()
    {
        SceneManager.LoadScene("Stage1");
    }

    public void SceneStage2()
    {
        SceneManager.LoadScene("Stage2");
    }

    public void SceneStage3()
    {
        SceneManager.LoadScene("Stage3");
    }
}
