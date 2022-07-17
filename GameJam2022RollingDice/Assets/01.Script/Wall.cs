using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Collider collider;
    public float fixForce = 10;
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

        }
    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.CompareTag("Dice"))
        {
            float randomX = Random.Range(vel.x * -0.8f, vel.x);
            float randomY = Random.Range(vel.y * -0.8f, vel.y);
            float randomZ = Random.Range(vel.z * -0.8f, vel.z);
            Vector3 _vel = new Vector3(randomX, randomY, randomZ);
            other.rigidbody.AddForce(_vel * fixForce);
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Dice"))
        {
            float randomX = Random.Range(vel.x * -0.8f, vel.x);
            float randomY = Random.Range(vel.y * -0.8f, vel.y);
            float randomZ = Random.Range(vel.z * -0.8f, vel.z);
            Vector3 _vel = new Vector3(randomX, randomY, randomZ);
            other.rigidbody.AddForce(_vel * fixForce);
        }
    }
}
