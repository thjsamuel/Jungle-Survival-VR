using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance = null;
    public CharacterController pc;
    public Player pawn;
    public bool isStaringAtSomething;
    private float health;
    private float max_health;
    public Slider u_healthbar;
    private RectTransform t_barTransform;
    private float barwidth;
    private Color barcolor;
    private Transform t_canvas;

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
        pc = GetComponent<CharacterController>();
        isStaringAtSomething = false;
        pawn = GameObject.Find("Main Camera").GetComponent<Player>();
        max_health = health = 100;
        t_barTransform = u_healthbar.GetComponent<RectTransform>();
        barwidth = t_barTransform.sizeDelta.x / max_health;
        barcolor = u_healthbar.transform.GetChild(0).GetComponent<Image>().color;
        t_canvas = u_healthbar.transform.parent;
    }
	
	// Update is called once per frame
	void Update () {
	    //t_barTransform.sizeDelta = new Vector3(t_barTransform.sizeDelta.x, )
        //t_canvas.eulerAngles = new Vector3(0, t_canvas.eulerAngles.y, t_canvas.eulerAngles.z);
	}

    public void ResetStaring()
    {
        isStaringAtSomething = false; 
        //cooldown.startTiming();	
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.CompareTag("EnemyModel"))
        {
            decreaseHealth(20);
        }
    }

    /// <summary>
    /// Decrease health value of player by dmg
    /// </summary>
    /// <param name="damage"></param>
    public void decreaseHealth(float damage)
    {
        if (health > 0)
            health -= damage;
        t_barTransform.sizeDelta = new Vector2(t_barTransform.sizeDelta.x - barwidth * damage, t_barTransform.sizeDelta.y);
        u_healthbar.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, barcolor, health / max_health); // normalize formula, x - min / max - min;
    }

    /// <summary>
    /// Check status of player, checks for param that is true
    /// </summary>
    /// <param name="dead"></param>
    /// <returns></returns>
    public bool checkStatus(bool dead)
    {
        if (dead == true)
        {
            if (Mathf.Approximately(health, 0))
            {
                return true;
            }
        }
        return false;
    }

    public void restoreMaxHealth()
    {
        health = max_health;
        t_barTransform.sizeDelta = new Vector2(max_health * barwidth, t_barTransform.sizeDelta.y);
        u_healthbar.transform.GetChild(0).GetComponent<Image>().color = barcolor;

    }

    public bool checkStatus(float value)
    {
        if (health < value)
            return true;
        return false;
    }
}
