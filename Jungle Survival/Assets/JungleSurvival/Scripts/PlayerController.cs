﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance = null;
    public CharacterController pc;

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
