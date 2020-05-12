using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum ItemType { A, B };
	private GameObject[] allGoals;
	// Start is called before the first frame update
	void Start()
    {
		allGoals = GameObject.FindGameObjectsWithTag("Goal");
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
