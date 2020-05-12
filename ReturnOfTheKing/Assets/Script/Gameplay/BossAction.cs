using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
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

	private bool isAvailable;

	private Boss myBoss;

	// Start is called before the first frame update
	void Start()
    {
		targetDirection = transform.forward;

		myBoss = transform.GetComponent<Boss>();
	}

    // Update is called once per frame
    void Update()
    {
		isAvailable = myBoss.isAvailable;

		ControlBoss();
		ToggleSwitchCD();
	}


	void ControlBoss()
	{
		if (isAvailable)
		{
			Vector2 mousePosition = Input.mousePosition;
			Ray ray = Camera.main.ScreenPointToRay(mousePosition);
			RaycastHit hit;
			Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE, layerMask);

			// rotate boss to mouse position
			if (Input.GetMouseButtonUp(1) && hit.collider.tag == "Ground")
			{
				targetDirection = new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);
			}
			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);

			transform.rotation = Quaternion.LookRotation(newDirection);

			//change to another one
			if(Input.GetMouseButtonUp(0) && hit.collider.tag == "Boss")
			{
				nextBoss = hit.collider.gameObject;
				if (!nextBoss.GetComponent<BossAction>().isSwitched && nextBoss != gameObject)
				{
					myBoss.isAvailable = false;
					isSwitched = true;
					nextBoss.GetComponent<Boss>().isAvailable = true;
				}

			}
		}
		
	}

	void ToggleSwitchCD()
	{
		if (isSwitched)
		{
			switchTimer += Time.deltaTime;
		}

		if(switchTimer > switchCoolDown)
		{
			isSwitched = false;
			switchTimer = 0f;
		}
	}

}
