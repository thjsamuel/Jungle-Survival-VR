using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance = null;
    public CharacterController pc;
    public Player pawn;
    public bool isStaringAtSomething;
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
        pc = gameObject.AddComponent<CharacterController>();
        isStaringAtSomething = false;
        pawn = GameObject.Find("Main Camera").GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ResetStaring()
    {
        isStaringAtSomething = false; 
        //cooldown.startTiming();	
    }
}
