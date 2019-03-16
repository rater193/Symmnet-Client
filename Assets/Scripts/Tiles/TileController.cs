using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.tile
{
	[ExecuteInEditMode]
	public class TileController : MonoBehaviour
	{

		public bool
			n = false,
			s = false,
			e = false,
			w = false,
			nw = false,
			sw = false,
			ne = false,
			se = false
			;
		//This is the controller for storing the tiles
		public static bool generatingTiles = false;
		public static Dictionary<string, TileController> tileLayer1 = new Dictionary<string, TileController>();
		public Texture2D targetTilesetToUse;

		//This is the tile texture
		private Texture2D tileTexture;
		private Sprite tileSprite;
		

		public string getUniqueID()
		{
			Vector3 newPos = getTileCoords(transform.position);
			return Mathf.RoundToInt(newPos.x) + "," + Mathf.RoundToInt(newPos.y) + "," + Mathf.RoundToInt(newPos.z);
		}

		public void InitTile()
		{

			//Fixing line issues?
			transform.localScale = new Vector3(1.001f, 1.001f, 1.001f);

			/*
			if(generatingTiles == true)
			{
				yield return new WaitForFixedUpdate();
			}*/

			//
			//First we get our coordinates
			Vector3 newPos = getTileCoords(transform.position);
			//Debug.Log("newPos: " + newPos);
			Vector3 newSetPos = newPos * 48f / 100f;
			//transform.position = new Vector3(newSetPos.x, newSetPos.y, transform.position.z);

			gameObject.name = "tile_" + newPos;

			string newid = getUniqueID();

			if (tileLayer1.ContainsKey(newid))
			{
				//Debug.Log("Tile already exists! " + gameObject.name);
				tileLayer1.Remove(newid);
				//Destroy(gameObject);
			}
			else
			{
				//Adding the new tile to the list
			}

			tileLayer1.Add(newid, this);

			//yield return new WaitForFixedUpdate();

			int z = Mathf.RoundToInt(newPos.z);

			//e = tileExists(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y));
			/*
			n = isMyTile(Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
			s = isMyTile(Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);
			w = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y), z, targetTilesetToUse);
			e = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y), z, targetTilesetToUse);
			nw = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
			sw = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);
			ne = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
			se = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);
			*/

			//yield return new WaitForFixedUpdate();
			//tileLayer1.Add(getTileID(transform.position.x * 100f, transform.position.y * 100f), this);
			//updateTileLook();

			//End
			//yield return null;
		}

		public void DeleteTile()
		{
			string newid = getUniqueID();

			if (tileLayer1.ContainsKey(newid))
			{
				tileLayer1.Remove(newid);
			}

			Destroy(gameObject);
		}

		public static bool isMyTile(int x, int y, int z, Texture targetTilesetToUse)
		{
			TileController otherTile = getTile(x, y, z);
			return otherTile != null ? (otherTile.targetTilesetToUse == targetTilesetToUse) : false;
		}

		public static TileController getTile(int x, int y, int z)
		{
			//Debug.Log("Checking " + new Vector2(x, y) + " from " + (transform.position * 100f / 48f));
			TileController ret = null;
			tileLayer1.TryGetValue(x + "," + y + "," + z, out ret);
			return ret;
		}

		public static bool tileExists(int x, int y, int z)
		{
			//Debug.Log("Checking " + new Vector2(x, y) + " from " + (transform.position * 100f / 48f));
			return tileLayer1.ContainsKey(x + "," + y + "," + z);
		}

		public static Vector3 getTileCoords(Vector3 pos)
		{
			return new Vector3(Mathf.RoundToInt (pos.x * 100f / 48f), Mathf.RoundToInt(pos.y * 100f / 48f), pos.z);
		}

		public void updateTileLook()
		{
			if (targetTilesetToUse)
			{
				/*
				bool
					n = tileExists((int)(transform.position.x * 100f) + 0, (int)(transform.position.y * 100f) + 48),
					s = tileExists((int)(transform.position.x * 100f) + 0, (int)(transform.position.y * 100f) - 48),
					e = tileExists((int)(transform.position.x * 100f) + 48, (int)(transform.position.y * 100f) + 0),
					w = tileExists((int)(transform.position.x * 100f) - 48, (int)(transform.position.y * 100f) + 0),
					nw = tileExists((int)(transform.position.x * 100f) - 48, (int)(transform.position.y * 100f) + 48),
					sw = tileExists((int)(transform.position.x * 100f) - 48, (int)(transform.position.y * 100f) - 48),
					ne = tileExists((int)(transform.position.x * 100f) + 48, (int)(transform.position.y * 100f) + 48),
					se = tileExists((int)(transform.position.x * 100f) + 48, (int)(transform.position.y * 100f) - 48)
					;*/
				Vector3 newPos = getTileCoords(transform.position);
				int z = Mathf.RoundToInt(newPos.z);
				n = isMyTile(Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
				s = isMyTile(Mathf.FloorToInt(newPos.x), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);
				w = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y), z, targetTilesetToUse);
				e = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y), z, targetTilesetToUse);
				nw = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
				sw = isMyTile(Mathf.FloorToInt(newPos.x - 1), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);
				ne = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y + 1), z, targetTilesetToUse);
				se = isMyTile(Mathf.FloorToInt(newPos.x + 1), Mathf.FloorToInt(newPos.y - 1), z, targetTilesetToUse);

				//NW

				int
					tileCW = 24,
					tileCH = 24;

				Color[] colorsNW = new Color[0];
				if (n || w)
				{
					if (n && !w)
					{
						colorsNW = targetTilesetToUse.GetPixels(0, 144 - (5 * 24), tileCW, tileCH);
					}
					else if (!n && w)
					{
						colorsNW = targetTilesetToUse.GetPixels(2 * 24, 144 - (3 * 24), tileCW, tileCH);
					}
					else if (n && w && !nw)
					{
						colorsNW = targetTilesetToUse.GetPixels(2 * 24, 144 - (1 * 24), tileCW, tileCH);
					}
					else if (n && w && nw)
					{
						colorsNW = targetTilesetToUse.GetPixels(2 * 24, 144 - (5 * 24), tileCW, tileCH);
					}
				}
				else
				{
					colorsNW = targetTilesetToUse.GetPixels(0, 144 - 24, tileCW, tileCH);
				}

				//SW
				Color[] colorsSW = new Color[0];
				if (s || w)
				{
					if (s && !w)
					{
						colorsSW = targetTilesetToUse.GetPixels(0 * 24, 144 - (4 * 24), tileCW, tileCH);
					}
					else if (!s && w)
					{
						colorsSW = targetTilesetToUse.GetPixels(2 * 24, 144 - (6 * 24), tileCW, tileCH);
					}
					else if (s && w && !sw)
					{
						colorsSW = targetTilesetToUse.GetPixels(2 * 24, 144 - (2 * 24), tileCW, tileCH);
					}
					else if (s && w && sw)
					{
						colorsSW = targetTilesetToUse.GetPixels(2 * 24, 144 - (4 * 24), tileCW, tileCH);
					}
				}
				else
				{
					colorsSW = targetTilesetToUse.GetPixels(0 * 24, 144 - (2 * 24), tileCW, tileCH);
				}

				//NE
				Color[] colorsNE = new Color[0];
				if (n || e)
				{
					if (n && !e)
					{
						colorsNE = targetTilesetToUse.GetPixels(3 * 24, 144 - (5 * 24), tileCW, tileCH);
					}
					else if (!n && e)
					{
						colorsNE = targetTilesetToUse.GetPixels(1 * 24, 144 - (3 * 24), tileCW, tileCH);
					}
					else if (n && e && !ne)
					{
						colorsNE = targetTilesetToUse.GetPixels(3 * 24, 144 - (1 * 24), tileCW, tileCH);
					}
					else if (n && e && ne)
					{
						colorsNE = targetTilesetToUse.GetPixels(1 * 24, 144 - (5 * 24), tileCW, tileCH);
					}
				}
				else
				{
					colorsNE = targetTilesetToUse.GetPixels(1 * 24, 144 - (1 * 24), tileCW, tileCH);
				}

				//SE
				Color[] colorsSE = new Color[0];
				if (s || e)
				{
					if (s && !e)
					{
						colorsSE = targetTilesetToUse.GetPixels(3 * 24, 144 - (4 * 24), tileCW, tileCH);
					}
					else if (!s && e)
					{
						colorsSE = targetTilesetToUse.GetPixels(1 * 24, 144 - (6 * 24), tileCW, tileCH);
					}
					else if (s && e && !se)
					{
						colorsSE = targetTilesetToUse.GetPixels(3 * 24, 144 - (2 * 24), tileCW, tileCH);
					}
					else if (s && e && se)
					{
						colorsSE = targetTilesetToUse.GetPixels(1 * 24, 144 - (4 * 24), tileCW, tileCH);
					}
				}
				else
				{
					colorsSE = targetTilesetToUse.GetPixels(1 * 24, 144 - (2 * 24), tileCW, tileCH);
				}

				tileTexture = new Texture2D(48, 48, TextureFormat.ARGB32, true);

				tileTexture.SetPixels(0, 24, tileCW, tileCH, colorsNW);
				tileTexture.SetPixels(0, 0, tileCW, tileCH, colorsSW);
				tileTexture.SetPixels(24, 24, tileCW, tileCH, colorsNE);
				tileTexture.SetPixels(24, 0, tileCW, tileCH, colorsSE);

				tileTexture.mipMapBias = 0f;
				tileTexture.filterMode = FilterMode.Bilinear;
				tileTexture.wrapMode = TextureWrapMode.Clamp;

				tileTexture.Apply();


				tileSprite = Sprite.Create(tileTexture, new Rect(0, 0, 48, 48), Vector2.zero);

				GetComponent<SpriteRenderer>().sprite = tileSprite;
				GetComponent<SpriteRenderer>().material = (Material)Resources.Load("Materials/SpriteDiffuse");
			}
		}

		public static string getTileID(float x, float y)
		{
			string ret = "";

			ret += "" + Mathf.RoundToInt(x);
			ret += "." + Mathf.RoundToInt(y);

			return ret;
		}
	}
}