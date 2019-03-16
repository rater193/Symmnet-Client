using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo
{
	public class TileSelector : MonoBehaviour
	{

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			Camera c = Camera.main;
			if (c)
			{
				Vector3 worldPos = c.ScreenToWorldPoint(Input.mousePosition) * 100f;
				transform.position = new Vector3(
					((Mathf.Floor(worldPos.x/48f)*48f)/100f) + 0.24f ,
					((Mathf.Floor(worldPos.y/48f)*48f)/100f) + 0.24f ,
					transform.position.z);
			}
		}

	}
}