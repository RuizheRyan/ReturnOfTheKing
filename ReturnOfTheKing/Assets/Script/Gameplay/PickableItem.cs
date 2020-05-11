using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
	enum ItemType {A, B};
	[Header("Attributes")]
	[SerializeField] private ItemType thisType;
	[SerializeField] private bool inRange = false;
	[SerializeField] private bool isPicked = false;
	[SerializeField] private float force = 100.0f;
	[SerializeField] private float offset = 2.0f;
	[SerializeField] private GameObject thisPlayer;

	[SerializeField] private LayerMask layerMask;

	[SerializeField] private Camera camera;

	private CharacterController myCharacterController;
	private Rigidbody rb;

	private const float MAX_RAY_DISTANCE = 100f;

	// Start is called before the first frame update
	void Start()
	{
		rb = gameObject.GetComponent<Rigidbody>();
		

	}
	private void OnValidate()
	{
		if(camera == null)
		{
			camera = Camera.main;
		}
	}

	// Update is called once per frame
	void Update()
    {

		InteractWithItem();
		ThrowItem();
	}

	private void InteractWithItem()
	{
		if (thisPlayer != null && myCharacterController.IsPlayerPressedE() )
		{
			if (inRange && !isPicked && !myCharacterController.isPicking)
			{
				transform.parent = thisPlayer.transform;
				transform.position = thisPlayer.transform.position + new Vector3(0f, offset, 0f);
				rb.useGravity = false;
				isPicked = true;
				myCharacterController.isPicking = true;
				
			}
			else if (isPicked && myCharacterController.isPicking)
			{
				transform.parent = null;
				transform.position = thisPlayer.transform.position;
				transform.localRotation = Quaternion.identity;
				isPicked = false;
				rb.useGravity = true;
				myCharacterController.isPicking = false;
			}

		}
	}

	private void ThrowItem()
	{
		Vector2 mousePosition = Input.mousePosition;
		Ray ray = camera.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);

		if(thisPlayer != null && myCharacterController.IsPlayerPressedMouse() && isPicked)
		{
			if(Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE, layerMask))
			{
				Vector3 targetDirection = new Vector3(hit.point.x - thisPlayer.transform.position.x, thisPlayer.transform.position.y, hit.point.z - thisPlayer.transform.position.z);

				targetDirection = Vector3.Normalize(targetDirection);
				transform.parent = null;
				rb.AddForce(targetDirection * force);
				rb.useGravity = true;

				//Vector3 newDirection = Vector3.RotateTowards(thisPlayer.transform.forward, targetDirection, 1f * Time.deltaTime, 0.0f);

			}
			isPicked = false;
			myCharacterController.isPicking = false;
		}



	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			//determine the character that is able to interact with this item
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
