using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    private float rollPosition = 2f;
    private float checkPosition = 1f;
    public int diceValue = 0;
    private float rollForce = 1000;
    public bool isRollStart = true;

    public bool isPlayerTurn = true;

    public UnityEvent RollingDiceEvent;

    public UnityEvent RollingEndEvent; //이제 적이든 플레이어든 턴을 넘겨야지 isplayerturn을 이용해서 

    public bool isColDontFix = false;

    private Player player;
    private Enemy enemy; public AudioClip[] clips; // 0 총쏘기  1 히트  2 재장전  3 뒤질때
    public AudioSource audioSource;
    public void PlayEffect(int num)
    {
        audioSource.clip = clips[num];
        audioSource.PlayOneShot(clips[num]);
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        wall = FindObjectsOfType<Wall>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isRollStart)
        {
            isRollStart = false;
            //RollStart();
        }

        if (rigidbody.velocity == Vector3.zero)
        {
            if (transform.localPosition.y >= checkPosition) { }
            else if (isColDontFix && CheckFixation())
            {
                float fixationForce = Random.Range(rollForce * 0.3f, rollForce * 0.8f);
                rigidbody.AddForce(-Vector3.right * fixationForce);
                Debug.Log("Fixation Reroll");
            }
            else
            {
                Debug.Log("다이스 멈췄다");
                isStopped = true;
            }
        }

        if (isStopped && isRolled)
        {
            //CheckFixation();

            CheckSideValue();

        }
    }

    public void StartTurn()
    {
        StartCoroutine(StartTurnCor());
    }

    public IEnumerator StartTurnCor()
    {
        yield return new WaitForSeconds(1f);
        if (isPlayerTurn)
        {
            isPlayerTurn = false;
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
            player.MyTurnStartEvent?.Invoke();
        }
        else
        {
            isPlayerTurn = true;
            if (enemy == null)
            {
                enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
            }
            enemy.AttackStartEvent?.Invoke();
        }
    }

    public void RollStart()
    {
        Debug.Log("Rol!!");
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
        transform.localPosition = new Vector3(9, rollPosition, 0);

        float rotationX = Random.Range(0, 360);
        float rotationY = Random.Range(0, 360);
        float rotationZ = Random.Range(0, 360);

        float force = Random.Range(rollForce * 0.8f, rollForce * 1.2f);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);
        rigidbody.AddForce(-Vector3.right * force);
    }

    public bool CheckFixation()
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
        return fixation;

    }

    public void CheckSideValue()
    {
        isRolled = false;
        Debug.Log("");
        for (int i = 0; i < diceSide.Length; i++)
        {
            if (diceSide[i].isColling)
            {
                diceValue = 7 - diceSide[i].sideValue;
                Debug.Log(diceValue);
                DiceManager.Instance.DiceResult = diceValue;
                DiceManager.Instance.SetDiceUI();
                RollingEndEvent?.Invoke();
            }
        }

        //if myturn 
    }

    public void WallClose()
    {
        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].collider.isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("DontFixation"))
        {
            isColDontFix = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        PlayEffect(0);
        if (other.gameObject.CompareTag("DontFixation"))
        {
            isColDontFix = false;
        }
    }
}
