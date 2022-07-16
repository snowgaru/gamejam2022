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


    public bool isCanMove; //�����ϼ� �մ°�
    public bool isMyTurn = true; //�� ���ΰ�
    public bool isSkilling; //��ų�� ������ΰ�

    public bool isSpawn; //ó�� ���������� �ٴڿ� ��� ����ĳ��Ʈ�� ����Ʈ�� �ȵ��� �����ִ°�
    public bool isFinish; //������ ��ġ�� �ٴ��� ü�����÷ξ� ����Ʈ�� �ȵ��� ���ִ°�

    //public bool isSetting; //������ �����ؾ��Ѵ�.

    public Vector2 CurrentPos;

    private int cantMoveDir = 0; // �ѹ� �� ������ ������ �ϴ°� // 1 �� // 2  �� // 3 �� // 4 �Ʒ�
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
            //Debug.Log("������ �����̺�Ʈ");
            //Debug.Log("���� �����ϰ� ������ �����ٰ� �̺�Ʈ");

        }
    }

    private void MoveCheck()
    {
        if (!isCanMove) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (cantMoveDir == 4) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.y + 1 > column - 1) return; //�ٴ� üũ
            isCanMove = false;
            cantMoveDir = 1;
            StartCoroutine(MoveXZ(Vector2.up));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (cantMoveDir == 3) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.x + -1 < 0) return; // �ٴ� üũ
            isCanMove = false;
            cantMoveDir = 2;
            StartCoroutine(MoveXZ(Vector2.left));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (cantMoveDir == 2) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.x + 1 > row - 1) return; // �ٴ� üũ
            isCanMove = false;
            cantMoveDir = 3;
            StartCoroutine(MoveXZ(Vector2.right));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (cantMoveDir == 1) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.y + -1 < 0) return; // �ٴ� üũ
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
                else { Skillfloors.Add(currentFloor); } //����ϴ� ��ų �ٴڵ�

            }
        }
    }

    private IEnumerator UseSkill()
    {
        previousFloor.SetMaterial(Color.white); //���������� �ٲ� �� �ٴ� �ٲ��ְ�
        for(int i = 0; i < Skillfloors.Count; i++) //����Ʈ�� �մ� ���ڸ�ŭ ��ų ���
        {
            switch(Skillfloors[i].currentSkill)
            {
                case 0:
                    Debug.Log("����!");
                    if (enemy == null)
                    {
                        enemy = GameObject.FindGameObjectWithTag("Enemy");
                    }
                    enemy.GetComponent<Enemy>().CurrentHp -=
                        Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
                    //���� ����Ʈ ��ƼŬ ���
                    UIManager.Instance.SetUI();
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.25f);

                    break;

                case 1:
                    Debug.Log("����!");
                    ShiledGauge += Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
                    UIManager.Instance.SetUI();
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.25f);
                    break;

                case 2:
                    Debug.Log("��!");
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
        DiceManager.Instance.GetRandomDiceNum(); // ���ο� �ֻ��������� �޾ƿ���
        Skillfloors.Clear();
        Changefloors.Clear();
        isMyTurn = true;
        isSkilling = false;
        isCanMove = true;
    }

    public IEnumerator ChangeFloorIcon(Floor floor)
    {
        floor.ChangeSkill();
        //�ٲ������ ����Ʈ�� ��ƼŬ �߰� �ؾ��ҵ� �ϴ� ���򺯰����� �صα���
        floor.SetMaterial(Color.black);
        yield return new WaitForSeconds(0.15f);
        floor.SetMaterial(Color.white);
    }


    private IEnumerator MoveXZ(Vector2 dir) //�յ��¿�� �̵��ϴ� �ڷ�ƾ
    {
        for(int i = 0; i < 15; i++)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * 0.1f), transform.position.y, transform.position.z + (dir.y * 0.1f));
            yield return new WaitForSeconds(0.01f);
        }

        CurrentPos += dir;
        DiceManager.Instance.MinusDice(); //���̽��� ���̳ʽ� ����
        if(DiceManager.Instance.DiceResult == 0) // 0�� �Ǹ� �� ����
        {
            isMyTurn = false;
            isFinish = true;
        }
        ShootRay(); //�ؿ��ִ� �� ��������
        isCanMove = true; //�̵��� �� �������� �ٽ� �������� ��
    }

    private IEnumerator MoveY() // ���߿��� �������� �ڷ�ƾ
    {
        for (int i = 1; i <= 50; i++)
        {
            transform.position = new Vector3(transform.position.x, (5.53f - (0.08f * i)), transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }

        SettingCurrentPos(); // row�� column�� ������ �ٲ���
        //�� ���������� ���� ����Ʈ �߰��ϸ� ��������;

        isSpawn = true; // ó�� ������ �ٴ��� ����Ʈ�� �ȵ��� ���ִ� �Һ���

        ShootRay(); //ó�� currentfloor�� �߰����༭ �����̸� currentfloor �� previousfloor�� ����
        isCanMove = true; 
    }

    private void SettingCurrentPos()
    {
        // row�� column�� ������ �ٲ��� ���� currentpos�� ����
        ///////////////////////////////////////////////////���� ���� row�� column���� ¦���� ��� ������ �� ���ľ���
        row = (int)CurrentPos.x;
        column = (int)CurrentPos.y;
        CurrentPos = new Vector2(row / 2,  column / 2);
    }
}
