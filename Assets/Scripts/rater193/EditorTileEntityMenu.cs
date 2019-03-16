using symmnet.client.rater193;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorTileEntityMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rater193.onReceivedTileEntities = OnReceiveTileEntities;
	}
	
	// Update is called once per frame
	void OnReceiveTileEntities () {
		Debug.Log("Received tile entities!");
		foreach(int key in Rater193.receivedEditorTileEntities.Keys)
		{
			int renderid = 0;
			Rater193.receivedEditorTileEntities.TryGetValue(key, out renderid);

			//Sprite s = spriterenderer ? spriterenderer.sprite : Sprite.Create(tex, new Rect(0, 96, 48, 48), Vector2.zero);
			
			GameObject newBtn = Instantiate<GameObject>(
				Resources.Load<GameObject>("Prefabs/GUI/TileEntityBtn"), transform);
			GameObject tileEntity = NetworkSettings.instance.getTileentityPrefab(renderid);

			newBtn.GetComponent<Image>().sprite = tileEntity.GetComponentInChildren<SpriteRenderer>() ?
				tileEntity.GetComponentInChildren<SpriteRenderer>().sprite : null;

			newBtn.GetComponent<TileButton>().targetTextureID = key;
		}
	}
}
