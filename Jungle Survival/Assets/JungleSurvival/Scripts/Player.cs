﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/* Player class
 * Contains information about gameplay regarding the player
 * Attributes include speed and the object it is attached to
 * */
public class Player : MonoBehaviour {
    private Vector3 vec_lookAt;
    public Vector3 LookAt
    {
        get { return vec_lookAt; }
        set { vec_lookAt = value; }
    } // GVR lookat vector, init using PointerEventData
    private GameObject go_head; // Player's head gameobject/maincamera, init as parent
    private float move_speed;  // Movement speed multiplier factor
    public float WalkSpeed
    {
        get { return move_speed; }
        set { move_speed = value; }
    }
    public float original_speed; // Original move speed

    bool is_peeking;
    bool is_originalHeadPos;
    public int rotated;
    float lean_threshold;
    bool walking;
    Vector3 origin;
    Vector3 lean_pos;
    private float lerptime;
    Vector3 originleft;
    Vector3 originright;
    Vector3 originmid;
    
	// Use this for initialization
	void Start () {
        go_head = transform.parent.gameObject;
        move_speed = 8.0f;
        original_speed = 8.0f;
        vec_lookAt = origin = go_head.transform.position;
        walking = false;
        rotated = 0;
        is_peeking = false;
        is_originalHeadPos = true;
        lean_threshold = 10;
        lerptime = 0;
	}

    public void resetMoveSpeed()
    {
        move_speed = original_speed;
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }

	// Update is called once per frame
	void Update () {
        // If distance between head and destination is too little, don't move, if too much dist, move to destination
        /*if ((Vector3.Distance(go_head.transform.position, vec_lookAt) > move_speed * Time.deltaTime) && is_originalHeadPos)
        {
            walk(move_speed);
            walking = true;
        }
        // If distance between head and destination is too little, just set destination to head position
        else if (Vector3.Distance(go_head.transform.position, vec_lookAt) < move_speed * Time.deltaTime)
        {
            //go_head.transform.position = vec_lookAt;
            walking = false;
        }

        //ALLInputManager.instance.CheckifDeviceTilt();*/

        oldheadtilt();
        //Vector3 secondpos = new Vector3(originalHeadPos.x - 2.5f, originalHeadPos.y, originalHeadPos.z);
        //Vector3 thirdpos = new Vector3(originalHeadPos.x + 2.5f, originalHeadPos.y, originalHeadPos.z);
       // Vector3 temp = ALLInputManager.instance.MoveBezierCurve2d(transform.GetChild(3).transform.position, transform.GetChild(4).transform.position, transform.GetChild(4).transform.position, transform.GetChild(1).transform.position, lerpvalue, Input.acceleration.x);
        //GameObject gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
       //GameObject gg2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
       // //    GameObject temp3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
       //gg.transform.position = temp;
       //gg2.transform.position = thirdpos;
    }

    private void oldheadtilt()
    {
        // if the head tilt is more than to one side, then player is peeking and lean position needs to be set to the side of the head
        // main camera is more/less than threshold value
        if (WrapAngle(transform.rotation.eulerAngles.z) <= -lean_threshold && rotated < 3 && !is_peeking)
        {
            //Debug.Log("right");
            rotated++;
            if (rotated == 1)
            {
                //origin = vec_lookAt;
                lean_pos = transform.GetChild(1).transform.position; // right
                is_peeking = true;
                is_originalHeadPos = false;
                Vector3 temp = new Vector3(transform.GetChild(2).transform.position.x, transform.GetChild(2).transform.position.y - 2, transform.GetChild(2).transform.position.z);
                originleft = temp;
                originright = transform.GetChild(1).transform.position;
            }
        }
        else if (WrapAngle(transform.rotation.eulerAngles.z) >= lean_threshold && rotated > -3 && !is_peeking)
        {
            //Debug.Log("left");
            rotated--;
            if (rotated == -1)
            {
                //origin = vec_lookAt;
                lean_pos = transform.GetChild(2).transform.position;
                is_peeking = true;
                is_originalHeadPos = false;
                go_head.transform.position = origin;
                Vector3 temp = new Vector3(transform.GetChild(1).transform.position.x, transform.GetChild(1).transform.position.y - 2, transform.GetChild(1).transform.position.z);
                originright = temp;
                originleft = transform.GetChild(2).transform.position;
            }
        }
        //else if (WrapAngle(transform.rotation.eulerAngles.z) <= -lean_threshold && rotated < 3 && is_peeking)
        //{
        //    go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, originleft, (ALLInputManager.instance.GetDeviceRotation() + 5) * Time.deltaTime);
        //    //Vector3 temp = ALLInputManager.instance.MoveBezierCurve2d(transform.GetChild(3).transform.position, transform.GetChild(4).transform.position, transform.GetChild(4).transform.position, transform.GetChild(1).transform.position, lerpvalue, Input.acceleration.x);
        //}

        // HEAD ROTATION RESET
        // if head is not rotated enough and the player is not walking, reset head position and for next tilting, and therefore player is no longer peeking
        else if (rotated == 0 && (WrapAngle(transform.rotation.eulerAngles.z) <= -lean_threshold))
        {
            Vector3 temp = Input.acceleration;
            temp.Normalize();
            Vector3 curvepoint = ALLInputManager.instance.GetPoint(originleft, transform.GetChild(3).transform.position, originright, 0.5f);
            go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, curvepoint, 6 * Time.deltaTime);
            //go_head.transform.position = ALLInputManager.instance.MoveBezierCurve2d(originleft, , originleft, originright, lerptime, (ALLInputManager.instance.GetDeviceRotation() + 5) * Time.deltaTime);
           /// GameObject gg = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //GameObject gg2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //GameObject gg3 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //gg.transform.position = go_head.transform.position;
            //gg2.transform.position = transform.GetChild(3).transform.position;
            //gg3.transform.position = originright;
        }
        else if (rotated == 0 && (WrapAngle(transform.rotation.eulerAngles.z) >= lean_threshold))
        {
            Vector3 curvepoint = ALLInputManager.instance.GetPoint(originright, transform.GetChild(3).transform.position, originleft, 0.5f);
            go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, curvepoint, 6 * Time.deltaTime);
            //go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, originleft, 9 * Time.deltaTime);
            //if ((Vector3.Distance(origin, go_head.transform.position) < ((ALLInputManager.instance.GetDeviceRotation() + 5) * Time.deltaTime)) && is_peeking)
            //{
            //    is_originalHeadPos = true;
            //    is_peeking = false;
            //}
        }

        else if ((WrapAngle(transform.rotation.eulerAngles.z) < lean_threshold && WrapAngle(transform.rotation.eulerAngles.z) > -lean_threshold))
        {
            rotated = 0;
            lerptime = 0;
            //if (Vector3.Distance(origin, go_head.transform.position) > Time.deltaTime)
            //{
            //    Vector3 dir = (origin - go_head.transform.position).normalized;
            //    go_head.transform.position += dir * (3 * Time.deltaTime);
            //}
            //else
            //    go_head.transform.position = origin;

            go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, origin, 5 * Time.deltaTime);
            if ((Vector3.Distance(origin, go_head.transform.position) < (6 * Time.deltaTime)) && is_peeking)
            {
                is_originalHeadPos = true;
                is_peeking = false;
            }
        }

        // if player is currently rotating his head and the leaning is not too far away from the neck origin position, allow leaning
        else if (is_peeking && Vector3.Distance(go_head.transform.position, origin) < 2)
        {
            //go_head.transform.position = ALLInputManager.instance.moveCurve(go_head.transform.position, lean_pos, ALLInputManager.instance.GetDeviceRotation());
            go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, lean_pos, 6 * Time.deltaTime); // can replaace 2 with input.acc.x
        }

        //else if (WrapAngle(transform.rotation.eulerAngles.z) <= -lean_threshold && is_peeking)
        //{
        //    go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, originright, (ALLInputManager.instance.GetDeviceRotation() + 5) * Time.deltaTime);
        //}
        //else if (WrapAngle(transform.rotation.eulerAngles.z) >= lean_threshold && is_peeking)
        //{
        //    go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, originleft, (ALLInputManager.instance.GetDeviceRotation() + 5) * Time.deltaTime);
        //}

    }

    // Walking
    void walk(float spd)
    {
        vec_lookAt = new Vector3(vec_lookAt.x, go_head.transform.position.y, vec_lookAt.z);
        go_head.transform.position = Vector3.MoveTowards(go_head.transform.position, vec_lookAt, spd * Time.deltaTime);  
    }

    void OrientateHead()
    {
        float xtiltVal = (float)-(Input.acceleration.x);
        // Rotate around z-axis
        go_head.transform.parent.transform.Rotate(xtiltVal, 0, 0, Space.Self);
        Debug.Log(xtiltVal);
    }

    void MoveHead()
    {
        float xtiltVal = Input.acceleration.x;
        Vector3 temp;
        temp = transform.right * xtiltVal * 30 * Time.deltaTime;
        go_head.transform.Translate(0, 0, temp.x);

        //Vector3 dir = transform.TransformDirection(transform.right);
        //dir *= xtiltVal;
        //dir *= 3;
        //PlayerController.instance.pc.Move(dir * Time.deltaTime);
        
    }

    void LeanLeft(float deviceRot)
    {
        float currAngle = go_head.transform.parent.rotation.eulerAngles.x;
        if (currAngle > 180.0f)
        {
            currAngle = 360 - currAngle;
        }
        float gradualAngle = Mathf.Lerp(currAngle, deviceRot, 5 * Time.deltaTime);
        go_head.transform.parent.transform.rotation = Quaternion.Euler(gradualAngle, go_head.transform.parent.transform.rotation.eulerAngles.y, go_head.transform.parent.transform.rotation.eulerAngles.z);
    }

    void LeanRight()
    {
        //float currAngle = transform.rotation.eulerAngles.x;
        //float targetAngle = ALLInputManager.instance.device_rot - 360.0f;
        //if (currAngle > 180.0f)
        //{
        //    targetAngle = 360.0f - 20;
        //}
        //float gradualAngle = Mathf.Lerp(currAngle, targetAngle, 2 * Time.deltaTime);
        //go_head.transform.parent.transform.rotation = Quaternion.Euler(gradualAngle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        float angle = 0.0f;
        if (Input.acceleration.x > 0)
        {
            angle = Mathf.MoveTowardsAngle(angle, ALLInputManager.instance.device_rot, 20 * Time.deltaTime);
        }
        go_head.transform.parent.transform.localRotation = Quaternion.AngleAxis(angle, transform.forward);

        float maxRot = 45.0f; //Max neg and pos strafe rotation value
        float rate = 2.0f; //Rate of change for Lerp

        transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, Mathf.Sign(Input.acceleration.x) * maxRot), Time.deltaTime * rate);
        go_head.transform.rotation = transform.localRotation;

    }

    //void LeanLeft()
    //{
    //    float currAngle = go_head.transform.rotation.eulerAngles.z;
    //    if (currAngle > 180.0f)
    //    {
    //        currAngle = 360 - currAngle;
    //    }
    //    float gradualAngle = Mathf.Lerp(currAngle, ALLInputManager.instance.device_rot, 5 * Time.deltaTime);
    //    transform.rotation = Quaternion.Euler(go_head.transform.rotation.eulerAngles.x, go_head.transform.rotation.eulerAngles.y, go_head.transform.rotation.eulerAngles.z);
    //}

    //void MoveHead()
    //{
    //    float xtiltVal = (float)-(Input.acceleration.z);
    //    Vector3 dir = transform.TransformDirection(xtiltVal * transform.right);
    //    PlayerController.instance.pc.Move(dir * Time.deltaTime);
    //    //transform.Translate(, transform.right)
    //    //transform.Translate(xtiltVal * 3 , 0, 0);
    //}

    //head_cam.WorldToViewportPoint() // can be used for ai vision, just need to insert player's position
}
