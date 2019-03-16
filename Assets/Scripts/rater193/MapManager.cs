using mmo.tile;
using symmnet.client.rater193;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.rater193
{

	class MapManager
	{
		public static Dictionary<string, TileController> tiles = new Dictionary<string, TileController>();
		public static Dictionary<string, GameObject> tileEntities = new Dictionary<string, GameObject>();

		public static string GetTileKey(int x, int y, int z)
		{
			return x + "," + y + "," + z;
		}


		public static TileController GetTile(int x, int y, int z)
		{
			TileController ret = null;
			tiles.TryGetValue(GetTileKey(x, y, z), out ret);
			return ret;
		}

		public static bool TileExists(int x, int y, int z)
		{
			return tiles.ContainsKey(GetTileKey(x, y, z));
		}


		public static bool TileEntityExists(int x, int y)
		{
			return tileEntities.ContainsKey(GetTileKey(x, y, -1));
		}

		public static void DeleteTile(int x, int y, int z)
		{
			if(TileExists(x, y, z))
			{
				TileController t = GetTile(x, y, z);
				GameObject.Destroy(t.gameObject);
				tiles.Remove(GetTileKey(x, y, z));
			}
		}

		public static void UpdateTile(int x, int y, int z)
		{
			TileController t = GetTile(x, y, z);
			if (t != null)
			{
				t.updateTileLook();
			}
		}

		public static TileController CreateTile(int x, int y, int z, Texture2D tileset)
		{
			if(TileExists(x, y, z))
			{
				DeleteTile(x, y, z);
			}

			GameObject newObj = new GameObject();
			//newObj.transform.parent = storage.transform;
			var gridSize = 0.48f;
			newObj.transform.position = new Vector3(gridSize * x, gridSize * y, -z);
			newObj.AddComponent<SpriteRenderer>();
			newObj.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Sprite_Tiles");
			newObj.transform.parent = Rater193.tileStorage.transform;
			TileController tile = newObj.AddComponent<TileController>();
			tile.targetTilesetToUse = tileset;

			tile.InitTile();

			tiles.Add(GetTileKey(x, y, z), tile);
			return tile;
		}

		public static void UpdateSuroundingTiles(int x, int y, int layer)
		{

			MapManager.UpdateTile(x - 1, y - 1, layer);
			MapManager.UpdateTile(x - 1, y, layer);
			MapManager.UpdateTile(x - 1, y + 1, layer);

			MapManager.UpdateTile(x + 1, y - 1, layer);
			MapManager.UpdateTile(x + 1, y, layer);
			MapManager.UpdateTile(x + 1, y + 1, layer);

			MapManager.UpdateTile(x, y - 1, layer);
			MapManager.UpdateTile(x, y + 1, layer);
		}

		public static void CreateTileEntity(int x, int y, int renderID)
		{
			GameObject oldTE = GetTileEntityRenderer(x, y);
			if (oldTE) GameObject.Destroy(oldTE.gameObject);
			//Debug.Log("NetworkSettings.instance.getTileentityPrefab(renderID): " + NetworkSettings.instance);
			GameObject newTile = GameObject.Instantiate<GameObject>(
				NetworkSettings.instance.getTileentityPrefab(renderID),
				new Vector3((((float)x) * 0.48f) + 0.24f, (((float)y) * 0.48f) + 0.24f, -1f),
				Quaternion.identity);
			SetTileEntity(x, y, newTile);
		}

		public static void SetTileEntity(int x, int y, GameObject tileEntity)
		{
			string tileID = GetTileKey(x, y, -1);
			if (tileEntities.ContainsKey(tileID))
			{
				tileEntities.Remove(tileID);
			}
			tileEntities.Add(tileID, tileEntity);
		}

		public static void DeleteTileEntity(int x, int y)
		{
			string tileID = GetTileKey(x, y, -1);
			if (tileEntities.ContainsKey(tileID))
			{
				GameObject tileRenderer;
				tileEntities.TryGetValue(tileID, out tileRenderer);
				if (tileRenderer) GameObject.Destroy(tileRenderer.gameObject);

				tileEntities.Remove(tileID);
			}
		}

		public static GameObject GetTileEntityRenderer(int x, int y)
		{
			string tileID = GetTileKey(x, y, -1);
			if (tileEntities.ContainsKey(tileID))
			{
				GameObject te = null;
				tileEntities.TryGetValue(tileID, out te);
				return te;
			}
			return null;
		}
	}
}
