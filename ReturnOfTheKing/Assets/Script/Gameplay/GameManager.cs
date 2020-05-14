using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//singleton
	public static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}

	public enum ItemType { A, B };
	private GameObject[] allGoals;

	[SerializeField]private bool isVictory = false;
	// Start is called before the first frame update
	void Start()
    {
		allGoals = GameObject.FindGameObjectsWithTag("Goal");
	}

    // Update is called once per frame
    void Update()
    {
		//CheckWin();

    }

	public void CheckWin()
	{
		int arrivedGoals = 0;
		foreach(GameObject goal in allGoals)
		{
			if (goal.GetComponent<Goal>().GetArrivalStatus())
			{
				arrivedGoals += 1;
			}
		}

		if(arrivedGoals == allGoals.Length)
		{
			isVictory = true;
		}

	}
}
