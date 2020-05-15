using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviourPun
{
	[Header("Attributes")]
	[SerializeField] private float rotateSpeed = 10f;
	[SerializeField] private float switchCoolDown = 5f;
	[Header("Do not Change")]
	[SerializeField] private LayerMask layerMask;

	[Header("Debugging")]
	[SerializeField] GameObject nextBoss;
	public bool isSwitched = false;
	[SerializeField] private float switchTimer = 0f;

	private const float MAX_RAY_DISTANCE = 100f;
	private Vector3 targetDirection;

	//private bool isAvailable;
	private GameManager gm;
	private Boss myBoss;

	// Start is called before the first frame update
	void Start()
    {
		gm = GameManager.Instance;
		myBoss = transform.GetComponent<Boss>();
		if (myBoss.isAvailable)
		{
			gm.currentTower = gameObject;
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
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
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
		switchTimer += Time.deltaTime;
		if(switchTimer >= switchCoolDown)
		{
			if(gm.currentTower == null)
			{
				gm.currentTower = gameObject;
			}
			else
			{
				gm.currentTower.GetComponent<Boss>().isAvailable = false;
				GetComponent<Boss>().isAvailable = true;
				gm.currentTower = gameObject;
			}
		}
	}

	private void OnMouseUpAsButton()
	{
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

}
