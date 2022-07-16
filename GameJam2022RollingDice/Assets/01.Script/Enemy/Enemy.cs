using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp = 100;

    public int atk = 10; // 패턴 만들기 전 임시;
    public int patternCount;

    public bool isPassive;

    public UnityEvent AttackStartEvent;

    public GameObject player;

    public virtual void Start()
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
    
    public virtual void Attack()
    {
        DiceManager.Instance.GetRandomDiceNum();
    }

    
}
