using mmo.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mmo.dungeon
{
	public class InteractableMenuSelector : InteractableBase
	{
		public CanvasGroup targetToToggle;
		public Button targetButtonToSelectByDefault;
		public override IEnumerator Interact()
		{
			if(targetToToggle)
			{
				PlayerMenuController.instance.selectMenu(targetToToggle);

				if(targetButtonToSelectByDefault)
				targetButtonToSelectByDefault.Select();
			}
			yield return base.Interact();
		}
	}
}