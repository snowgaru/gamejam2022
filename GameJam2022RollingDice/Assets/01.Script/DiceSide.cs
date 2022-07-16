using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    public int index;
    private DiceRoll diceRoll;
    // Start is called before the first frame update
    void Start()
    {

        diceRoll = FindObjectOfType<DiceRoll>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Col");
        if (other.gameObject.CompareTag("Stand"))
            diceRoll.diceSide[index].isColling = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Stand"))
            diceRoll.diceSide[index].isColling = false;
    }
}
