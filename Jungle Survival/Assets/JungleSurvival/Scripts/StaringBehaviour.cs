using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaringBehaviour : MonoBehaviour, IGvrPointerHoverHandler {
    public bool spotted;
    private Timer countToReset;

	// Use this for initialization
	void Start () {
        spotted = false;
        countToReset = gameObject.AddComponent<Timer>();
        countToReset.initTimer(5);
	}
	
	// Update is called once per frame
	void Update () {
        if (spotted)
        {
            if (countToReset.runAction)
            {
                spotted = false;
                countToReset.runAction = false;
            }
        }

	}

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        spotted = true;
        countToReset.startTiming();
        PlayerController.instance.isStaringAtSomething = true;
    }
}
