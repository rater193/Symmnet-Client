using mmo.tile;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.dungeon
{
	public class Generator : MonoBehaviour
	{
		public Texture2D tilesetWall;
		public Texture2D tilesetFloor;
		public Texture2D tilesetVoid;

		public GameObject[] props;

		public int
			mapWidth = 20,
			mapHeight = 20,
			maxGeneratedTilesPerFrame = 25,
			borderSize = 1,
			generatedProps = 10
			;

		private int genedTiles = 0;

		private GameObject storage;

		// Use this for initialization
		IEnumerator Start()
		{
			List<TileController> allowedTiles = new List<TileController>();

			storage = new GameObject();
			storage.name = "DungeonStorage";

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			sw.Start();

			TileController.generatingTiles = true;

			Debug.Log("Placing tiles");
			for (var x = 0; x < mapWidth; x++)
			{
				for (var y = 0; y < mapHeight; y++)
				{
					Texture2D targetTile = tilesetFloor;

					bool add = true;
					if (x < borderSize || x >= mapWidth - borderSize || y < borderSize || y >= mapHeight - borderSize)
					{
						targetTile = tilesetVoid;
						add = false;
					}

					TileController tile = createTile(x, y, 0, targetTile);

					if(add)
					{
						allowedTiles.Add(tile);
					}

					genedTiles++;
					if (genedTiles >= maxGeneratedTilesPerFrame)
					{
						genedTiles = 0;
						yield return new WaitForEndOfFrame();
					}


				}
			}

			//Generating props
			Debug.Log("Generating map props");
			while(generatedProps>0)
			{
				int targetIndex = Random.Range(0, props.Length - 1);
				GameObject prop = Instantiate(props[targetIndex]);

				int targetTileIndex = Random.Range(0, allowedTiles.Count - 1);
				TileController targetTile = allowedTiles.ToArray()[targetTileIndex];

				prop.transform.position = new Vector3(targetTile.transform.position.x + 0.24f, targetTile.transform.position.y + 0.24f, -1);

				generatedProps--;
			}

			//Updating looks
			Debug.Log("Updating tile visuals");
			for (var x = 0; x < mapWidth; x++)
			{
				for (var y = 0; y < mapHeight; y++)
				{
					updateTile(x, y, 0);

					genedTiles++;
					if (genedTiles >= maxGeneratedTilesPerFrame)
					{
						genedTiles = 0;
						yield return new WaitForEndOfFrame();
					}


				}
			}

			sw.Stop();

			Debug.Log("Done generating dungeon! Generated in " + (sw.ElapsedMilliseconds/1000f) + " seconds!");
			TileController.generatingTiles = false;
			yield return null;
		}

		public static void updateTile(int x, int y, int z)
		{
			if(TileController.tileExists(x, y, z))
			{
				TileController t = TileController.getTile(x, y, z);

				t.updateTileLook();
			}
		}

		public static TileController createTile(int x, int y, int z, Texture2D tileset)
		{
			GameObject newObj = new GameObject();
			//newObj.transform.parent = storage.transform;
			var gridSize = 0.48f;
			newObj.transform.position = new Vector3(gridSize * x, gridSize * y, gridSize * z);
			newObj.AddComponent<SpriteRenderer>();
			newObj.GetComponent<SpriteRenderer>().material = Resources.Load<Material>("Materials/Sprite_Tiles");
			TileController tile = newObj.AddComponent<TileController>();
			tile.targetTilesetToUse = tileset;

			return tile;
		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}