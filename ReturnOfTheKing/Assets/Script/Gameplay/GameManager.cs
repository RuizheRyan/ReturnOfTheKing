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
	private static GameManager _instance;
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<GameManager>();
			}
			return _instance;
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
	public GameObject deadPlayer = null;
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
		Debug.Log("someoneDead");
		//numberOfDeadPlayer += 1;
		Debug.Log("deadNumber:" + numberOfDeadPlayer);
		photonView.RPC("RPC_knell", RpcTarget.MasterClient);
	}
	public void someoneRelive()
	{
		//numberOfDeadPlayer -= 1;
		photonView.RPC("RPC_Relive", RpcTarget.MasterClient);
	}

	[PunRPC]
	public void RPC_knell()
	{
		Debug.Log("knellCalled");
		numberOfDeadPlayer += 1;
		Debug.Log("deadNumber:" + numberOfDeadPlayer);
	}
	[PunRPC]
	public void RPC_Relive()
	{
		numberOfDeadPlayer -= 1;
		Debug.Log("deadNumber:" + numberOfDeadPlayer);
	}
	[PunRPC]
	public void RPC_SetCurrentTower(int viewID)
	{
		currentTower = PhotonView.Find(viewID).gameObject;
	}
}
