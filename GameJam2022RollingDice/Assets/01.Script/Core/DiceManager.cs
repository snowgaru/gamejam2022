using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceManager : MonoBehaviour
{
    #region Singleton
    private static DiceManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static DiceManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public int DiceResult;

    void Start()
    {
        GetRandomDiceNum();
    }

    public void MinusDice()
    {
        DiceResult -= 1;
        UIManager.Instance.CurrentDiceNumText.text = DiceResult.ToString(); // 테스트용 text 나중에 바꿔야함
    }

    public void GetRandomDiceNum() // 정배야 이건 너가 주사위 떨어진거 맞춰서 넣으면 됨
    {
        DiceResult = Random.Range(1, 7);
        UIManager.Instance.CurrentDiceNumText.text = DiceResult.ToString(); // 테스트용 text 나중에 바꿔야함
    }
}
