using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPage : MonoBehaviour
{
	[SerializeField] private GameObject hiderInfo;
	[SerializeField] private GameObject seekerInfo;
	// Start is called before the first frame update
	private void Awake()
	{
		
	}

	public void ShowSeeker()
	{
		if (!PlayerPrefs.HasKey("FinishSeekerTutorial") || PlayerPrefs.GetInt("FinishSeekerTutorial") == 0)
		{
			PlayerPrefs.SetInt("FinishSeekerTutorial", 0);
			seekerInfo.SetActive(true);
		}		
	}
	public void HideSeeker()
	{
		seekerInfo.SetActive(false);
		PlayerPrefs.SetInt("FinishSeekerTutorial", 1);
	}

	public void ShowHider()
	{
		if (!PlayerPrefs.HasKey("FinishHiderTutorial") || PlayerPrefs.GetInt("FinishHiderTutorial") == 0)
		{
			PlayerPrefs.SetInt("FinishHiderTutorial", 0);
			hiderInfo.SetActive(true);
		}		
	}
	public void HideHider()
	{
		hiderInfo.SetActive(false);
		PlayerPrefs.SetInt("FinishHiderTutorial", 1);
	}

}
