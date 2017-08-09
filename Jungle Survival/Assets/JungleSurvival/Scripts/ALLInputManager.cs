using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ALLInputManager : MonoBehaviour  {
    public static ALLInputManager instance;

    public enum HeadState
    {
        NEUTRAL,
        TILT, // Input for android phone if it rotates around Z clockwise and anti-cw
        TURN, // if player looks left and right
    }
    [HideInInspector]
    public HeadState head_orient;
    public float device_rot;

    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

	// Use this for initialization
	void Start () {
        head_orient = HeadState.NEUTRAL;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public bool CheckifDeviceTilt()
    {
        float xVal = Input.acceleration.x;
        if (xVal > 0.1 || xVal < -0.1)
        {
        //    // is tilting
            if (xVal < 0.5 && xVal > -0.5)
            {
                // Has not passed limit of how much player head can tilt, allow tilting
                head_orient = HeadState.TILT;
                device_rot = xVal * 100f;
                return true;
            }
        }
        head_orient = HeadState.NEUTRAL;
        return false;
    }

    public float GetDeviceRotation()
    {
        device_rot = Input.acceleration.x;
        if (device_rot < 0)
            device_rot *= -1;
        return device_rot;
    }

}
