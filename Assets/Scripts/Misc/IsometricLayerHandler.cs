using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace mmo.misc
{
	//This makes objects overlay eachother with the object further down the screen being on top
	[ExecuteInEditMode()]
	public class IsometricLayerHandler : MonoBehaviour
	{
		public float offset = 0f;

		/// <summary>
		/// If this is ticked to true, it will remove this component after the first update
		/// </summary>
		public bool onlyUpdateOnce = false;

		// Update is called once per frame
#if UNITY_EDITOR
		void Update()
#else
		void FixedUpdate()
#endif
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.y - 1000f - offset);

			//This is to prevent errors in the editor when the scene updates
#if UNITY_EDITOR
			if (EditorApplication.isPlaying)
			{
				if (onlyUpdateOnce)
				{
					Destroy(this);
				}
			}
#else
			if (onlyUpdateOnce)
			{
				Destroy(this);
			}
#endif
		}
	}
}