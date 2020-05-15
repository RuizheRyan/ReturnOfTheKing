using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPun
{
	[SerializeField]
	private GameSettings _gameSettings;
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
	private GameObject[] allPlayers;

	[SerializeField]private bool playerIsVictory = false;

	//private bool monsterIsVictory = false;
	// Start is called before the first frame update
	void Start()
    {
		allGoals = GameObject.FindGameObjectsWithTag("Goal");
		allPlayers = GameObject.FindGameObjectsWithTag("Player");
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
			playerIsVictory = true;
			callloadEndScene(playerIsVictory);
		}
		bool existAlivePlayer = false;
		foreach (GameObject player in allPlayers)
		{		
			CharacterController playerController = player.GetComponent<CharacterController>();
			if (playerController.currentHealth <= 0)
			{
				playerController.checkSelfDeadState();
			}
			else
			{
				existAlivePlayer |= true; 
			}			
		}
		if (!existAlivePlayer)
		{
			//monsterIsVictory = true;
			callloadEndScene(playerIsVictory);
		}
	}

	private void callloadEndScene(bool playersWin)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("loadEndScene", RpcTarget.All,playersWin);
		}
		
	}

	[PunRPC]
	public void loadEndScene(bool playersWin)
	{
		_gameSettings.playerWin = playersWin;
		SceneManager.LoadScene(2);
	}
}
