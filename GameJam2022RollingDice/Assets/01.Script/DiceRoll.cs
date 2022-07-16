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

    public Rigidbody rigidbody;
    [SerializeField]
    private Wall[] wall;

    [SerializeField]
    private bool isRolled = true;
    [SerializeField]
    private bool isStopped = false;
    private float rollPosition = 9f;
    private float checkPosition = 0f;
    private int diceValue = 0;
    private float rollForce = 1000;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        wall = FindObjectsOfType<Wall>();
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
            {
                isStopped = true;
            }
        }

        if (isStopped)
        {
            if (isRolled)
            {
                //CheckFixation();
                CheckSideValue();
            }
        }
    }

    private void RollStart()
    {
        isRolled = true;
        isStopped = false;

        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].collider.isTrigger = true;
        }

        for (int i = 0; i < diceSide.Length; i++)
        {
            diceSide[i].isColling = false;
        }
        transform.position = new Vector3(rollPosition, 0, 0);

        float rotationX = Random.Range(0, 360);
        float rotationY = Random.Range(0, 360);
        float rotationZ = Random.Range(0, 360);

        float force = Random.Range(rollForce * 0.9f, rollForce * 1.1f);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        rigidbody.AddForce(-Vector3.right * force);

    }

    public void CheckFixation()
    {
        bool fixation = true;
        for (int i = 0; i < diceSide.Length; i++)
        {
            if (diceSide[i].isColling)
            {
                fixation = false;
                break;
            }
        }
        if (fixation)
        {
            float fixationForce = Random.Range(rollForce * 0.3f, rollForce * 0.8f);
            rigidbody.AddForce(-Vector3.right * fixationForce);
            Debug.Log("Fixation Reroll");
        }
    }

    public void CheckSideValue()
    {
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

    public void WallClose()
    {
        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].collider.isTrigger = false;
        }
    }
}
