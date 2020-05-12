using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	[Header("Attributes")]
	[SerializeField] private float HitCoolDown = 5f;
	[SerializeField] private int damage = 1;
	[Header("Integer Angle & Times by 5")]
	[SerializeField] private int detectingRange = 60;

	[Header("Do not change")]
	[SerializeField] private LayerMask layerMask;
	public bool isAvailable;
	[SerializeField] private float timer = 0f;


	private const float MAX_RAY_DISTANCE = 1000f;
	private bool isHit = false;
	private int numberOfRays;
	private RaycastHit[] hitsInfo;
	private int numberOfRaysHitPlayer = 0;
	// Start is called before the first frame update
	void Start()
    {


		numberOfRays = Mathf.FloorToInt(detectingRange / 5) + 1;
		hitsInfo = new RaycastHit[numberOfRays];
        
    }

    // Update is called once per frame
    void Update()
    {
		//Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);


		BossDetecting();
		ToggleCoolDown();
    }


	void BossDetecting()
	{
		if (!isHit & isAvailable)
		{
			Vector3 origin = new Vector3(transform.position.x, 1f, transform.position.z);
			Vector3 startDirection = Quaternion.AngleAxis(-detectingRange / 2, Vector3.up) * transform.forward;
			numberOfRaysHitPlayer = 0;
			//Debug.Log(startDirection);
			for (int i = 0; i < numberOfRays; i++)
			{
				Vector3 rayDirection = Quaternion.AngleAxis(i * 5f, Vector3.up) * startDirection;
				Ray ray = new Ray(origin, rayDirection);
				Debug.DrawRay(origin, rayDirection * 100f, Color.yellow);
				if(Physics.Raycast(ray, out hitsInfo[i], MAX_RAY_DISTANCE, layerMask))
				{
					//Debug.Log(i + " yes");
					if(hitsInfo[i].collider.tag == "Player")
					{
						hitsInfo[i].collider.transform.GetComponent<CharacterController>().UnderAttack(damage);
					}
					//if(hitsInfo[i].collider.tag == "Obstacle")
					//{
					//	Debug.Log(i+ " Obstacle");
					//}
					
				}

			}
			
		}
	}


	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "PickableItem")
		{
			if (!other.gameObject.GetComponent<PickableItem>().GetPickingState())
			{
				isHit = true;

				Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
				rb.velocity = Vector3.zero;
			}
			else
			{
				other.gameObject.GetComponent<PickableItem>().DropItem();
			}
		}
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
