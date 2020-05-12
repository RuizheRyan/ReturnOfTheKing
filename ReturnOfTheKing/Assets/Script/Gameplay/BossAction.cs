using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{
	[Header("Attributes")]
	[SerializeField] private float rotateSpeed = 10f;
	[Header("Do not Change")]
	[SerializeField] private LayerMask layerMask;

	private const float MAX_RAY_DISTANCE = 100f;
	private Vector3 targetDirection;

	// Start is called before the first frame update
	void Start()
    {
		targetDirection = transform.forward;
	}

    // Update is called once per frame
    void Update()
    {
		BossRotateToMousPosition();
	}


	void BossRotateToMousPosition()
	{
		Vector2 mousePosition = Input.mousePosition;
		Ray ray = Camera.main.ScreenPointToRay(mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonUp(1) && Physics.Raycast(ray, out hit, MAX_RAY_DISTANCE, layerMask))
		{
			targetDirection = new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z);
		}
		Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0.0f);
		//Debug.Log(targetDirection - transform.forward);

		transform.rotation = Quaternion.LookRotation(newDirection);
	}
}
