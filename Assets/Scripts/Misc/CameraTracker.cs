using mmo.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.misc
{
	public class CameraTracker : MonoBehaviour
	{
		public static CameraTracker previouslyCreatedInstance = null;
		public GameObject targetToFollow;
		// Use this for initialization
		void Start()
		{
			previouslyCreatedInstance = this;
		}

		// Update is called once per frame
		void Update()
		{
			if(targetToFollow)
			{
				Vector2 pos1 = transform.position;
				Vector2 pos2 = (Vector2)targetToFollow.transform.position;

				Vector2 diff = pos1 + ((pos2 - pos1) * 2f * Time.deltaTime);
				transform.position = new Vector3(diff.x, diff.y, transform.position.z);

				//transform.Translate((pos2 - pos1) * 4f * Time.deltaTime);
				//Comment this line out if u want it to be smoother(may cause potential issues rendering lines between tiles)
				//transform.position = new Vector3(Mathf.Round(transform.position.x * 100f) / 100f, Mathf.Round(transform.position.y * 100f) / 100f, transform.position.z);
			}
		}
	}
}