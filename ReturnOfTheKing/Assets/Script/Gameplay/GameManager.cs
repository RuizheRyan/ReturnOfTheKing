using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
{
	[SerializeField]
	private GameSettings _gameSettings;

	public GameObject currentTower;
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

<<<<<<< HEAD
	[SerializeField]private bool playerIsVictory = false;
	[SerializeField]private int numberOfDeadPlayer = 0;

	//private bool monsterIsVictory = false;
=======
	[SerializeField]private bool isVictory = false;
>>>>>>> parent of a8222a6... Dead
	// Start is called before the first frame update
	void Start()
    {
		allGoals = GameObject.FindGameObjectsWithTag("Goal");
<<<<<<< HEAD
		numberOfDeadPlayer = 0;
=======
>>>>>>> parent of a8222a6... Dead
	}

    // Update is called once per frame
    void Update()
    {
		CheckWin();
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
<<<<<<< HEAD
			playerIsVictory = true;
			callloadEndScene(playerIsVictory);
		}
		if (numberOfDeadPlayer >= (PhotonNetwork.PlayerList.Length - 1) && numberOfDeadPlayer != 0)
		{
			loadEndScene(playerIsVictory);
=======
			isVictory = true;
			callloadEndScene(isVictory);
>>>>>>> parent of a8222a6... Dead
		}

	}

	private void callloadEndScene(bool playersWin)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				_gameSettings.masterClientWinState = false;
			}
			else
			{
				_gameSettings.masterClientWinState = true;
			}
			photonView.RPC("loadEndScene", RpcTarget.All);
		}
		
	}

	[PunRPC]
	public void loadEndScene()
	{
		Debug.Log("EndCalled");
		SceneManager.LoadScene(2);
	}

	public void checkSomeoneDead()
	{
		photonView.RPC("someoneDead", RpcTarget.All);
	}
	[PunRPC]
	public void someoneDead()
	{
		numberOfDeadPlayer += 1;
	}
}
