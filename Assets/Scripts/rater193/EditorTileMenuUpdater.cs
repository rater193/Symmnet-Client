using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace symmnet.client.rater193.mapeditor
{
	public class EditorTileMenuUpdater : MonoBehaviour
	{

		// Use this for initialization
		IEnumerator Start()
		{
			yield return new WaitForSeconds(1);
			Refresh();
			yield return null;
		}

		public void Refresh()
		{
			#region deleting children
			for (int i = 0; i < transform.childCount; i++)
			{
				GameObject child = transform.GetChild(i).gameObject;
				Destroy(child);
			}
			#endregion
			#region loading tiles
			int[] indexes = Rater193.textures.Keys.ToArray();
			for (int index = 0; index < indexes.Length; index++)
			{
				int textureID = indexes[index];
				Texture2D tex = null;
				Rater193.textures.TryGetValue(textureID, out tex);

				GameObject newBtn = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/TileButton"), transform);
				Image img = newBtn.GetComponentInChildren<Image>();

				//Get ethe sprite renderer sprite renderer if one exists, otherwise create the sprite
				Sprite s = Sprite.Create(tex, new Rect(0, 96, 48, 48), Vector2.zero);

				img.sprite = s;

				img.GetComponent<TileButton>().targetTextureID = textureID;
			}
			#endregion
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}