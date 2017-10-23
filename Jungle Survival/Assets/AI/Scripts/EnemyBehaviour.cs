using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public float currLerpTime;
    public float bounceLerpTime1;
    public float bounceLerpTime2;
    public bool bounce1;
    public bool bounce2;
    public bool bouncing;
    public float lerpTime = 1f;
    public bool reached;
    public Vector3 target;
    public float f_timescale;
    private AudioSource a_source;
	// Use this for initialization
	void Start () {
        currLerpTime = bounceLerpTime1 = bounceLerpTime2 = 0;
        lerpTime = 1f;
        reached = true;

        bounce1 = bounce2 = true;
        f_timescale = 1;
        a_source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void resetAttributes()
    {
        currLerpTime = 0;
        reached = true;
    }

    public void moveMeshCurved(Vector3 targetPos)
    {
        //currLerpTime += Time.deltaTime;
        //if (currLerpTime > lerpTime)
        //{
        //    currLerpTime = lerpTime;
        //}
        float journeyLength = Vector3.Distance(transform.position, targetPos);
        //float t = currLerpTime / lerpTime;
        journeyLength = 1 / journeyLength;
        //journeyLength *= Time.deltaTime;
        journeyLength = Mathf.Sin(journeyLength * Mathf.PI * (Time.deltaTime * 5));
        if (journeyLength > 0)
            transform.position = Vector3.Lerp(transform.position, targetPos, journeyLength);
        else
            transform.position = Vector3.Lerp(transform.position, targetPos, 1);
    }

    public void moveMeshBezierCurved(Vector3 finalPos, Vector3 midPos, bool dir = true)
    {
        float slowmospeed = (Time.deltaTime * f_timescale) / VRGameManager.instance.rl_environment.actSpeed;
        if (dir)
        {
            currLerpTime += slowmospeed;
            if (currLerpTime > lerpTime)
            {
                currLerpTime = lerpTime;
            }
            if (currLerpTime > 0.5f)
            {
                if (!a_source.isPlaying)
                    a_source.PlayDelayed(0.5f);
            }
            //float journeyLength = Vector3.Distance(transform.position, finalPos);
            //journeyLength = (1 / journeyLength);
            //journeyLength = 1 / (1 - journeyLength * Time.deltaTime);
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y, midPos.z - 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y, midPos.z - 3);
            Vector3 curvePoint = GetPointOnBezierCurve(EnemyController.instance.originalPosList[0], secondPos, thirdPos, finalPos, currLerpTime, true);
            transform.position = curvePoint;
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
        else
        {
            currLerpTime += slowmospeed;
            if (currLerpTime > lerpTime)
                currLerpTime = lerpTime;
            if (currLerpTime > 0.5f)
            {
                if (!a_source.isPlaying)
                    a_source.PlayDelayed(0.5f);
            }
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y, midPos.z + 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y, midPos.z + 3);
            Vector3 curvePoint = GetPointOnBezierCurve(EnemyController.instance.originalPosList[1], secondPos, thirdPos, finalPos, currLerpTime, true);
            transform.position = curvePoint;
        }
    }

    public void moveMeshBezierCurvedReverse(Vector3 startPos, Vector3 finalPos, Vector3 midPos, bool dir = true)
    {
        float slowmospeed = Time.deltaTime / VRGameManager.instance.rl_environment.actSpeed;
        if (dir)
        {
            currLerpTime += slowmospeed;
            if (currLerpTime > lerpTime)
                currLerpTime = lerpTime;
            //float journeyLength = Vector3.Distance(transform.position, finalPos);
            //journeyLength = (1 / journeyLength);
            //journeyLength = 1 / (1 - journeyLength * Time.deltaTime);
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y, midPos.z - 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y, midPos.z - 3);
            Vector3 curvePoint = GetPointOnBezierCurve(startPos, secondPos, thirdPos, finalPos, currLerpTime, true);
            transform.position = curvePoint;
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
        else
        {
            currLerpTime += slowmospeed;
            if (currLerpTime > lerpTime)
                currLerpTime = lerpTime;
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y, midPos.z + 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y, midPos.z + 3);
            Vector3 curvePoint = GetPointOnBezierCurve(startPos, secondPos, thirdPos, finalPos, currLerpTime, true);
            transform.position = curvePoint;
        }
    }

    public void bounceMeshBezier(Vector3 finalPos, Vector3 midPos, bool dir = true)
    {
        float slowmospeed = Time.deltaTime / VRGameManager.instance.rl_environment.actSpeed;
        if (dir)
        {
            if (bounce1 == true)
                bounceLerpTime1 += slowmospeed;
            else
                bounceLerpTime1 -= slowmospeed;
            if (bounceLerpTime1 > lerpTime)
            {
                bounceLerpTime1 = lerpTime;
                bounce1 = false;
            }
            else if (bounceLerpTime1 < 0)
            {
                bounceLerpTime1 = 0;
                bounce1 = true;
            }
            //float journeyLength = Vector3.Distance(transform.position, finalPos);
            //journeyLength = (1 / journeyLength);
            //journeyLength = 1 / (1 - journeyLength * Time.deltaTime);
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y + 3, midPos.z - 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y + 3, midPos.z - 3);
            Vector3 curvePoint = GetPointOnBezierCurve(EnemyController.instance.originalPosList[0], thirdPos, secondPos, EnemyController.instance.originalPosList[0], bounceLerpTime1, true);
            transform.position = curvePoint;
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
        else
        {
            if (bounce2 == true)
                bounceLerpTime2 += slowmospeed;
            else
                bounceLerpTime2 -= slowmospeed;
            if (bounceLerpTime2 > lerpTime)
            {
                bounceLerpTime2 = lerpTime;
                bounce2 = false;
            }
            else if (bounceLerpTime2 < 0)
            {
                bounceLerpTime2 = 0;
                bounce2 = true;
            }
            Vector3 secondPos = new Vector3(midPos.x - 1, midPos.y + 3, midPos.z + 2);
            Vector3 thirdPos = new Vector3(midPos.x - 5, midPos.y + 3, midPos.z + 3);
            Vector3 curvePoint = GetPointOnBezierCurve(EnemyController.instance.originalPosList[1], thirdPos, secondPos, EnemyController.instance.originalPosList[1], bounceLerpTime2, true);
            transform.position = curvePoint;
        }
    }

    Vector3 GetPointOnBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        Vector3 a = Vector3.Lerp(p0, p1, t);
        Vector3 b = Vector3.Lerp(p1, p2, t);
        Vector3 c = Vector3.Lerp(p2, p3, t);
        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);
        Vector3 pointOnCurve = Vector3.Lerp(d, e, t);

        return pointOnCurve;
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
