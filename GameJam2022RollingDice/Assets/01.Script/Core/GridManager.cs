using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject Floor;
    public GameObject FloorParent;

    public GameObject Player;
    public GameObject Enemy;

    public int row = 5; //가로 갯수
    public int column = 5; //세로 갯수

    void Start()
    {
        StartCoroutine(CreateFloor());
    }

    public IEnumerator CreateFloor()
    {
        for(int i = 0; i < row; i ++)
        {
            for (int j = 0; j < column; j++)
            {
                Instantiate(Floor, new Vector3(-7.15f + (i * 1.5f), 5, 3 - (j * 1.5f)), Quaternion.identity, FloorParent.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        CreatePlayer();
        CreateMonster();
        UIManager.Instance.GetPlayerAndEnemy();//플레이어와 에너미 생성된거 가져옴
        UIManager.Instance.SetUI();
    }

    private void CreateMonster()
    {
        GameObject enemy = Instantiate(Enemy, new Vector3(6, 5, -1), Quaternion.Euler(new Vector3(90,0,0)) );
    }

    public void CreatePlayer()
    {
        GameObject player = Instantiate(Player, new Vector3(-7.15f + ((row / 2) * 1.5f), 5f, 3 - ((column / 2) * 1.5f)), 
            Quaternion.Euler(new Vector3(90,0,0)) );
        player.GetComponent<Player>().CurrentPos = new Vector2(row, column); // 일단 플레이어에 row 와 column값을 넣어주고 플레이어에서 /2를 하여 위치 고정
    }
}
