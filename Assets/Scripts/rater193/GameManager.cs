using Assets.Scripts.rater193;
using symmnet.client.rater193;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance;
		// Use this for initialization
		IEnumerator Start()
		{
			yield return new WaitForSeconds(5);
			
			yield return null;
		}

		public void SetLayerToUse(int layer)
		{
			Rater193.tileLayer = layer;
		}
		
	}
}