using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{

    #region Singleton
    private static TurnManager instance = null;

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

    public static TurnManager Instance
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

    public int nowTurn = 0;
    public int PlayerRollTurn = 1;
    public int PlayerTurn = 2;
    public int EnemyRollTurn = 3;
    public int EnemyTurn = 4;
}
