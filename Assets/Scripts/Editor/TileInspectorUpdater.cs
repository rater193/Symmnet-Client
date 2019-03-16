using mmo.tile;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace mmo.editor
{
	[CustomEditor(typeof(TileController))]
	[CanEditMultipleObjects()]
	public class TileInspectorUpdater : Editor
	{
		public override void OnInspectorGUI()
		{
			string text = "Update Tile Preview";
			if(targets.Length>1)
			{
				text += "s";
			}
			if (GUILayout.Button(text))
			{
				Debug.Log("size: " + targets.Length);

				//Resetting the dictionary
				TileController.tileLayer1.Clear();

				//Populating the dictionary
				foreach (TileController tile in targets)
				{
					if(TileController.tileLayer1.ContainsKey(tile.getUniqueID())) { Debug.Log("Duplicate found: " + tile.gameObject.name); DestroyImmediate(tile.gameObject); continue; }
					TileController.tileLayer1.Add(tile.getUniqueID(), tile);
				}
				//Updating the preview
				foreach (TileController tile in targets)
				{
					tile.updateTileLook();
					
				}

				//Finally resetting it once more
				TileController.tileLayer1.Clear();
				
				//Applying new values to the editor to save the changes
				foreach (TileController tile in targets)
				{
					EditorUtility.SetDirty(tile);
					EditorUtility.SetDirty(tile.GetComponent<SpriteRenderer>());
					EditorUtility.SetDirty(tile.gameObject);
				}


			}
			base.OnInspectorGUI();
		}
	}
}