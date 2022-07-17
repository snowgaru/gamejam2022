using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp = 100;

    public int atk = 10; // ���� ����� �� �ӽ�;
    public int patternCount;

    public bool isPassive;

    public UnityEvent AttackStartEvent;

    public GameObject player;

    protected DiceRoll diceRoll;

    protected int random;

    public void Update()
    {
        Vector3 tempBar = UIManager.Instance.EnemyHpBar.transform.localScale;
        float TempNum = CurrentHp;
        float TempNum2 = MaxHp;
        UIManager.Instance.EnemyHpBar.transform.localScale = new Vector3(TempNum / TempNum2, tempBar.y, tempBar.z);
        //test
        if (Input.GetKeyDown(KeyCode.P))
        {
            CurrentHp = 1;
        }
    }

    public virtual void Start()
    {
        diceRoll = FindObjectOfType<DiceRoll>();
        StartCoroutine(MoveY());
    }

    private IEnumerator MoveY() // ���߿��� �������� �ڷ�ƾ
    {
        for (int i = 1; i <= 50; i++)
        {
            transform.position = new Vector3(transform.position.x, (5.53f - (0.08f * i)), transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }

    }

    public virtual void Attack()
    {
        //DiceManager.Instance.GetRandomDiceNum();
        //Debug.Log("Attack");
        //DiceRoll diceRoll = FindObjectOfType<DiceRoll>();
        //diceRoll.RollStart();
    }

    public void PlayerHPCheck()
    {
        if (player.GetComponent<Player>().CurrentHp <= 0)
        {
            UIManager.Instance.RestartPanelOn();
        }
    }

}
