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

    public bool isSetCritical; //Ʈ���� ��� ũ��Ƽ���÷ξ� ������ �ʿ���

    public bool isCantMoveDebuf; //3��° ������ �̵� �Ұ� �����
    public int CantMoveDebuf;
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

    public bool playerRolled = false;

    public float TempNumHp;
    public float TempNumGd;
    private DiceRoll diceRoll;

    public GameObject HealEffect;
    public GameObject AttackEffect;
    public GameObject GuardEffect;

    public AudioClip[] clips; // 0 총쏘기  1 히트  2 재장전  3 뒤질때
    public AudioSource audioSource;
    public void PlayEffect(int num)
    {
        audioSource.clip = clips[num];
        audioSource.PlayOneShot(clips[num]);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
        diceRoll = FindObjectOfType<DiceRoll>();
        StartCoroutine(MoveY());
    }

    Vector3 barVec;
    float hp;
    float fullhp;

    void Update()
    {
        if (CurrentHp > MaxHp)
        {
            CurrentHp = MaxHp;
        }
        barVec = UIManager.Instance.PlayerHpBar.transform.localScale;
        hp = CurrentHp;
        fullhp = MaxHp;
        //TempNumHp = CurrentHp / fullhp;
        UIManager.Instance.PlayerHpBar.transform.localScale = new Vector3(hp / fullhp, barVec.y, barVec.z);
        barVec = UIManager.Instance.PlayerGuardBar.transform.localScale;
        TempNumGd = ShiledGauge;
        UIManager.Instance.PlayerGuardBar.transform.localScale = new Vector3(TempNumGd / 100, barVec.y, barVec.z);

        //test
        if (Input.GetKeyDown(KeyCode.L))
        {
            CurrentHp = 1;
        }

        MoveCheck();

        if (!isMyTurn && !isSkilling)
        {
            isSkilling = true;
            isCanMove = false;
            StartCoroutine(UseSkill());
            //Debug.Log("������ �����̺�Ʈ");
            //Debug.Log("���� �����ϰ� ������ �����ٰ� �̺�Ʈ");

        }
    }

    private void MoveCheck()
    {
        //if (DiceManager.Instance.DiceResult <= 0)
        //    isCanMove = false;
        //else
        //    isCanMove = true;
        if (!isCanMove) return;

        if (Input.GetKeyDown(KeyCode.W) && !Input.GetKeyDown(KeyCode.A)
            && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
        {
            //���� 3�� �̵� �Ұ� üũ
            if (isCantMoveDebuf && CantMoveDebuf == 1)
            {
                CurrentHp -= 20;
                CameraManager.Instance.ShakeVoid(0.1f, 0.25f);
                UIManager.Instance.SetUI();
            };

            if (cantMoveDir == 4) return;

            //return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.y + 1 > column - 1) return; //�ٴ� üũ
            isCanMove = false;
            cantMoveDir = 1;
            StartCoroutine(MoveXZ(Vector2.up));
        }
        if (Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.W)
            && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.D))
        {
            if (isCantMoveDebuf && CantMoveDebuf == 2)
            {
                CurrentHp -= 20;
                CameraManager.Instance.ShakeVoid(0.1f, 0.25f);
                UIManager.Instance.SetUI();
            };

            if (cantMoveDir == 3) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.x + -1 < 0) return; // �ٴ� üũ
            isCanMove = false;
            cantMoveDir = 2;
            StartCoroutine(MoveXZ(Vector2.left));
        }
        if (Input.GetKeyDown(KeyCode.D) && !Input.GetKeyDown(KeyCode.W)
            && !Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.A))
        {
            if (isCantMoveDebuf && CantMoveDebuf == 4)
            {
                CurrentHp -= 20;
                CameraManager.Instance.ShakeVoid(0.1f, 0.25f);
                UIManager.Instance.SetUI();
            };

            if (cantMoveDir == 2) return; // �ǵ��ư��� üũ (�ݴ������ ����)
            if (CurrentPos.x + 1 > row - 1) return; // �ٴ� üũ
            isCanMove = false;
            cantMoveDir = 3;
            StartCoroutine(MoveXZ(Vector2.right));
        }
        if (Input.GetKeyDown(KeyCode.S) && !Input.GetKeyDown(KeyCode.W)
            && !Input.GetKeyDown(KeyCode.A) && !Input.GetKeyDown(KeyCode.D))
        {
            if (isCantMoveDebuf && CantMoveDebuf == 3)
            {
                CurrentHp -= 20;
                CameraManager.Instance.ShakeVoid(0.1f, 0.25f);
                UIManager.Instance.SetUI();
            };

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
                if (previousFloor != null)
                {
                    if (previousFloor.isCritical == true)
                        previousFloor?.SetMaterial(Color.yellow);
                    else
                        previousFloor?.SetMaterial(Color.white);

                    if (previousFloor.isDebuf == true)
                        previousFloor?.SetMaterial(Color.magenta);
                    else
                        previousFloor?.SetMaterial(Color.white);
                }
                previousFloor = currentFloor;
                previousFloor?.SetMaterial(Color.red);

                currentFloor = hit.transform.GetComponent<Floor>();

                if (previousFloor != null) { Changefloors.Add(currentFloor); }
                if (isFinish) { isFinish = false; Changefloors.Add(currentFloor); }

                if (isSpawn) { isSpawn = false; }
                else { Skillfloors.Add(currentFloor); } //����ϴ� ��ų �ٴڵ�

            }
        }
    }

    private IEnumerator UseSkill()
    {
        int skillDamage;
        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        previousFloor.SetMaterial(Color.white); //���������� �ٲ� �� �ٴ� �ٲ��ְ�
        for (int i = 0; i < Skillfloors.Count; i++) //����Ʈ�� �մ� ���ڸ�ŭ ��ų ���
        {
            if (Skillfloors[i].isCritical == true)
            {
                skillDamage = Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage * 2;
            }
            else
            {
                skillDamage = Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage;
            }

            switch (Skillfloors[i].currentSkill)
            {
                case 0:
                    Debug.Log("����!");
                    UIManager.Instance.PlayerSkillText.text = "Attack!";
                    enemy.GetComponent<Enemy>().CurrentHp -= skillDamage;
                    //���� ����Ʈ ��ƼŬ ���
                    UIManager.Instance.SetUI();
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.1f);
                    CameraManager.Instance.ShakeVoid(0.1f, 0.35f);
                    yield return new WaitForSeconds(0.4f);

                    yield return new WaitForSeconds(0.2f);

                    break;

                case 1:
                    Debug.Log("����!");
                    UIManager.Instance.PlayerSkillText.text = "Shield!";
                    ShiledGauge += skillDamage;
                    UIManager.Instance.SetUI();
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    yield return new WaitForSeconds(0.7f);

                    //yield return new WaitForSeconds(0.25f);
                    break;

                case 2:
                    Debug.Log("��!");
                    UIManager.Instance.PlayerSkillText.text = "Heal!";
                    if (CurrentHp + Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage >= 100)
                    {
                        CurrentHp = 100;
                    }
                    else
                    {
                        CurrentHp += skillDamage;
                    }
                    UIManager.Instance.SetUI();
                    StartCoroutine(ChangeFloorIcon(Changefloors[i]));
                    //yield return new WaitForSeconds(1f);

                    yield return new WaitForSeconds(0.7f);
                    break;

                default:
                    yield return new WaitForSeconds(1f);
                    break;
            }

            UIManager.Instance.PlayerSkillText.text = " ";

            if (Skillfloors[i].isDebuf == true)
            {
                CurrentHp -= 15;
            }

            if (Skillfloors[i].isCritical == true)
            {
                //Skillfloors[i].skillList[Skillfloors[i].currentSkill].damage /= 2;
                Skillfloors[i].isCritical = false;
                Skillfloors[i].SetMaterial(Color.white);
                isSetCritical = true;
            } //�̰� �ϰ� ü�� 0�Ǵ°� üũ�ؾ��� ���߿��� ����

            if (enemy.GetComponent<Enemy>().CurrentHp <= 0)
            {
                UIManager.Instance.NextScenePanelOn();
            }

            if (CurrentHp <= 0)
            {
                UIManager.Instance.RestartPanelOn();
            }
        }



        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        //enemy.GetComponent<Enemy>().AttackStartEvent?.Invoke();
        diceRoll.RollingDiceEvent?.Invoke();
        //MyturnSetting();
    }

    public void MyturnSetting()
    {
        //DiceManager.Instance.GetRandomDiceNum(); // ���ο� �ֻ��������� �޾ƿ���
        if (isSetCritical) { isSetCritical = false; FloorManager.Instance.SelectRandomFloor(); } //���� ũ��Ƽ���÷ξ �缳���ؾ��Ѵٸ� �ϸ����

        ShiledGauge = 0; //��������� �ʱ�ȭ
        UIManager.Instance.SetUI();

        Skillfloors.Clear();
        Changefloors.Clear();
        cantMoveDir = 0;
        isMyTurn = true;
        isSkilling = false;
        isCanMove = true;
        diceRoll.isRollStart = true;
    }

    public IEnumerator ChangeFloorIcon(Floor floor)
    {
        //if(floor)
        switch (floor.currentSkill)
        {
            case 0:
                Instantiate(AttackEffect, new Vector3(floor.transform.position.x, 1.6f, floor.transform.position.z), Quaternion.identity);
                PlayEffect(0);
                break;

            case 1:
                Instantiate(GuardEffect, new Vector3(floor.transform.position.x, 1.6f, floor.transform.position.z), Quaternion.identity);
                PlayEffect(1);
                break;

            case 2:
                Instantiate(HealEffect, new Vector3(floor.transform.position.x, 1.6f, floor.transform.position.z), Quaternion.identity);
                PlayEffect(2);
                break;

            default:

                break;
        }
        //�ٲ������ ����Ʈ�� ��ƼŬ �߰� �ؾ��ҵ� �ϴ� ���򺯰����� �صα���
        floor.SetMaterial(Color.black);
        yield return new WaitForSeconds(0.25f);
        floor.ChangeSkill();
        floor.SetMaterial(Color.white);

    }


    private IEnumerator MoveXZ(Vector2 dir) //�յ��¿�� �̵��ϴ� �ڷ�ƾ
    {
        for (int i = 0; i < 15; i++)
        {
            transform.position = new Vector3(transform.position.x + (dir.x * 0.1f), transform.position.y, transform.position.z + (dir.y * 0.1f));
            yield return new WaitForSeconds(0.01f);
        }

        CurrentPos += dir;
        DiceManager.Instance.MinusDice(); //���̽��� ���̳ʽ� ����
        if (DiceManager.Instance.DiceResult == 0) // 0�� �Ǹ� �� ����
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
        diceRoll.RollingDiceEvent?.Invoke();
        //isCanMove = true;
    }

    private void SettingCurrentPos()
    {
        // row�� column�� ������ �ٲ��� ���� currentpos�� ����
        ///////////////////////////////////////////////////���� ���� row�� column���� ¦���� ��� ������ �� ���ľ���
        row = (int)CurrentPos.x;
        column = (int)CurrentPos.y;
        CurrentPos = new Vector2(row / 2, column / 2);
    }
}
