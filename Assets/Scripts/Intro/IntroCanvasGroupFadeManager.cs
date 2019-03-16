using mmo.misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace mmo.intro
{
	[RequireComponent(typeof(CanvasGroup))]
	public class IntroCanvasGroupFadeManager : MonoBehaviour
	{
		private CanvasGroup cg;

		// Use this for initialization
		IEnumerator Start()
		{
			//First we must hide our canvasgroup from being seen
			cg = GetComponent<CanvasGroup>();
			cg.alpha = 0;
			
			//Then we start the fading process
			//This is our time we have waited, or at least thats what we will use it for :P
			float time = 0;

			//Fading in
			while(time<1f)
			{
				time += Time.fixedDeltaTime;
				if(time>1f)
				{
					time = 1f;
				}
				cg.alpha = time;
				yield return new WaitForFixedUpdate();
			}

			//Waiting a few seconds
			yield return new WaitForSeconds(3);

			//Fading out
			while (time > 0f)
			{
				time -= Time.fixedDeltaTime;
				if (time < 0f)
				{
					time = 0f;
				}
				cg.alpha = time;
				yield return new WaitForFixedUpdate();
			}

			//Loading the next scene
			Utilities.NextScene();

			//Finalize
			yield return null;
		}
	}
}