using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Enemy : Enemy
{
    public override void Attack()
    {
        base.Attack();
        StartCoroutine(AttackCor());
    }

    private IEnumerator AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        int count = DiceManager.Instance.DiceResult;
        for (int i = 0; i < count; i++)
        {
            DiceManager.Instance.MinusDice();
            Debug.Log("몬스터어택");
            if (player.GetComponent<Player>().ShiledGauge >= atk)
            {
                player.GetComponent<Player>().ShiledGauge -= atk;
            }
            else if (player.GetComponent<Player>().ShiledGauge == 0)
            {
                player.GetComponent<Player>().CurrentHp -= atk;
            }
            else if (player.GetComponent<Player>().ShiledGauge < atk)
            {
                int num = atk - player.GetComponent<Player>().ShiledGauge;
                player.GetComponent<Player>().ShiledGauge = 0;
                player.GetComponent<Player>().CurrentHp -= num;
            }
            UIManager.Instance.SetUI();
            yield return new WaitForSeconds(1f);

        }
        player.GetComponent<Player>().MyTurnStartEvent?.Invoke();
    }
}
