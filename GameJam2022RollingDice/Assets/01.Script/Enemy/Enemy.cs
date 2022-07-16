using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp = 100;

    public int atk = 10; // 패턴 만들기 전 임시;
    public int[] AttackPattern;

    public UnityEvent AttackStartEvent;

    public GameObject player;

    void Start()
    {
        StartCoroutine(MoveY());
    }

    private IEnumerator MoveY() // 공중에서 떨어지는 코루틴
    {
        for (int i = 1; i <= 50; i++)
        {
            transform.position = new Vector3(transform.position.x, (5.53f - (0.08f * i)), transform.position.z);
            yield return new WaitForSeconds(0.005f);
        }

    }
    
    public void Attack()
    {
        DiceManager.Instance.GetRandomDiceNum();

        StartCoroutine(AttackCor());
    }

    private IEnumerator AttackCor()    
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        int count = DiceManager.Instance.DiceResult;
        for(int i = 0; i < count; i++)
        {
            Debug.Log("몬스터어택");
            player.GetComponent<Player>().CurrentHp -= atk;
            UIManager.Instance.SetUI();
            yield return new WaitForSeconds(1f);
            DiceManager.Instance.MinusDice();
        }
        player.GetComponent<Player>().MyTurnStartEvent?.Invoke();


    }
}
