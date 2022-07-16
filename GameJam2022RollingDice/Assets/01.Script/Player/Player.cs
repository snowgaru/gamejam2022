using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool isCanMove;

    public Vector2 CurrentPos;

    private int row;
    private int column;

    void Start()
    {
        StartCoroutine(MoveY());
    }

    void Update()
    {
        MoveCheck();

    }

    private void MoveCheck()
    {
        if (!isCanMove) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (CurrentPos.y + 1 > column - 1) return; //바닥 체크
            isCanMove = false;
            StartCoroutine(MoveXZ(Vector2.up));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (CurrentPos.x + -1 < 0) return; // 바닥 체크
            isCanMove = false;
            StartCoroutine(MoveXZ(Vector2.left));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (CurrentPos.x + 1 > row - 1) return; // 바닥 체크
            isCanMove = false;
            StartCoroutine(MoveXZ(Vector2.right));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (CurrentPos.y + -1 < 0) return; // 바닥 체크
            isCanMove = false;
            StartCoroutine(MoveXZ(Vector2.down));
        }
    }

    private IEnumerator MoveXZ(Vector2 dir) //앞뒤좌우로 이동하는 코루틴
    {
        Debug.Log("movexz");
        for(int i = 0; i < 15; i++)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * 0.1f), transform.position.y, transform.position.z + (dir.y * 0.1f));
            yield return new WaitForSeconds(0.01f);
        }

        CurrentPos += dir;
        Debug.Log(CurrentPos);

        isCanMove = true; //이동이 다 끝났으니 다시 움직여도 됨
    }

    private IEnumerator MoveY() // 공중에서 떨어지는 코루틴
    {
        for (int i = 1; i <= 50; i++)
        {
            transform.position = new Vector3(transform.position.x, (5.53f - (0.08f * i)), transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }

        SettingCurrentPos(); // row와 column을 설정해 줄꺼임
        //다 떨어졌을때 착지 이펙트 추가하면 괜찮을듯;
        isCanMove = true;
        Debug.Log(isCanMove);
    }

    private void SettingCurrentPos()
    {
        // row와 column을 설정해 줄꺼임 또한 currentpos로 변경
        row = (int)CurrentPos.x;
        Debug.Log(row);
        column = (int)CurrentPos.y;
        Debug.Log(column);
        CurrentPos = new Vector2(row / 2,  column / 2);
    }
}
