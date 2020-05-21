using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviourPun
{
	[Header("Attributes")]
	[SerializeField] private float rotateSpeed = 10f;
	public float switchCoolDown = 3f;
	[Header("Do not Change")]
	[SerializeField] private LayerMask layerMask;

	[Header("Debugging")]
	[SerializeField] GameObject nextBoss;
	public bool isSwitched = false;
	public float switchTimer = 0f;
	public float delayTimer;
	public float delay;

	[SerializeField]
	Camera bossCamera;

	private const float MAX_RAY_DISTANCE = 1000f;
	private Vector3 targetDirection;

	//private bool isAvailable;
	private GameManager gm;
	private Boss myBoss;

	// Start is called before the first frame update
	void Start()
    {
		bossCamera = bossCamera == null ? Camera.main : bossCamera;
		gm = GameManager.Instance;
		myBoss = transform.GetComponent<Boss>();
		if (myBoss.isAvailable)
		{
			gm.CurrentTower = gameObject;
		}
		targetDirection = transform.up;
	}

    // Update is called once per frame
    void Update()
    {
		if (base.photonView.IsMine)
		{
			//isAvailable = myBoss.isAvailable;

			ControlBoss();
			//ToggleSwitchCD();
		}
		if (isSwitched)
		{
			delayTimer += Time.deltaTime;
			if(delayTimer >= delay)
			{
				delayTimer = 0;
				isSwitched = false;
				GetComponent<Boss>().isAvailable = true;
			}
		}
	}


	void ControlBoss()
	{
		if(myBoss == null)
		{
			myBoss = GetComponent<Boss>();
		}
		if (myBoss.isAvailable)
		{
			Vector2 mousePosition = Input.mousePosition;
			Ray ray = bossCamera.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE, layerMask);

			// rotate boss to mouse position
			if (Input.GetMouseButtonUp(1) && hit.collider.tag == "Ground")
			{
				targetDirection = new Vector3(hit.point.x - transform.position.x, hit.point.y - transform.position.y, 0);
			}
			transform.up = Vector3.RotateTowards(transform.up, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);

			//change to another one
			//if (Input.GetMouseButton(0) && hit.collider.tag == "Boss")
			//{
			//	nextBoss = hit.collider.gameObject;
			//	//if (!nextBoss.GetComponent<BossAction>().isSwitched && nextBoss != gameObject)
			//	//{
			//	if(nextBoss != gameObject)
			//	{ 
			//		switchTimer += Time.deltaTime;
			//		if(switchTimer >= switchCoolDown)
			//		{
			//			myBoss.isAvailable = false;
			//			//isSwitched = true;
			//			nextBoss.GetComponent<Boss>().isAvailable = true;
			//			switchTimer = 0f;
			//		}


			//	}

			//}
		}
		
	}

	private void OnMouseDrag()
	{
		if (!PhotonNetwork.IsMasterClient || isSwitched || myBoss.isAvailable)
		{
			return;
		}
		switchTimer += Time.deltaTime;
		if(switchTimer >= switchCoolDown)
		{
			if(gm.CurrentTower == null)
			{
				isSwitched = true;
				gm.CurrentTower = gameObject;
			}
			else
			{
				gm.CurrentTower.GetComponent<Boss>().isAvailable = false;
				isSwitched = true;
				gm.CurrentTower = gameObject;
			}
			switchTimer = 0;
		}
	}

	private void OnMouseUpAsButton()
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			return;
		}
		switchTimer = 0;
	}
	//void ToggleSwitchCD()
	//{
	//	if (isSwitched)
	//	{
	//		switchTimer += Time.deltaTime;
	//	}

	//	if(switchTimer > switchCoolDown)
	//	{
	//		isSwitched = false;
	//		switchTimer = 0f;
	//	}
	//}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("PickableItem"))
		{
			myBoss.isAvailable = false;
			isSwitched = true;
		}
	}

}
