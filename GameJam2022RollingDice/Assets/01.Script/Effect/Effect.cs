using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyThis",2f);
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
