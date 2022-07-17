using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Enemy : Enemy
{

    private float Strong = 1.8f; // 15��
    private float Weak = 1f; // 7

    public override void Attack()
    {
        base.Attack();

        int random = Random.Range(0, 2);
        // 0 : ���尡 ������� ���������� ������� ���������� 
        // 1 : ���尡 ������� ���������� ������� ����������

        switch (random)
        {
            case 0:
                Debug.Log("ù��° ��ų ���");
                StartCoroutine(Skill0AttackCor());
                break;

            case 1:
                Debug.Log("�ι�° ��ų ���");
                StartCoroutine(Skill1AttackCor());
                break;

            default:
                break;
        }

    }

    private IEnumerator Skill0AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        int count = DiceManager.Instance.DiceResult;
        for (int i = 0; i < count; i++)
        {
            DiceManager.Instance.MinusDice();

            if (player.GetComponent<Player>().ShiledGauge >= (int)(atk * Strong))
            {
                Debug.Log("�ǵ������ - " + (atk * Strong));
                player.GetComponent<Player>().ShiledGauge -= (int)(atk * Strong);
            }

            else if (player.GetComponent<Player>().ShiledGauge <= 0)
            {
                Debug.Log("ü�� - " + (int)(atk * Weak));
                player.GetComponent<Player>().CurrentHp -= (int)(atk * Weak);
            }

            else if (player.GetComponent<Player>().ShiledGauge < (int)(atk * Strong))
            {
                int num = (int)(atk * Strong) - player.GetComponent<Player>().ShiledGauge;
                player.GetComponent<Player>().ShiledGauge = 0;
                player.GetComponent<Player>().CurrentHp -= num;
            }

            UIManager.Instance.SetUI();

            yield return new WaitForSeconds(1f);
            PlayerHPCheck();
        }
        diceRoll.RollingDiceEvent?.Invoke();
    }


    private IEnumerator Skill1AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        int count = DiceManager.Instance.DiceResult;
        for (int i = 0; i < count; i++)
        {
            DiceManager.Instance.MinusDice();

            if (player.GetComponent<Player>().ShiledGauge >= (int)(atk * Weak))
            {
                player.GetComponent<Player>().ShiledGauge -= (int)(atk * Weak);
            }

            else if (player.GetComponent<Player>().ShiledGauge == 0)
            {
                player.GetComponent<Player>().CurrentHp -= (int)(atk * Strong);
            }


            else if (player.GetComponent<Player>().ShiledGauge < (int)(atk * Weak))
            {
                int num = (int)(atk * Weak) - player.GetComponent<Player>().ShiledGauge;
                player.GetComponent<Player>().ShiledGauge = 0;
                player.GetComponent<Player>().CurrentHp -= num;
            }

            UIManager.Instance.SetUI();

            yield return new WaitForSeconds(1f);
            PlayerHPCheck();
        }
        diceRoll.RollingDiceEvent?.Invoke();
    }
}
