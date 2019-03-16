using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.dungeon
{
	public class InteractableAnimation : InteractableBase
	{
		public Animator animator;
		public string animationName = "";
		public bool DestroyAfterInteract = false;
		public float DestroyAfterSeconds = 0.5f;

		public override IEnumerator Interact()
		{
			if(animator)
			{

				animator.Play(animationName);
				yield return new WaitForSeconds(DestroyAfterSeconds);
				if(DestroyAfterInteract)
				{
					Destroy(gameObject);
				}
			}
			yield return base.Interact();
		}
	}
}