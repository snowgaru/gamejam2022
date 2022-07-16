using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class DiceRoll : MonoBehaviour
{
    [Serializable]
    public struct DiceSide
    {
        public GameObject sideObject;
        public int sideValue;
        public bool isColling;
        public DiceSide(GameObject _sideObject, int _sideValue)
        {
            sideObject = _sideObject;
            sideValue = _sideValue;
            isColling = false;
        }
    }

    [SerializeField]
    public DiceSide[] diceSide;

    private Rigidbody rigidbody;
    private bool isRolled = true;
    private bool isStopped = false;
    private float rollPosition = 10f;
    private float checkPosition = 9f;
    private int diceValue = 0;
    private float rollForce = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RollStart();
        }

        if (rigidbody.velocity == Vector3.zero)
        {
            if (transform.position.y >= checkPosition) { }
            else
                isStopped = true;
        }

        if (isStopped && isRolled)
        {
            isRolled = false;
            Debug.Log("Stopped");
            for (int i = 0; i < diceSide.Length; i++)
            {
                if (diceSide[i].isColling)
                {
                    diceValue = diceSide[i].sideValue;
                    Debug.Log(diceValue);
                }
            }
        }
    }

    private void RollStart()
    {
        isRolled = true;
        isStopped = false;
        for (int i = 0; i < diceSide.Length; i++)
        {
            diceSide[i].isColling = false;
        }
        transform.position = new Vector3(4, 0, 0);

        float rotationX = Random.Range(0, 360);
        float rotationY = Random.Range(0, 360);
        float rotationZ = Random.Range(0, 360);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        float force = Random.Range(rollForce * 0.9f, rollForce * 1.1f);
        rigidbody.AddForce(-transform.right * force);
    }
}
