using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class StaringBehaviour : MonoBehaviour, IGvrPointerHoverHandler {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGvrPointerHover(PointerEventData eventData)
    {
        PlayerController.instance.isStaringAtSomething = true;
    }
}
