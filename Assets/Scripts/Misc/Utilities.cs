using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mmo.misc
{
	/// <summary>
	/// Authors:
	///		rater193
	///		Symmetrik
	///	Ddescription:
	///		This is a bunch of functions that will be used in other locations.
	///		To help keep the code clean, this will be where you can change code
	///		that is repeatedly used.
	/// </summary>
	public class Utilities
	{

		/// <summary>
		/// Loads the next scene in the scene order list
		/// </summary>
		public static void NextScene()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}