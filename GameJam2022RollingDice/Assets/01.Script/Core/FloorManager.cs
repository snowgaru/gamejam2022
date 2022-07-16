using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    #region Singleton
    private static FloorManager instance = null;

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

    public static FloorManager Instance
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

    public List<Floor> floors;

    public void GetFloors(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Floor floor = transform.GetChild(i).GetComponent<Floor>() ;
            floors.Add(floor);
        }
    }

    public void SelectRandomFloor()
    {
        Debug.Log("½ÇÇàµÊ");

        int random = Random.Range(0, floors.Count);
        Debug.Log(random);
        floors[random].isCritical = true;
        floors[random].SetMaterial(Color.yellow);
    }
}
