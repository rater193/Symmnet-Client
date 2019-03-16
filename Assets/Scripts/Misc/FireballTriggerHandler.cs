using mmo.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.misc
{
	public class FireballTriggerHandler : MonoBehaviour
	{
		public float speed = 0f;
		public Vector2 dir = Vector2.zero;
		public float lifetime = 5f;

		public void Update()
		{
			Vector2 newpos = Vector2.MoveTowards(transform.position, (Vector2)transform.position + dir, speed * Time.deltaTime);
			transform.position = new Vector3(newpos.x, newpos.y, transform.position.z);
			lifetime -= Time.deltaTime;
			if(lifetime<=0)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(collision.gameObject.GetComponent<PlayerController>())
			{
				return;
			}
			if(collision.gameObject.GetComponentInChildren<Shake>())
			{
				Destroy(collision.gameObject);
			}
			Destroy(gameObject);
		}
	}
}