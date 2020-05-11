using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{

	[SerializeField] private bool inRange = false;
	[SerializeField] private bool isPicked = false;
	[SerializeField] private GameObject thisPlayer;

	private CharacterController myCharacterController;

	// Start is called before the first frame update
	void Start()
	{



	}

	// Update is called once per frame
	void Update()
    {

		InteractWithItem();
	}

	private void InteractWithItem()
	{
		if (Input.GetKeyUp(KeyCode.F))
		{
			if (thisPlayer != null && inRange && !isPicked)
			{
				transform.parent = thisPlayer.transform;
				transform.position = thisPlayer.transform.position + new Vector3(0f, 0.5f, 0f);
				isPicked = true;
			}
			else if (thisPlayer != null && isPicked)
			{
				transform.parent = null;
				transform.position = thisPlayer.transform.position;
				transform.localRotation = Quaternion.identity;
				isPicked = false;
			}

		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			thisPlayer = other.gameObject;
			myCharacterController = thisPlayer.GetComponent<CharacterController>();
			inRange = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			thisPlayer = null;
			inRange = false;
		}
	}

}
