using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private int health;
    // Start is called before the first frame update
    public static PlayerCharacter instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        health = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hurt(int damage)
    {
        health -= damage;
        Debug.Log($"Health: {health}");
    }
}
