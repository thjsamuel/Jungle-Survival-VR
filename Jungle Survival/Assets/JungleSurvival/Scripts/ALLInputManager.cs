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

    public Vector3 MoveBezierCurve2d(Vector3 startPos, Vector3 secPos, Vector3 thirdPos, Vector3 finalPos, float lerptime, float dir)
    {
        float speed = Time.deltaTime * dir;
        lerptime += speed;
        if (lerptime > 1)
            lerptime = 1;
        if (lerptime < 0)
            lerptime = 0;
        Vector3 curvePoint = GetPointOnBezierCurve(startPos, secPos, thirdPos, finalPos, lerptime, true);
        return curvePoint;
        //if (currLerpTime < lerpTime)
        //{
        //    GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    GameObject temp2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    GameObject temp3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    temp.transform.position = secondPos;
        //    temp2.transform.position = thirdPos;
        //    temp3.transform.position = curvePoint;
        //}
    }

    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, bool optimised)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result =
            (u3) * p0 +
            (3f * u2 * t) * p1 +
            (3f * u * t2) * p2 +
            (t3) * p3;

        return result;
    }

}
