using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Collider collider;
    public float fixForce = 300f;
    public Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dice"))
        {
            //collider.isTrigger = false;
            DiceRoll diceRoll = FindObjectOfType<DiceRoll>();
            diceRoll.WallClose();
            diceRoll.rigidbody.AddForce(vel * fixForce);
        }
    }
}
