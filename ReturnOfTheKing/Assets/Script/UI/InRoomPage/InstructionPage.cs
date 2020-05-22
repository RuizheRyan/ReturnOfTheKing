using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPage : MonoBehaviour
{
	[SerializeField] private GameObject hiderInfo;
	[SerializeField] private GameObject seekerInfoP1;
	[SerializeField] private GameObject seekerInfoP2;
	// Start is called before the first frame update
	private void Awake()
	{
		
	}

	public void ShowSeeker()
	{
		if (!PlayerPrefs.HasKey("FinishSeekerTutorial") || PlayerPrefs.GetInt("FinishSeekerTutorial") == 0)
		{
			PlayerPrefs.SetInt("FinishSeekerTutorial", 0);
			seekerInfoP1.SetActive(true);
		}
		return;
	}

	public void ShowSeekerPage2()
	{
		seekerInfoP1.SetActive(false);
		seekerInfoP2.SetActive(true);
		return;
	}
	public void HideSeeker()
	{
		seekerInfoP2.SetActive(false);
		PlayerPrefs.SetInt("FinishSeekerTutorial", 1);
		return;
	}

	public void ShowHider()
	{
		if (!PlayerPrefs.HasKey("FinishHiderTutorial") || PlayerPrefs.GetInt("FinishHiderTutorial") == 0)
		{
			PlayerPrefs.SetInt("FinishHiderTutorial", 0);
			hiderInfo.SetActive(true);
		}
		return;
	}

	public void HideHider()
	{
		hiderInfo.SetActive(false);
		PlayerPrefs.SetInt("FinishHiderTutorial", 1);
	}

}
