using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.misc
{
	public class FireballAnimator : MonoBehaviour
	{
		public float time = 0;
		// Use this for initialization
		void Start()
		{
			time = Random.Range(0, 100f);
			transform.localPosition = (new Vector2(transform.localPosition.x, -Mathf.Abs(Mathf.Sin(time * 10f)) - (Mathf.PI / 2f)) / 4f * -1f) - new Vector2(0f, 0.6f);
		}

		// Update is called once per frame
		void Update()
		{
			time += Time.deltaTime;
			transform.localPosition = (new Vector2(transform.localPosition.x, -Mathf.Abs(Mathf.Sin(time * 10f)) - (Mathf.PI / 2f))/4f * -1f) - new Vector2(0f,0.6f);
			transform.Rotate(new Vector3(0, 0, -Time.deltaTime * 360f));
		}
	}
}