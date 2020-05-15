﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CharacterController : MonoBehaviourPun
{
	[Header("Attributes")]
	public float fullHealth = 100f;
	[SerializeField] private float rotateSpeed = 4f;
	[SerializeField] private float normalSpeed = 4f;
	[SerializeField] private float slowDownSpeed = 2f;
	[SerializeField] private float coolDownTime = 10f;
	[SerializeField] private float throwForce = 5;
	[SerializeField] private float secondsToFrozen = 3f;

	[Header("Debugging")]
	public bool hasThrown = false;
	public bool isDetected = false;
	public float currentHealth;
	[SerializeField] private float timer = 0f;
	public bool isHolding = false;

	//[SerializeField] private Collider PickableItemACollider;
	//[SerializeField] private Collider PickableItemBCollider;

	Vector3 forward, right;
	private float moveSpeed;

	Transform mainCam;
	Rigidbody rb;
	GameObject theOneRing;

	public PhotonView photonView;
	// Start is called before the first frame update
	void Start()
    {
		photonView = PhotonView.Get(this);
		rb = GetComponent<Rigidbody>();
		mainCam = Camera.main.transform;
		CoordinationSetting();
		currentHealth = fullHealth;

		GameObject[] allItems;
		allItems = GameObject.FindGameObjectsWithTag("PickableItem");
		foreach(GameObject item in allItems)
		{
			Physics.IgnoreCollision(item.GetComponent<BoxCollider>(), GetComponent<Collider>());
		}
		//Physics.IgnoreCollision(PickableItemACollider, GetComponent<Collider>());
		//Physics.IgnoreCollision(PickableItemBCollider, GetComponent<Collider>());


	}

    // Update is called once per frame
    void Update()
    {
		if (base.photonView.IsMine)
		{
			ToggleSpeed();

			Move();
			ToggleCoolDown();

			if (theOneRing != null)
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					if (isHolding)
					{
						photonView.RPC("RPC_DropItem", RpcTarget.Others);
						DropItem();
					}
					else
					{
						photonView.RPC("RPC_PickUpItem", RpcTarget.Others, PhotonView.Get(theOneRing).ViewID);
						PickUpItem();
					}
				}
				if (Input.GetMouseButtonDown(0) && isHolding)
				{
					photonView.RPC("RPC_ThrowItem", RpcTarget.Others);
					ThrowItem();
				}
			}
			else
			{
			}
		}

	}

	void CoordinationSetting()
	{
		forward = Camera.main.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize(forward);
		right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
	}

	void Move()
	{
		//Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
		Vector3 rightMovement = mainCam.right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
		Vector3 upMovement = mainCam.forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");
		rightMovement.z = 0;
		upMovement.z = 0;

		Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
		heading = Quaternion.Euler(0, 0, 1) * heading;

		if(Input.GetAxis("HorizontalKey") != 0 || Input.GetAxis("VerticalKey") != 0)
		{
			transform.up = Vector3.RotateTowards(transform.up, heading, rotateSpeed, 0f);
		}

		//transform.position += heading * moveSpeed * Time.deltaTime;
		rb.velocity = heading * moveSpeed;

		//transform.position += rightMovement;
		//transform.position += upMovement;
	}


	public bool IsPlayerPressedE()
	{
		return Input.GetKeyUp(KeyCode.E);
	}

	public bool IsPlayerPressedMouse()
	{
		return Input.GetMouseButtonUp(0);
	}

	void ToggleCoolDown()
	{
		if (hasThrown)
		{
			timer += Time.deltaTime;
		}
		if(timer >= coolDownTime)
		{
			hasThrown = false;
			timer = 0;
		}
	}

	void ToggleSpeed()
	{
		//keep movespeed at normal speed when not hit by boss
		moveSpeed += normalSpeed * Time.deltaTime;
		moveSpeed = Mathf.Clamp(moveSpeed, 0, normalSpeed);
	}


	public void UnderAttack(int damage)
	{
		currentHealth -= damage * Time.deltaTime;
		if(moveSpeed > slowDownSpeed)
		{
			moveSpeed -= 2 * normalSpeed;
		}
		else
		{
			moveSpeed -= (normalSpeed +  slowDownSpeed * Time.deltaTime / secondsToFrozen);
		}
	}

	void PickUpItem()
	{
		
		isHolding = true;
		theOneRing.GetComponent<Rigidbody>().isKinematic = true;
		theOneRing.transform.SetParent(transform);
		theOneRing.transform.localPosition = Vector3.forward * -2;
		//theOneRing.GetComponent<PhotonView>().Synchronization = ViewSynchronization.Off;
		theOneRing.GetComponent<PhotonView>().TransferOwnership(photonView.Owner);
	}
	[PunRPC]
	void RPC_PickUpItem(int pvid)
	{
		//var pv = PhotonView.Get(obj);
		var temp = PhotonView.Find(pvid).gameObject;
		//pv.Synchronization = ViewSynchronization.Off;
		isHolding = true;
		temp.GetComponent<Rigidbody>().isKinematic = true;
		temp.transform.SetParent(transform);
		temp.transform.localPosition = Vector3.forward * -2;
	}


	void DropItem()
	{
		isHolding = false;
		theOneRing.GetComponent<Rigidbody>().isKinematic = false;
		theOneRing.transform.SetParent(null);
		theOneRing.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
		theOneRing = null;
	}
	[PunRPC]
	void RPC_DropItem()
	{
		var pv = PhotonView.Get(theOneRing);
		var temp = PhotonView.Find(pv.ViewID).gameObject;
		isHolding = false;
		temp.GetComponent<Rigidbody>().isKinematic = false;
		temp.transform.SetParent(null);
		temp.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
		theOneRing = null;
	}


	void ThrowItem()
	{
		Vector2 mousePosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, 1 << 8))
		{
			Vector3 targetDirection = hit.point - transform.position;
			targetDirection.z = 0;
			isHolding = false;
			theOneRing.GetComponent<Rigidbody>().isKinematic = false;
			theOneRing.transform.SetParent(null);
			theOneRing.GetComponent<Rigidbody>().AddForce(targetDirection.normalized * throwForce, ForceMode.Impulse);
			theOneRing.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
			theOneRing = null;
		}
	}

	[PunRPC]
	void RPC_ThrowItem()
	{
		var pv = PhotonView.Get(theOneRing);
		var temp = PhotonView.Find(pv.ViewID).gameObject;
		isHolding = false;
		temp.GetComponent<Rigidbody>().isKinematic = false;
		temp.transform.SetParent(null);
		temp.GetComponent<PhotonView>().Synchronization = ViewSynchronization.UnreliableOnChange;
		theOneRing = null;
		
	}

	private void OnTriggerStay(Collider other)
	{
		if(theOneRing == null && other.CompareTag("PickableItem"))
		{
			theOneRing = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(!isHolding && other.CompareTag("PickableItem"))
		{
			theOneRing = null;
		}
	}

	public void callselfCheck(int damage)
	{
		photonView.RPC("checkUnderAttack", RpcTarget.Others, photonView.ViewID, damage);
	}

	[PunRPC]
	public void checkUnderAttack(int victimID, int damage)
	{
		if(victimID == photonView.ViewID)
		{
			UnderAttack(damage);
		}
	} 

}
