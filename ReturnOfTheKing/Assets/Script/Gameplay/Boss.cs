using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
	[SerializeField] private float rotateSpeed = 10f;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private bool isAvailable;

	private Vector3 targetDirection;
	private const float MAX_RAY_DISTANCE = 100f;
	private bool isHit;
	// Start is called before the first frame update
	void Start()
    {
		targetDirection = transform.forward;
        
    }

    // Update is called once per frame
    void Update()
    {
		Debug.DrawRay(transform.position, transform.forward * 10f, Color.red);

		BossRotate();
    }

	void BossRotate()
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
