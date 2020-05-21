using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPage : MonoBehaviour
{
	[SerializeField] private GameObject hiderInfo;
	[SerializeField] private GameObject seekerInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void ShowSeeker()
	{
		seekerInfo.SetActive(true);
	}
	public void HideSeeker()
	{
		seekerInfo.SetActive(false);
	}

	public void ShowHider()
	{
		hiderInfo.SetActive(true);
	}
	public void HideHider()
	{
		hiderInfo.SetActive(false);
	}

}
