using DynamicLight2D;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CharacterController : MonoBehaviourPun, IPunObservable
{
	#region IPunObservable implementation
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//if (stream.IsWriting)
		//{
		//	// We own this player: send the others our data

		//	stream.SendNext(isDetected);
		//	stream.SendNext(moveSpeed);
		//	stream.SendNext(currentHealth);
		//}
		//else
		//{
		//	// Network player, receive data
		//	this.isDetected = (bool)stream.ReceiveNext();
		//	this.moveSpeed = (float)stream.ReceiveNext();
		//	this.currentHealth = (float)stream.ReceiveNext();
		//}
	}
	#endregion

	[Header("Attributes")]
	public float fullHealth = 100f;
	[SerializeField] private float rotateSpeed = 4f;
	[SerializeField] private float normalSpeed = 8f;
	[SerializeField] private float slowDownSpeed = 1.3333f;
	[SerializeField] private float coolDownTime = 10f;
	[SerializeField] private float throwForce = 5;
	[SerializeField] private float secondsToFrozen = 3f;
	public float rescueCoolDown = 5;
	public float rescueTimer = 0;
	[HideInInspector]
	public bool rescuable = false;

	[Header("Debugging")]
	public bool hasThrown = false;
	public bool _isDetected = false;
	public bool IsDetected
	{
		get
		{
			return _isDetected;
		}
		set
		{
			_isDetected = value;
			GetComponentInChildren<Renderer>().material.SetFloat("_Alpha", _isDetected ? 1 : 0);
			photonView.RPC("RPC_SetDetectedStatus", RpcTarget.Others, photonView.ViewID, value);
		}
	}
	public float currentHealth;
	[SerializeField] private float timer = 0f;
	public bool isHolding = false;

	//[SerializeField] private Collider PickableItemACollider;
	//[SerializeField] private Collider PickableItemBCollider;

	Vector3 forward, right;
	private float moveSpeed;
	[SerializeField] private bool dead = false;
	[SerializeField]
	public bool Dead
	{
		get
		{
			return dead;
		}
		set
		{
			dead = value;
			if (dead)
			{
				_gameManager.someoneDead();
				_gameManager.deadPlayer = gameObject;
			}
			else
			{
				_gameManager.someoneRelive();
			}
		}
	}

	private GameManager _gameManager;

	[SerializeField]
	Camera mainCam;
	Rigidbody rb;
	GameObject theOneRing;

	[SerializeField]
	Material clientMat;
	[SerializeField]
	Material hostMat;
	// Start is called before the first frame update
	void Start()
	{
		_gameManager = GameManager.instance;
		rb = GetComponent<Rigidbody>();
		mainCam = mainCam == null ? Camera.main : mainCam;
		//CoordinationSetting();
		currentHealth = fullHealth;

		GameObject[] allItems;
		allItems = GameObject.FindGameObjectsWithTag("PickableItem");
		foreach (GameObject item in allItems)
		{
			Physics.IgnoreCollision(item.GetComponent<BoxCollider>(), GetComponent<Collider>());
		}
		if (PhotonNetwork.IsMasterClient)
		{
			GetComponentInChildren<Renderer>().material = hostMat;
		}
		else
		{
			GetComponentInChildren<Renderer>().material = clientMat;
		}
		//Physics.IgnoreCollision(PickableItemACollider, GetComponent<Collider>());
		//Physics.IgnoreCollision(PickableItemBCollider, GetComponent<Collider>());

		foreach (var item in GameObject.FindGameObjectsWithTag("Caster"))
		{
			if (item.GetComponent<PlayerCasterController>().player == null)
			{
				item.GetComponent<PlayerCasterController>().player = gameObject;
				break;
			}
		}
	}

	private void FixedUpdate()
	{
		float detectingRange = 30;
		Vector3 origin = transform.position;
		origin.z += 1;
		Vector3 startDirection = Quaternion.AngleAxis(-detectingRange / 2, -Vector3.forward) * transform.up;
		float deltaAngle = detectingRange / (3 - 1f);
		RaycastHit hitInfo;
		rescuable = false;
		for (int i = 0; i < 3; i++)
		{
			Vector3 rayDirection = Quaternion.AngleAxis(i * deltaAngle, -Vector3.forward) * startDirection;
			Ray ray = new Ray(origin, rayDirection);
			if (Physics.Raycast(ray, out hitInfo, 0.2f, 1 << 11) && hitInfo.transform.GetComponent<CharacterController>().Dead)
			{
				Debug.DrawRay(ray.origin, hitInfo.point, Color.green);
				rescuable = true;
			}
			else
			{
				Debug.DrawRay(ray.origin, ray.origin + ray.direction.normalized * 2, Color.green);
			}
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (_isDetected)
		{
			UnderAttack(10);
		}
		else
		{
			moveSpeed = normalSpeed;
		}
		if (_gameManager.deadPlayer != null && rescuable && Input.GetKey(KeyCode.Space))
		{
			rescueTimer += Time.deltaTime;
			if (rescueTimer >= rescueCoolDown)
			{
				rescueTimer = 0;
				_gameManager.deadPlayer.GetComponent<CharacterController>().currentHealth = 50;
				_gameManager.deadPlayer.GetComponent<CharacterController>().Dead = false;
				_gameManager.deadPlayer = null;
			}
		}
		if (base.photonView.IsMine && !Dead && moveSpeed > 0)
		{

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
		if (currentHealth <= 0 && Dead == false)
		{
			_gameManager.someoneDead();
		}
	}

	void CoordinationSetting()
	{
		forward = mainCam.transform.forward;
		forward.y = 0;
		forward = Vector3.Normalize(forward);
		right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
	}

	void Move()
	{
		//Vector3 direction = new Vector3(Input.GetAxis("HorizontalKey"), 0, Input.GetAxis("VerticalKey"));
		Vector3 rightMovement = mainCam.transform.right * Input.GetAxis("HorizontalKey");
		Vector3 upMovement = mainCam.transform.forward * Input.GetAxis("VerticalKey");
		rightMovement.z = 0;
		upMovement.z = 0;

		Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
		heading = Quaternion.Euler(0, 0, 1) * heading;

		if (Input.GetAxis("HorizontalKey") != 0 || Input.GetAxis("VerticalKey") != 0)
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
		if (timer >= coolDownTime)
		{
			hasThrown = false;
			timer = 0;
		}
	}


	public void UnderAttack(int damage)
	{
		currentHealth -= damage * Time.deltaTime;
		if (moveSpeed > normalSpeed / 2)
		{
			//moveSpeed -= 2 * normalSpeed;
			moveSpeed = normalSpeed / 2;
		}
		else
		{
			moveSpeed -= moveSpeed <= 0 ? 0 : slowDownSpeed * Time.deltaTime;
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
		Ray ray = mainCam.ScreenPointToRay(mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, 1 << 8 | 1 << 12))
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
		if (theOneRing == null && other.CompareTag("PickableItem"))
		{
			theOneRing = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!isHolding && other.CompareTag("PickableItem"))
		{
			theOneRing = null;
		}
	}


	public void callselfCheck(int damage)
	{
		photonView.RPC("RPC_checkUnderAttack", RpcTarget.Others, photonView.ViewID, damage);
	}

	[PunRPC]
	public void RPC_checkUnderAttack(int victimID, int damage)
	{
		if (victimID == photonView.ViewID)
		{
			UnderAttack(damage);
		}
	}

	[PunRPC]
	public void RPC_SetDetectedStatus(int victimID, bool value)
	{
		if (victimID == photonView.ViewID)
		{
			_isDetected = value;
		}
	}

	//public void checkSelfDeadState()
	//{
	//	if (PhotonNetwork.IsMasterClient)
	//	{
	//		Debug.Log("Broadcastdead");
	//		photonView.RPC("RPC_amIDead", RpcTarget.Others, photonView.ViewID);
	//	}
	//}

	//[PunRPC]
	//public void RPC_amIDead(int deadManID)
	//{
	//	if (deadManID == photonView.ViewID && photonView.IsMine)
	//	{
	//		Debug.Log("Idead");
	//		dead = true;
	//		_gameManager.someoneDead();
	//	}
	//}

}
