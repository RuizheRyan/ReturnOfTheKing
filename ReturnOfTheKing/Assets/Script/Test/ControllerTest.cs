﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviourPun
{
	[Header("Attributes")]
	[SerializeField] private float moveSpeed = 4f;
	[SerializeField] private float coolDownTime = 10f;

	[Header("Debugging")]
	public bool hasThrown;
	[SerializeField] private float timer = 0f;
	//[SerializeField] private Collider PickableItemACollider;
	//[SerializeField] private Collider PickableItemBCollider;
	public bool isPicking = false;

	Vector3 forward, right;

    // Start is called before the first frame update
    void Start()
    {
		CoordinationSetting();

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
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
			{
				Move();
			}

			ToggleCoolDown();
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
		Vector3 rightMovement = right * moveSpeed * Time.deltaTime * Input.GetAxis("HorizontalKey");
		Vector3 upMovement = forward * moveSpeed * Time.deltaTime * Input.GetAxis("VerticalKey");

		Vector3 heading = Vector3.Normalize(rightMovement + upMovement);

		transform.forward = heading;

		transform.position += heading * moveSpeed * Time.deltaTime;

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
		}
	}
}
