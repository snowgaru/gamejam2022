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
        //GetRandomDiceNum();
    }


    public void MinusDice()
    {
        DiceResult -= 1;
        UIManager.Instance.CurrentDiceNumText.text = DiceResult.ToString(); // �׽�Ʈ�� text ���߿� �ٲ����
    }

    public void GetRandomDiceNum() // ����� �̰� �ʰ� �ֻ��� �������� ���缭 ������ ��
    {
        DiceResult = Random.Range(1, 7);
        UIManager.Instance.CurrentDiceNumText.text = DiceResult.ToString(); // �׽�Ʈ�� text ���߿� �ٲ����
    }
    public void SetDiceUI()
    {
        UIManager.Instance.CurrentDiceNumText.text = DiceResult.ToString();
    }
}
