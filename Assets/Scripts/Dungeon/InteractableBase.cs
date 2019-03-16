using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.dungeon
{
	public class InteractableBase : MonoBehaviour
	{
		public virtual IEnumerator Interact() { yield return null; }
	}
}