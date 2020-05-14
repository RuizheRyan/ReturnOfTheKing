using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	[SerializeField] private GameManager.ItemType thisGoalType;
	[SerializeField] private bool isArrived = false;

	private PickableItem myItemScript;
	private CharacterController myCharacterController;

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PickableItem")
		{
			myItemScript = other.gameObject.GetComponent<PickableItem>();

			if(myItemScript.thisType == thisGoalType)
			{
				isArrived = true;
				// reset character status if charater walk into the collider
				if (other.transform.parent != null)
				{
					myCharacterController = other.gameObject.transform.parent.GetComponent<CharacterController>();
					if (myCharacterController.isHolding)
					{
						myCharacterController.isHolding = false;
					}
				}

				Destroy(other.gameObject);
			}
		}

	}


	public bool GetArrivalStatus()
	{
		return isArrived;
	}
}
