using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    #region Singleton
    private static SceneLoadManager instance = null;

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

    public static SceneLoadManager Instance
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
    public GameObject Player;
    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;


    public int currentStage = 1;

    public int row = 5; //가로 갯수
    public int column = 5; //세로 갯수


    public GameObject NextPanel;
    public GameObject RestartPanel;
    public GameObject EndPanel;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void CreateMonster()
    {
        if (currentStage == 1)
        {
            GameObject enemy = Instantiate(Enemy1, new Vector3(6, 5, -1), Quaternion.Euler(new Vector3(90, 0, 0)));
        }
        else if (currentStage == 2)
        {
            GameObject enemy = Instantiate(Enemy2, new Vector3(6, 5, -1), Quaternion.Euler(new Vector3(90, 0, 0)));
        }
        else if (currentStage == 3)
        {
            GameObject enemy = Instantiate(Enemy3, new Vector3(6, 5, -1), Quaternion.Euler(new Vector3(90, 0, 0)));
        }
    }

    public void CreatePlayer()
    {
        GameObject player = Instantiate(Player, new Vector3(-7.15f + ((row / 2) * 1.5f), 5f, 3 - ((column / 2) * 1.5f)),
            Quaternion.Euler(new Vector3(90, 0, 0)));
        player.GetComponent<Player>().CurrentPos = new Vector2(row, column); // 일단 플레이어에 row 와 column값을 넣어주고 플레이어에서 /2를 하여 위치 고정
    }


    public void NextScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
