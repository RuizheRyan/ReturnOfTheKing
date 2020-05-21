﻿using Photon.Pun;
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

	public GameObject currentTower;
	public GameObject CurrentTower
	{
		set
		{
			currentTower = value;
			photonView.RPC("RPC_SetCurrentTower", RpcTarget.Others, currentTower.GetComponent<PhotonView>().ViewID);
		}
		get
		{
			return currentTower;
		}
	}

	public enum ItemType { A, B };
	private GameObject[] allGoals;

	[SerializeField]private bool playerIsVictory = false;
	[SerializeField]private int numberOfDeadPlayer = 0;

	//private bool monsterIsVictory = false;
	// Start is called before the first frame update
	void Start()
    {
		allGoals = GameObject.FindGameObjectsWithTag("Goal");
		numberOfDeadPlayer = 0;
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

		if(arrivedGoals >= allGoals.Length - 1)
		{
			playerIsVictory = true;
			callloadEndScene(playerIsVictory);
		}
		if (numberOfDeadPlayer >= (PhotonNetwork.PlayerList.Length - 1) && numberOfDeadPlayer != 0)
		{
			callloadEndScene(playerIsVictory);
		}
	}

	private void callloadEndScene(bool playersWin)
	{
		if (PhotonNetwork.IsMasterClient)
		{
			photonView.RPC("RPC_loadEndScene", RpcTarget.All,playersWin);
		}		
	}

	[PunRPC]
	public void RPC_loadEndScene(bool playersWin)
	{
		_gameSettings.playerWin = playersWin;
		if (PhotonNetwork.IsMasterClient)
		{			
			SceneManager.LoadScene(2);
		}
	}

	public void someoneDead()
	{
		numberOfDeadPlayer += 1;
	}

	//[PunRPC]
	//public void RPC_knell()
	//{
	//	Debug.Log("knellCalled");
	//	numberOfDeadPlayer += 1;
	//}
	[PunRPC]
	public void RPC_SetCurrentTower(int viewID)
	{
		currentTower = PhotonView.Find(viewID).gameObject;
	}
}
