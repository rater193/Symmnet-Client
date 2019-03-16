using Assets.Scripts.rater193;
using Lidgren.Network;
using mmo.dungeon;
using mmo.misc;
using mmo.tile;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace symmnet.client.rater193
{
	public class Rater193
	{
		enum messages
		{
			//Server to client messages
			ClientLoadMapChunk = (byte)0,
			ClientSetTile = (byte)1,
			ClientReceiveTile = (byte)2,
			ClientDeleteTile = (byte)5,
			ClientDeleteColumn = (byte)6,
			ClientReceiveTileEntityList = (byte)10,
			ClientUnloadChunk = (byte)11,

			//Client to server messages
			ServerGetChunk = (byte)0,
			ServerSetTile = (byte)1,
			ServerDeleteTile = (byte)5,
			ServerDeleteColumn = (byte)6
		}

		public static int TargetTile = 1,
			TargetTileEntity = 100,
			tilePlacePosX = -100000000,
			tilePlacePosY = -100000000
			;

		static float debounceClick = 0;
		public static int
			tileLayer = 1,
			tileSize = 1,
			chunkSize = 16
			;

		public static bool hasInitialized = false;

		public static Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

		public static Dictionary<string, GameObject> chunks = new Dictionary<string, GameObject>();
		public static Dictionary<int, int> receivedEditorTileEntities = new Dictionary<int, int>();
		public static Action onReceivedTileEntities;

		public static Networking networking;

		public static GameObject tileStorage;

		public static void Init()
		{
			//Making sure we only initialize just once!
			if (!hasInitialized)
			{

				tileStorage = new GameObject("Tiles");
				GameObject.DontDestroyOnLoad(tileStorage);

				Texture2D[] _t = Resources.LoadAll<Texture2D>("Tilesets/");

				for (int pos = 0; pos < _t.Length; pos++)
				{
					try
					{
						Texture2D tex = _t[pos];
						textures.Add(int.Parse(tex.name), tex);
					}
					catch (Exception e)
					{

					}
				}

				/*
				//Initializing the textures
				textures.Add(1, Resources.Load<Texture2D>("Tilesets/Grass-SandFloor"));
				textures.Add(2, Resources.Load<Texture2D>("Tilesets/GrassFloor"));
				*/

				//Debounce handler.
				hasInitialized = true;
			}

		}

		public static void RequestChunk(int x, int y)
		{
			string chunkKey = x + ", " + y + "," + 0;
			
			if (!chunks.ContainsKey(chunkKey))
			{
				NetOutgoingMessage message = networking.client.CreateMessage();
				message.Write((byte)2); //WORLD
				message.Write((byte)255); // RATERS MESSAGE HEADER

				message.Write((byte)messages.ServerGetChunk); // GET A CHUNK FROM THE SERVER

				message.Write((int)x);
				message.Write((int)y);
				networking.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 10);
				chunks.Add(chunkKey, new GameObject("chunk-" + chunkKey));

			}



		}

		public static GameObject GetChunk(int x, int y)
		{
			string chunkKey = x + ", " + y + "," + 0;
			GameObject ret;
			chunks.TryGetValue(chunkKey, out ret);

			#region creating the chunk if it doesnt exsist!
			if (!ret)
			{
				ret = new GameObject("chunk-" + chunkKey);
				chunks.Add(chunkKey, ret);
			}
			#endregion

			return ret;
		}

		public static void DeleteChunk(int x, int y)
		{
			GameObject chunk = GetChunk(x, y);

			for(int _x = 0; _x < 16; _x++)
			{
				for (int _y = 0; _y < 16; _y++)
				{
					int worldX = (x * 16) + _x;
					int worldY = (y * 16) + _y;
					MapManager.DeleteTileEntity(worldX, worldY);
					for (int _layer = 0; _layer <= 6; _layer++)
					{
						MapManager.DeleteTile(worldX, worldY, _layer);
					}
				}
			}

			GameObject.Destroy(chunk);

			string chunkKey = x + ", " + y + "," + 0;
			chunks.Remove(chunkKey);
		}

		public static void handleData(NetIncomingMessage data)
		{
			#region Shared switch case vars
			//Shared vars, to prevent errors
			int _x,
				_y,
				_layer,
				_id,
				_size
				;
			TileController tile;
			Texture2D targetTexture;
			#endregion

			#region Handling message data
			//Handling the message ID
			switch (data.ReadByte())
			{
				case (byte)messages.ClientLoadMapChunk:
					#region Reading the buffer
					int chunkX = data.ReadInt32();
					int chunkY = data.ReadInt32();
					int chunkLayer = data.ReadInt32();

					chunkSize = data.ReadByte();
					#endregion
					#region Loading chunks
					//This is a list we will use for later, to handle updating tile looks
					List<TileController> tiles = new List<TileController>();


					//Generating the chunk
					#region Creating the chunk
					Debug.Log("Receiving chunk");
					//Loading the new tiles
					for (_x = 0; _x < chunkSize; _x++)
					{
						for (_y = 0; _y < chunkSize; _y++)
						{
							//creating the tile
							int tileID = data.ReadInt32();
							if (chunkLayer >= 0)
							{
								if (textures.ContainsKey(tileID))
								{
									//Retreiving the texture to use
									targetTexture = null;
									textures.TryGetValue(tileID, out targetTexture);

									//Creating the tile object itself
									tile = MapManager.CreateTile((int)_x + (chunkX * chunkSize), (int)_y + (chunkY * chunkSize), chunkLayer, targetTexture);
									//Adding it to the list for referencing later.
									tiles.Add(tile);
								}
							}
							else
							{
								if (tileID >= 0)
								{
									//Debug.Log("" + tileID + ", " + chunkLayer);
									MapManager.CreateTileEntity((int)_x + (chunkX * chunkSize), (int)_y + (chunkY * chunkSize), tileID);
								}
								else
								{
								}
							}

						}
					}
					#endregion

					#region Updating the tiles bordering the chunk
					//Updating chunk borders
					for (int i = -1; i < chunkSize + 1; i++)
					{
						int
							x0 = (chunkX * chunkSize),
							y0 = (chunkY * chunkSize),

							x1 = i + (chunkX * chunkSize),
							y1 = i + (chunkY * chunkSize),

							x2 = x1 + chunkSize + 2,
							y2 = y1 + chunkSize + 2;


						MapManager.UpdateTile(x1, y0 - 1, chunkLayer);
						MapManager.UpdateTile(x1, y0 + chunkSize, chunkLayer);

						MapManager.UpdateTile(x0 - 1, y1, chunkLayer);
						MapManager.UpdateTile(x0 + chunkSize, y1, chunkLayer);

					}
					#endregion

					#region Updating all the tiles in the chunk
					//Updating the tile visuals
					foreach (TileController t in tiles)
					{
						t.updateTileLook();
					}
					#endregion

					#endregion
					break;
				case (byte)messages.ClientReceiveTileEntityList:
					var _tileEntityCount = data.ReadInt32();
					receivedEditorTileEntities.Clear();
					while(--_tileEntityCount>=0)
					{
						var _teid = data.ReadInt32();
						var _terid = data.ReadInt32();
						receivedEditorTileEntities.Add(_teid, _terid);
					}

					//Invoking the action, so we can have other things listen when this event was received
					if (onReceivedTileEntities != null)
					{
						onReceivedTileEntities();
					}
					break;
				case (byte)messages.ClientReceiveTile:
					#region Reading the buffer
					_size = data.ReadInt32();
					_x = data.ReadInt32();
					_y = data.ReadInt32();
					_layer = data.ReadInt32();
					_id = data.ReadInt32();
					#endregion
					#region Placing a single tile
					targetTexture = null;
					textures.TryGetValue(_id, out targetTexture);


					for (int __x = -_size; __x <= _size; __x++)
					{
						for (int __y = -_size; __y <= _size; __y++)
						{
							if (_layer >= 0)
							{
								tile = MapManager.CreateTile((int)_x + __x, (int)_y + __y, _layer, targetTexture);
								tile.InitTile();
							}
							else
							{
								Debug.Log("Create render id: " + _id);
								MapManager.CreateTileEntity(_x + __x, _y + __y, _id);
							}
						}
					}


					for (int __x = -_size - 1; __x <= _size + 1; __x++)
					{
						for (int __y = -_size - 1; __y <= _size + 1; __y++)
						{
							MapManager.UpdateTile(__x + _x, __y + _y, _layer);
						}
					}
					#endregion
					break;

				case (byte)messages.ClientDeleteTile:
					#region Reading the buffer
					_size = data.ReadInt32();
					_x = data.ReadInt32();
					_y = data.ReadInt32();
					_layer = data.ReadInt32();
					#endregion
					#region Deleting a single tile

					for (int __x = -_size; __x <= _size; __x++)
					{
						for (int __y = -_size; __y <= _size; __y++)
						{
							if (_layer >= 0)
								MapManager.DeleteTile(_x + __x, _y + __y, _layer);
							else
								MapManager.DeleteTileEntity(_x + __x, _y + __y);
						}
					}

					if (_layer >= 0)
					{
						for (int __x = -_size - 1; __x <= _size + 1; __x++)
						{
							for (int __y = -_size - 1; __y <= _size + 1; __y++)
							{
								MapManager.UpdateTile(tilePlacePosX + __x, tilePlacePosY + __y, _layer);
							}
						}
					}
					#endregion
					break;

				case (byte)messages.ClientDeleteColumn:
					#region Reading the buffer
					_size = data.ReadInt32();
					_x = data.ReadInt32();
					_y = data.ReadInt32();
					#endregion
					#region Deleting a single column
					for (_layer = -1; _layer <= 6; _layer++)
					{

						for (int __x = -_size; __x <= _size; __x++)
						{
							for (int __y = -_size; __y <= _size; __y++)
							{
								if (_layer >= 0)
									MapManager.DeleteTile(_x + __x, _y + __y, _layer);
								else
									MapManager.DeleteTileEntity(_x + __x, _y + __y);
							}
						}

						if (_layer >= 0)
						{
							for (int __x = -_size - 1; __x <= _size + 1; __x++)
							{
								for (int __y = -_size - 1; __y <= _size + 1; __y++)
								{
									MapManager.UpdateTile(tilePlacePosX + __x, tilePlacePosY + __y, _layer);
								}
							}
						}
					}
					#endregion
					break;

				case (byte)messages.ClientUnloadChunk:
					#region Reading the buffer
					_x = data.ReadInt32();
					_y = data.ReadInt32();
					#endregion
					Debug.Log("Unload chunk: " + _x + ", " + _y);
					DeleteChunk(_x, _y);
					//
					break;
			}
			#endregion
		}

		internal static void LocalPlayerStart(PlayerControl player)
		{
			//Starts requesting chunks from the server
			player.StartCoroutine(HandleChunks(player));
			CameraTracker.previouslyCreatedInstance.targetToFollow = player.gameObject;
		}

		private static IEnumerator HandleChunks(PlayerControl player)
		{
			#region Handling requesting chunks from the server
			while (true)
			{
				for (int _cx = -1; _cx <= 1; _cx++)
				{
					for (int _cy = -1; _cy <= 1; _cy++)
					{
						int px = Mathf.FloorToInt(((player.transform.position.x * 100f) / 48f) / 16f);
						int py = Mathf.FloorToInt(((player.transform.position.y * 100f) / 48f) / 16f);
						RequestChunk(px + _cx, py + _cy);
					}
				}
				yield return new WaitForSeconds(0.5f);
			}
			#endregion
		}

		internal static void LocalPlayerFixedUpdate(PlayerControl player)
		{

		}

		internal static void LocalPlayerUpdate(PlayerControl player)
		{
			#region Vars used for placing/deleting tiles
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) * 100f;
			Vector3 setPos = new Vector3(
				((Mathf.Floor(worldPos.x / 48f) * 48f) / 100f) + 0.24f,
				((Mathf.Floor(worldPos.y / 48f) * 48f) / 100f) + 0.24f,
				0);

			int posX = (Mathf.FloorToInt((setPos.x * 100f) / 48f)),
				posY = (Mathf.FloorToInt((setPos.y * 100f) / 48f)),
				_layer = tileLayer;
				;

			bool
				leftClick = Input.GetMouseButton(0),
				rightClick = Input.GetMouseButton(1),
				shiftClick = Input.GetKey(KeyCode.LeftShift)
				;
			#endregion

			#region Handling tile size
			tileSize = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? 3 : 0;
			#endregion

			if ((Mathf.RoundToInt(posX * 100f) != Mathf.RoundToInt(tilePlacePosX * 100f) || Mathf.RoundToInt(posY * 100f) != Mathf.RoundToInt(tilePlacePosY * 100f)) && (leftClick || rightClick) && !EventSystem.current.IsPointerOverGameObject())
			{
				#region Setting some vars to make it so we dont place the tile in the same spot twice
				tilePlacePosX = posX;
				tilePlacePosY = posY;
				#endregion

				#region placing tiles
				if (leftClick)
				{
					#region Telling the server to place the tile
					NetOutgoingMessage message = networking.client.CreateMessage();
					message.Write((byte)2); //WORLD
					message.Write((byte)255); // RATERS MESSAGE HEADER

					message.Write((byte)messages.ServerSetTile); // SET A TILE

					message.Write((int)tileSize);//The size of the chunk to place
					message.Write((int)tilePlacePosX);// X
					message.Write((int)tilePlacePosY);// Y
					message.Write((int)tileLayer);// LAYER
					message.Write((int)tileLayer >= 0 ? TargetTile : TargetTileEntity);// TILE, Replace the 100 with the tile entity id


					networking.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
					#endregion
					#region Placing the tile locally on the client
					if (tileLayer >= 0)
					{
						debounceClick = 0.05f;


						Texture2D targetTexture = null;
						textures.TryGetValue(TargetTile, out targetTexture);


						for (int __x = -tileSize; __x <= tileSize; __x++)
						{
							for (int __y = -tileSize; __y <= tileSize; __y++)
							{
								var tile = MapManager.CreateTile((int)tilePlacePosX + __x, (int)tilePlacePosY + __y, tileLayer, targetTexture);

								tile.InitTile();
							}
						}

						for (int __x = -tileSize - 1; __x <= tileSize + 1; __x++)
						{
							for (int __y = -tileSize - 1; __y <= tileSize + 1; __y++)
							{
								MapManager.UpdateTile(__x + tilePlacePosX, __y + tilePlacePosY, _layer);
							}
						}
					}
					#endregion
				}
				#endregion
				#region Deleting tiles
				if (rightClick)
				{
					#region Telling the server to delete the tile/column
					NetOutgoingMessage message = networking.client.CreateMessage();
					message.Write((byte)2); //WORLD
					message.Write((byte)255); // RATERS MESSAGE HEADER

					message.Write((byte)(shiftClick ? messages.ServerDeleteColumn : messages.ServerDeleteTile)); // SET A TILE

					message.Write((int)tileSize);// SIZE
					message.Write((int)tilePlacePosX);// X
					message.Write((int)tilePlacePosY);// Y
					if (!shiftClick)
					{
						message.Write((int)_layer);// LAYER
					}
					#endregion
					#region Deleting the tile/column locally, to give a better feel for the editor 
					networking.client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
					debounceClick = 0.05f;

					int layerStart = (shiftClick) ? -1 : _layer;
					int layerEnd = (shiftClick) ? 6 : _layer;
					for (_layer = shiftClick ? layerStart : _layer; _layer <= layerEnd; _layer++)
					{
						//Deleting tiles first
						for (int __x = -tileSize; __x <= tileSize; __x++)
						{
							for (int __y = -tileSize; __y <= tileSize; __y++)
							{
								if (_layer >= 0)
									MapManager.DeleteTile(tilePlacePosX + __x, tilePlacePosY + __y, _layer);
								else
									MapManager.DeleteTileEntity(tilePlacePosX + __x, tilePlacePosY + __y);
							}
						}

						if (_layer >= 0)
						{
							//Then updating their images
							for (int __x = -tileSize - 1; __x <= tileSize + 1; __x++)
							{
								for (int __y = -tileSize - 1; __y <= tileSize + 1; __y++)
								{
									MapManager.UpdateTile(__x + tilePlacePosX, __y + tilePlacePosY, _layer);
								}
							}
						}
					}
					#endregion
				}
				#endregion
			}
			else
			{
				if(!leftClick && !rightClick)
				{
					tilePlacePosX = -1000000000;
				}
			}
		}
	}
}