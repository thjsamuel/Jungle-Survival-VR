using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloorInteraction : MonoBehaviour, IGvrPointerHoverHandler
{
    private Player i_player;
	// Use this for initialization
	void Start () {
        i_player = GameObject.FindGameObjectWithTag("Head").transform.GetComponentInChildren<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Event management, get player's camera ray casted pos, used for getting player's destination to walk to
    public void OnClick(BaseEventData event_data)
    {
        // if player clicked on floor, refer to inspector
        PointerEventData pointer = (PointerEventData)event_data;
        if (i_player != null)
        {   // Get player's current lookat position using GVR Pointers
            i_player.LookAt = pointer.pointerCurrentRaycast.worldPosition;
        }
    }

    public void OnGvrPointerHover(PointerEventData eventData) // used for slowing down player's speed if looking at floor
    {
        // if player walkspeed is currently at normal(faster) speed, slow down the player by half of the original speed
        if (i_player.WalkSpeed >= i_player.original_speed)
            i_player.WalkSpeed *= 0.5f;
    }
}
