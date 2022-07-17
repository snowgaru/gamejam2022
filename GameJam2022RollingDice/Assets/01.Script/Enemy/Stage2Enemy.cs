using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Enemy : Enemy
{

    private float Strong = 1.8f; // 15��
    private float Weak = 1f; // 7

    public override void Start()
    {
        base.Start();
        int random = Random.Range(0, 2);
    }

    public override void Attack()
    {
        base.Attack();


        // 0 : ���尡 ������� ���������� ������� ���������� 
        // 1 : ���尡 ������� ���������� ������� ����������

        switch (random)
        {
            case 0:
                nextSkillTitle.text = "Shield Break Attack";
                nextSkillExplain.text = "(Deals 18 damage when the guard gauge is present, and 10 damage when it doesn't.) X Dice Eyes";
                Debug.Log("ù��° ��ų ���");
                StartCoroutine(Skill0AttackCor());
                break;

            case 1:
                nextSkillTitle.text = "Heart Break Attack";
                nextSkillExplain.text = "(Deals 18 damage when the guard gauge is not present, and 10 damage when it does.) X Dice Eyes";
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
            CameraManager.Instance.ShakeVoid(0.1f, 0.35f);
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
        random = Random.Range(0, 2);
        diceRoll.RollingDiceEvent?.Invoke();
    }


    private IEnumerator Skill1AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        CameraManager.Instance.ShakeVoid(0.1f, 0.35f);
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
        random = Random.Range(0, 2);
        diceRoll.RollingDiceEvent?.Invoke();
    }
}
