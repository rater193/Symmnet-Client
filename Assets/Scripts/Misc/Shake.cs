using mmo.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.misc
{

	public class Shake : MonoBehaviour
	{
		public float intensity = 0.5f;
		public float distance = 1f;
		public float minScale = 1f;
		public float maxScale = 1.5f;
		public float aggroRange = 2f;
		public Color passiveColor = Color.white;
		public Color aggroColor = Color.red;
		public float targetSpeed = 1f;
		public float volume = 0.5f;


		// Update is called once per frame
		void FixedUpdate()
		{
			PlayerController player = PlayerController.localPlayerController;

			float power = Vector2.Distance(transform.parent.position, player.transform.position);

			if(power > distance)
			{
				power = distance;
			}

			float mult = (distance - power) / distance;

			transform.localPosition = new Vector2(
				Random.Range(-intensity * 100f, intensity * 100f) / 100f * mult,
				Random.Range(-intensity * 100f, intensity * 100f) / 100f * mult
				);

			float scaleDiff = maxScale - minScale;
			float scaleAdd = scaleDiff * mult;
			float tarScale = minScale + scaleAdd;

			transform.localScale = new Vector2(tarScale, tarScale);

			if (Vector2.Distance(transform.parent.position, player.transform.position) <= aggroRange) {
				float speedMult = Vector2.Distance(transform.parent.position, player.transform.position) / aggroRange;
				float tarSpeed = targetSpeed * speedMult;
				//Debug.Log("speedMult: " + speedMult);
				GetComponent<AudioSource>().pitch = (speedMult * -1f) + 1f * (Random.Range(150f,200f)/100f);
				GetComponent<AudioSource>().volume = (speedMult * -1f * volume) + volume;
				if(!GetComponent<AudioSource>().isPlaying)
				{
					GetComponent<AudioSource>().Play();
				}

				Vector2 newPos = Vector2.MoveTowards(transform.parent.position, player.transform.position, tarSpeed);

				Vector2 velocity = newPos - (Vector2)transform.parent.position;

				transform.parent.GetComponent<Rigidbody2D>().velocity = velocity;
				//transform.parent.position = Vector2.MoveTowards(transform.parent.position, player.transform.position, tarSpeed);
				GetComponent<SpriteRenderer>().color = aggroColor;
			}
			else
			{
				GetComponent<SpriteRenderer>().color = passiveColor;
				GetComponent<AudioSource>().volume = 0f;
				GetComponent<AudioSource>().Stop();
			}
		}
	}
}