using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3Enemy : Enemy
{
    public override void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<Player>().isCantMoveDebuf = false;
        base.Start();
        random = Random.Range(1, 4);
    }
    public int debufDamage = 15;
    public override void Attack()
    {
        base.Attack();

        for (int i = 0; i < 25; i++) //디버프 타일 삭제시키기
        {
            FloorManager.Instance.floors[i].DeleteDebuf();
        }

        
        switch(random)
        {
            case 1:
                StartCoroutine(Skill1AttackCor());
                Debug.Log("첫번쨰 스킬");
                break;

            case 2:
                StartCoroutine(Skill2AttackCor());
                Debug.Log("두번쨰 스킬");
                break;

            case 3:
                StartCoroutine(Skill3AttackCor());
                Debug.Log("세번쨰 스킬");
                break;

            default:
                break;
        }
        
        //StartCoroutine(AttackCor());
    }

    public List<int> list;

    private IEnumerator Skill1AttackCor()
    {
        debufDamage = DiceManager.Instance.DiceResult;


        for (int i = 0; i < 25; i++)
        {
            list.Add(i);
        }

        ShuffleList(list);

        int count = DiceManager.Instance.DiceResult;

        for (int i = 0; i < 4 + (count * 2); i++)
        {
            FloorManager.Instance.floors[list[i]].MakeDebuf();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);

        #region //
        /*if (player == null)
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
        */
        #endregion

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        random = Random.Range(1, 4);
        diceRoll.RollingDiceEvent?.Invoke();
    }

    private IEnumerator Skill2AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        //이동 불가는 차례대로 w 1  a  2  s  3  d  4

        int dontMoveInt = Random.Range(1, 5);

        player.GetComponent<Player>().isCantMoveDebuf = true;
        player.GetComponent<Player>().CantMoveDebuf = dontMoveInt;

        Debug.Log(dontMoveInt);

        int count = DiceManager.Instance.DiceResult;

        for (int i = 0; i < count; i++)
        {
            CameraManager.Instance.ShakeVoid(0.1f, 0.35f);
            DiceManager.Instance.MinusDice();
            if (player.GetComponent<Player>().ShiledGauge >= (atk - 2))
            {
                player.GetComponent<Player>().ShiledGauge -= (atk - 2);
            }
            else if (player.GetComponent<Player>().ShiledGauge == 0)
            {
                player.GetComponent<Player>().CurrentHp -= (atk - 2);
                if (CurrentHp + (atk - 2) >= 100)
                {
                    CurrentHp = MaxHp;
                }
                else
                {
                    CurrentHp += (atk - 2);
                }
            }
            else if (player.GetComponent<Player>().ShiledGauge < (atk - 2))
            {
                int num = (atk - 2) - player.GetComponent<Player>().ShiledGauge;
                player.GetComponent<Player>().ShiledGauge = 0;
                player.GetComponent<Player>().CurrentHp -= num;
                if(CurrentHp + num >= 100)
                {
                    CurrentHp = MaxHp;
                }
                else
                {
                    CurrentHp += num;
                }
            }
            UIManager.Instance.SetUI();
            yield return new WaitForSeconds(1f);
            PlayerHPCheck();
        }
        random = Random.Range(1, 4);
        diceRoll.RollingDiceEvent?.Invoke();
    }

    private IEnumerator Skill3AttackCor()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        int count = DiceManager.Instance.DiceResult;
        for (int i = 0; i < count; i++)
        {
            CameraManager.Instance.ShakeVoid(0.1f, 0.35f);
            DiceManager.Instance.MinusDice();
            if (player.GetComponent<Player>().ShiledGauge >= atk)
            {
                player.GetComponent<Player>().ShiledGauge -= atk;
            }
            else if (player.GetComponent<Player>().ShiledGauge == 0)
            {
                player.GetComponent<Player>().CurrentHp -= atk;

                if (CurrentHp + (atk) >= 100)
                {
                    CurrentHp = MaxHp;
                }
                else
                {
                    CurrentHp += (atk / 2);
                }
            }
            else if (player.GetComponent<Player>().ShiledGauge < atk)
            {
                int num = atk - player.GetComponent<Player>().ShiledGauge;
                player.GetComponent<Player>().ShiledGauge = 0;
                player.GetComponent<Player>().CurrentHp -= num;

                if (CurrentHp + num >= 100)
                {
                    CurrentHp = MaxHp;
                }
                else
                {
                    CurrentHp += num;
                }
            }
            UIManager.Instance.SetUI();
            yield return new WaitForSeconds(1f);
            PlayerHPCheck();
        }
        random = Random.Range(1, 4);
        diceRoll.RollingDiceEvent?.Invoke();
    }

    public void ShuffleList<T>(List<T> list)
    {
        int random1;
        int random2;

        T tmp;

        for (int index = 0; index < list.Count; ++index)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            tmp = list[random1];
            list[random1] = list[random2];
            list[random2] = tmp;
        }
    }
}
