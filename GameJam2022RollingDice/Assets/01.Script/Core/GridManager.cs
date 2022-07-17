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


    public int row = 5; //���� ����
    public int column = 5; //���� ����

    void Awake()
    {
        Time.timeScale = 1f;
    }

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
                if(FloorParent == null)
                {
                    FloorParent = GameObject.Find("Floor");
                }
                Instantiate(Floor, new Vector3(-7.15f + (i * 1.5f), 10, 3 - (j * 1.5f)), Quaternion.identity, FloorParent.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        CreatePlayer();
        CreateMonster();
        FloorManager.Instance.GetFloors(row * column);
        FloorManager.Instance.SelectRandomFloor();
        UIManager.Instance.GetPlayerAndEnemy();//�÷��̾�� ���ʹ� �����Ȱ� ������
        UIManager.Instance.SetUI();
    }

    public void CreatePlayer()
    {
        GameObject player = Instantiate(Player, new Vector3(-7.15f + ((row / 2) * 1.5f), 5f, 3 - ((column / 2) * 1.5f)),
            Quaternion.Euler(new Vector3(90, 0, 0)));
        player.GetComponent<Player>().CurrentPos = new Vector2(row, column); // �ϴ� �÷��̾ row �� column���� �־��ְ� �÷��̾�� /2�� �Ͽ� ��ġ ����
    }

    public void CreateMonster()
    {
        GameObject enemy = Instantiate(Enemy, new Vector3(6, 5, -1), Quaternion.Euler(new Vector3(90, 0, 0)));
    }

}
