using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance = null;
    public CharacterController pc;
    public Player pawn;
    public bool isStaringAtSomething;
    private float health; 
    private float max_health;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        pc = GetComponent<CharacterController>();
        isStaringAtSomething = false;
        pawn = GameObject.Find("Main Camera").GetComponent<Player>();
        max_health = health = 100;
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ResetStaring()
    {
        isStaringAtSomething = false; 
        //cooldown.startTiming();	
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("EnemyModel"))
        {
            decreaseHealth(100);
        }
    }

    /// <summary>
    /// Decrease health value of player by dmg
    /// </summary>
    /// <param name="damage"></param>
    public void decreaseHealth(float damage)
    {
        if (health > 0)
            health -= damage;
    }

    /// <summary>
    /// Check status of player, checks for param that is true
    /// </summary>
    /// <param name="dead"></param>
    /// <returns></returns>
    public bool checkStatus(bool dead)
    {
        if (dead == true)
        {
            if (Mathf.Approximately(health, 0))
            {
                return true;
            }
        }
        return false;
    }

    public void restoreMaxHealth()
    {
        health = max_health;
    }
}
