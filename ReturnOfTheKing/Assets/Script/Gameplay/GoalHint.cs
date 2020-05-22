using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHint : MonoBehaviour
{
    bool itemIsNear;
    [SerializeField]
    float speed = 5;
    Material mat;
    [SerializeField]
    GameManager.ItemType targetItemType;
    float distance;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (itemIsNear)
        {
            mat.SetFloat("_Alpha", Mathf.Sin(Time.time * speed * (1.5f - distance / 5)) * 0.25f + 0.75f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!itemIsNear && other.CompareTag("PickableItem") && other.GetComponent<PickableItem>().thisType == targetItemType)
        {
            distance = (other.transform.position - transform.position).magnitude;
            itemIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PickableItem"))
        {
            itemIsNear = false;
        }
    }
}
