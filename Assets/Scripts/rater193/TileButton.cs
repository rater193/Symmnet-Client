using symmnet.client.rater193;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileButton : MonoBehaviour, IPointerClickHandler
{
	public int targetTextureID = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void OnPointerClick(PointerEventData eventData)
	{
		if (Rater193.tileLayer >= 0)
		{
			Rater193.TargetTile = targetTextureID;
		}
		else
		{
			Rater193.TargetTileEntity = targetTextureID;
		}
	}
}
