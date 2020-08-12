using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviourPun, IPunObservable
{
	#region IPunObservable implementation
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(isAvailable);
		}
		else
		{
			// Network player, receive data
			this.isAvailable = (bool)stream.ReceiveNext();
		}
	}
	#endregion

	[Header("Attributes")]
	[SerializeField] private float HitCoolDown = 5f;
	[SerializeField] private int damage = 1;
	[Header("Integer Angle & Times by 5")]
	public int detectingRange = 60;
	[SerializeField] private int numberOfRays;
	


	[Header("Do not change")]
	[SerializeField] private LayerMask layerMask;
	public bool isAvailable;
	[SerializeField] private float timer = 0f;


	private const float MAX_RAY_DISTANCE = 100f;
	private bool isHit = false;
	private RaycastHit hitsInfo;

	public CharacterController firstPlayer;
	public CharacterController secondPlayer;

	List<GameObject> hitPlayers = new List<GameObject>();
	// Start is called before the first frame update
	void Start()
    {


		//numberOfRays = Mathf.FloorToInt(detectingRange / 5) + 1;
		//hitsInfo = new RaycastHit[numberOfRays];

	}

    // Update is called once per frame
    void Update()
    {
		//Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);
		ToggleCoolDown();
    }

	private void FixedUpdate()
	{
		if (PhotonNetwork.IsMasterClient)
		{
			BossDetecting();
		}
	}

	void BossDetecting()
	{
		if (!isHit & isAvailable)
		{
			Vector3 origin = transform.position;
			origin.z += 1;
			Vector3 startDirection = Quaternion.AngleAxis(-detectingRange / 2, -Vector3.forward) * transform.up;
			float deltaAngle = (float)detectingRange / ((float)numberOfRays - 1f);
			//Debug.Log(startDirection);

			int rayMissNum = 0;
			foreach(GameObject item in hitPlayers)
			{
				item.GetComponent<CharacterController>().IsDetected = false;
			}
			hitPlayers.Clear();
			for (int i = 0; i < numberOfRays; i++)
			{
				Vector3 rayDirection = Quaternion.AngleAxis(i * deltaAngle, -Vector3.forward) * startDirection;
				Ray ray = new Ray(origin, rayDirection);
				if(Physics.Raycast(ray, out hitsInfo, MAX_RAY_DISTANCE, layerMask))
				{
					//Debug.Log(i + " yes");
					Debug.DrawRay(ray.origin, hitsInfo.point, Color.yellow);
					if(hitsInfo.transform.CompareTag("Player"))
					{
						//hitsInfo.collider.transform.GetComponent<CharacterController>().callselfCheck(damage);
						if(hitPlayers.Count == 0)
						{
							hitPlayers.Add(hitsInfo.transform.gameObject);
						}
						//foreach(GameObject item in hitPlayers)
						//{
						//	if(item == hitsInfo.transform.gameObject)
						//	{
						//		continue;
						//	}
						//	hitPlayers.Add(hitsInfo.transform.gameObject);
						//}
						if (!hitPlayers.Contains(hitsInfo.transform.gameObject))
						{
							hitPlayers.Add(hitsInfo.transform.gameObject);
						}
					}
					else
					{
						rayMissNum++;
					}
					//if(hitsInfo[i].collider.tag == "Obstacle")
					//{
					//	Debug.Log(i+ " Obstacle");
					//}
				}
				else
				{
					rayMissNum++;
					Debug.DrawRay(ray.origin, ray.origin + ray.direction.normalized * 100, Color.yellow);
				}
			}
			foreach(GameObject item in hitPlayers)
			{
				item.GetComponent<CharacterController>().IsDetected = true;
			}
			//if(rayMissNum >= numberOfRays)
			//{
			//	hitPlayers.Clear();
			//}
			//else
			//{
			//}
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		//if(other.tag == "PickableItem")
		//{
		//	if (!other.gameObject.GetComponent<PickableItem>().GetPickingState())
		//	{
		//		isHit = true;

		//		Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
		//		rb.velocity = Vector3.zero;
		//	}
		//	else
		//	{
		//		other.gameObject.GetComponent<PickableItem>().DropItem();
		//	}
		//}
	}

	void ToggleCoolDown()
	{
		if (isHit)
		{
			timer += Time.deltaTime;
		}

		if (timer >= HitCoolDown)
		{
			isHit = false;
			timer = 0;
		}
	}

	

}
