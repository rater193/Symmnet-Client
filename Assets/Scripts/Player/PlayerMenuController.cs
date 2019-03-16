using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mmo.player
{
	public class PlayerMenuController : MonoBehaviour
	{
		public static PlayerMenuController instance;

		/// <summary>
		/// Never just set this variable, use selectMenu to set this instead!
		/// </summary>
		public CanvasGroup currentSelectedMenuGroup = null;

		/// <summary>
		/// This is going to be used with the player controller, to tell if
		/// the main menu with the stats is selected. If so, then it will
		/// enable the controls for the player object.
		/// </summary>
		public CanvasGroup defaultMenu = null;

		public void Start()
		{
			instance = this;
		}

		public void selectMenu(CanvasGroup newMenu)
		{
			//Unselecting previous menu, if able to
			if (currentSelectedMenuGroup!=null)
			{
				currentSelectedMenuGroup.interactable = false;
				currentSelectedMenuGroup.blocksRaycasts = false;
				currentSelectedMenuGroup.alpha = 0f;
			}
			
			//Selecting new menu
			currentSelectedMenuGroup = newMenu;
			currentSelectedMenuGroup.interactable = true;
			currentSelectedMenuGroup.blocksRaycasts = true;
			currentSelectedMenuGroup.alpha = 1f;

		}

		public void SelectButton(Button btn)
		{
			btn.Select();
		}

	}
}