using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public float currLerpTime;
    public float lerpTime = 1f;
	// Use this for initialization
	void Start () {
        currLerpTime = 0;
        lerpTime = 1f;
	}
	
	// Update is called once per frame
	void Update () {
		
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
        transform.position = Vector3.Lerp(transform.position, targetPos, journeyLength);
    }
}
