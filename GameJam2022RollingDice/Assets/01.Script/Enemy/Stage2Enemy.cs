using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Enemy : Enemy
{
    
    private float Strong = 1.6f; // 15뎀
    private float Weak = 0.8f; // 7
   
    public override void Attack()
    {
        base.Attack();

        int random = Random.Range(0, 2);
        // 0 : 쉴드가 있을경우 높은데미지 없을경우 적은데미지 
        // 1 : 쉴드가 있을경우 낮은데미지 없을경우 높은데미지

        switch (random)
        {
            case 0:
                Debug.Log("첫번째 스킬 사용");
                StartCoroutine(Skill0AttackCor());
                break;

            case 1:
                Debug.Log("두번째 스킬 사용");
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
                Debug.Log("실드게이지 - " + (atk * Strong));
                player.GetComponent<Player>().ShiledGauge -= (int)(atk * Strong);
            }

            else if (player.GetComponent<Player>().ShiledGauge <= 0)
            {
                Debug.Log("체력 - " + (int)(atk * Weak));
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

        }
        player.GetComponent<Player>().MyTurnStartEvent?.Invoke();
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

        }
        player.GetComponent<Player>().MyTurnStartEvent?.Invoke();
    }
}
