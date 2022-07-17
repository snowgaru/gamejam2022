using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{


    public GameObject Floor;
    public GameObject FloorParent;


    void Start()
    {
        StartCoroutine(CreateFloor());
        DontDestroyOnLoad(this.gameObject);
    }

    public IEnumerator CreateFloor()
    {
        for(int i = 0; i < SceneLoadManager.Instance.row; i ++)
        {
            for (int j = 0; j < SceneLoadManager.Instance.column; j++)
            {
                Instantiate(Floor, new Vector3(-7.15f + (i * 1.5f), 10, 3 - (j * 1.5f)), Quaternion.identity, FloorParent.transform);
                yield return new WaitForSeconds(0.1f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        SceneLoadManager.Instance.CreatePlayer();
        SceneLoadManager.Instance.CreateMonster();
        FloorManager.Instance.GetFloors(SceneLoadManager.Instance.row * SceneLoadManager.Instance.column);
        FloorManager.Instance.SelectRandomFloor();
        UIManager.Instance.GetPlayerAndEnemy();//플레이어와 에너미 생성된거 가져옴
        UIManager.Instance.SetUI();
    }


}
