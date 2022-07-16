using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int MaxHp;
    public int CurrentHp;
    public int ShiledGauge;


    public bool isCanMove; //움직일수 잇는가
    public bool isMyTurn = true; //내 턴인가
    public bool isSkilling; //스킬을 사용중인가

    public bool isSpawn; //처음 스폰했을때 바닥에 쏘는 레이캐스트가 리스트에 안들어가게 막아주는것
    public bool isFinish; //마지막 위치의 바닥은 체인지플로어 리스트에 안들어가게 해주는거

    //public bool isSetting; //세팅을 시작해야한다.

    public Vector2 CurrentPos;

    private int cantMoveDir = 0; // 한번 간 방향은 못가게 하는것 // 1 위 // 2  왼 // 3 오 // 4 아래
    private int row;
    private int column;

    public List<Floor> Skillfloors;
    public List<Floor> Changefloors;
    public Floor currentFloor;
    public Floor previousFloor;

    public UnityEvent MyTurnStartEvent;

    public GameObject enemy;

    void Start()
    {
        StartCoroutine(MoveY());
    }

    void Update()
    {
        MoveCheck();
        
        if(!isMyTurn && !isSkilling)
        {
            isSkilling = true;
            isCanMove = false;
            StartCoroutine( UseSkill());
            //Debug.Log("적한테 공격이벤트");
            //Debug.Log("적이 공격하고 나한테 끝났다고 이벤트");

        }
    }

    private void MoveCheck()
    {
        if (!isCanMove) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (cantMoveDir == 4) return; // 되돌아가기 체크 (반대방향의 숫자)
            if (CurrentPos.y + 1 > column - 1) return; //바닥 체크
            isCanMove = false;
            cantMoveDir = 1;
            StartCoroutine(MoveXZ(Vector2.up));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (cantMoveDir == 3) return; // 되돌아가기 체크 (반대방향의 숫자)
            if (CurrentPos.x + -1 < 0) return; // 바닥 체크
            isCanMove = false;
            cantMoveDir = 2;
            StartCoroutine(MoveXZ(Vector2.left));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (cantMoveDir == 2) return; // 되돌아가기 체크 (반대방향의 숫자)
            if (CurrentPos.x + 1 > row - 1) return; // 바닥 체크
            isCanMove = false;
            cantMoveDir = 3;
            StartCoroutine(MoveXZ(Vector2.right));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (cantMoveDir == 1) return; // 되돌아가기 체크 (반대방향의 숫자)
            if (CurrentPos.y + -1 < 0) return; // 바닥 체크
            isCanMove = false;
            cantMoveDir = 4;
            StartCoroutine(MoveXZ(Vector2.down));
        }
    }


    private void ShootRay()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red, 1f);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
        {
            if (hit.transform.CompareTag("Floor"))
            {
                previousFloor?.SetMaterial(Color.white);
                previousFloor = currentFloor;
                previousFloor?.SetMaterial(Color.red);

                if (previousFloor != null) { Changefloors.Add(currentFloor); }


                currentFloor = hit.transform.GetComponent<Floor>();


                if(isFinish) { isFinish = false; Changefloors.Add(previousFloor); }

                if (isSpawn) { isSpawn = false; }
                else { Skillfloors.Add(currentFloor); } //사용하는 스킬 바닥들

            }
        }
    }

    private IEnumerator UseSkill()
    {
        previousFloor.SetMaterial(Color.white); //빨간색으로 바뀐 전 바닥 바꿔주고
        for(int i = 0; i < Skillfloors.Count; i++) //리스트에 잇는 숫자만큼 스킬 사용
        {
            switch(Skillfloors[i].currentSkill)
            {
                case 0:
                    Debug.Log("공격!");
                    if (enemy == null)
                    {
                        enemy = GameObject.FindGameObjectWithTag("Enemy");
                    }
                    enemy.GetComponent<Enemy>().CurrentHp -=
                        Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
                    //어택 이펙트 파티클 출력
                    UIManager.Instance.SetUI();
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.25f);

                    break;

                case 1:
                    Debug.Log("가드!");
                    ShiledGauge += Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
                    UIManager.Instance.SetUI();
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.25f);
                    break;

                case 2:
                    Debug.Log("힐!");
                    if(CurrentHp + Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage > 100)
                    {
                        CurrentHp = 100;
                    }
                    else 
                    {
                        CurrentHp += Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
                    }
                    UIManager.Instance.SetUI();
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.25f);
                    break;

                default:
                    yield return new WaitForSeconds(1f);
                    break;
            }
        }
        
       

        if(enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        enemy.GetComponent<Enemy>().AttackStartEvent?.Invoke();

        //MyturnSetting();
    }

    public void MyturnSetting()
    {
        DiceManager.Instance.GetRandomDiceNum(); // 새로운 주사위랜덤값 받아오고
        Skillfloors.Clear();
        Changefloors.Clear();
        isMyTurn = true;
        isSkilling = false;
        isCanMove = true;
    }

    public IEnumerator ChangeFloorIcon(Floor floor)
    {
        floor.ChangeSkill();
        //바뀌었을때 이팩트나 파티클 추가 해야할듯 일단 색깔변경으로 해두긴함
        floor.SetMaterial(Color.black);
        yield return new WaitForSeconds(0.15f);
        floor.SetMaterial(Color.white);
    }


    private IEnumerator MoveXZ(Vector2 dir) //앞뒤좌우로 이동하는 코루틴
    {
        for(int i = 0; i < 15; i++)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * 0.1f), transform.position.y, transform.position.z + (dir.y * 0.1f));
            yield return new WaitForSeconds(0.01f);
        }

        CurrentPos += dir;
        DiceManager.Instance.MinusDice(); //다이스값 마이너스 해줌
        if(DiceManager.Instance.DiceResult == 0) // 0이 되면 턴 종료
        {
            isMyTurn = false;
            isFinish = true;
        }
        ShootRay(); //밑에있는 값 가져와줌
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

        isSpawn = true; // 처음 스폰한 바닥은 리스트에 안들어가게 해주는 불변수

        ShootRay(); //처음 currentfloor를 추가해줘서 움직이면 currentfloor 가 previousfloor로 가게
        isCanMove = true; 
    }

    private void SettingCurrentPos()
    {
        // row와 column을 설정해 줄꺼임 또한 currentpos로 변경
        ///////////////////////////////////////////////////현재 버그 row와 column값이 짝수일 경우 오류가 남 고쳐야해
        row = (int)CurrentPos.x;
        column = (int)CurrentPos.y;
        CurrentPos = new Vector2(row / 2,  column / 2);
    }
}
