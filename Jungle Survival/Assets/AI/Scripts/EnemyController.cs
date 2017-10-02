using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    public static EnemyController instance = null;
    public GameObject[] minionList;
    public Vector3[] originalPosList;

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
        minionList = GameObject.FindGameObjectsWithTag("EnemyModel");
        originalPosList = new Vector3[minionList.Length];
        for (int i = 0; i < minionList.Length; ++i)
        {
            originalPosList[i] = minionList[i].transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public EnemyBehaviour getEnemy(int index)
    {
        return minionList[index].GetComponent<EnemyBehaviour>();
    }

    /// <summary>
    /// Resets enemy position
    /// </summary>
    /// <param name="index"></param>
    public void resetEnemy(int index)
    {
        minionList[index].transform.position = originalPosList[index];
    }

    /// <summary>
    /// Resets enemy position object by searching through list for same object as param
    /// </summary>
    /// <param name="minion"></param>
    public void resetEnemy(GameObject minion)
    {
        for (int i = 0; i < minionList.Length; ++i)
        {
            if (minionList[i].Equals(minion))
            {
                minionList[i].transform.position = originalPosList[i];
            }
        }
        //minionList[index].transform.position = originalPosList[index];
    }
}
