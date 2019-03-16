using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Lidgren.Network;
using mmo.tile;
using mmo.dungeon;
using symmnet.client.rater193;

public class DataHandle : MonoBehaviour
{
	internal static float TileMult = 0.32f;

	public void HandleDataBES(NetIncomingMessage data)
	{
		switch(data.ReadByte())
		{
			case 1: //CLIENT ID
				MyInfo.playerId = data.ReadInt32();
				break;
			#region Account data
			case 2: //ACCOUNT
				switch(data.ReadByte())
				{
					case 1: //LOGIN
						MyInfo.username = data.ReadString();
						MyInfo.authority = data.ReadInt32();
						MyInfo.mapEditor = data.ReadBoolean();
						MyInfo.level = data.ReadInt32();
						MyInfo.xp = data.ReadFloat();
						MyInfo.skinColour = data.ReadString();
						MyInfo.hairColour = data.ReadString();
						MyInfo.clothingColour = data.ReadString();
						int size = data.ReadInt32();
						byte[] array = data.ReadBytes(size);
						if(size > 1)
							MyInfo.avatar = array;
						GetComponent<Networking>().ServerConnect(data.ReadInt32());
						break;
				}
				break;
				#endregion
		}
	}

	public void HandleDataServer(NetIncomingMessage data)
	{
		switch (data.ReadByte())
		{
			case 1: //WORLD
				switch (data.ReadByte())
				{
					case 1: //SPAWNING
						switch(data.ReadByte())
						{
							case 1: //SPAWN SELF
								GetComponent<World>().SpawnSelf();
								break;
							case 2: //SPAWN OTHER
								int playerId = data.ReadInt32();
								if(playerId != MyInfo.playerId)
									GetComponent<World>().SpawnOther(playerId, data.ReadString(), data.ReadInt32(), data.ReadInt32(), data.ReadString(), data.ReadString(), data.ReadString(), data.ReadFloat(), data.ReadFloat());
								break;
							case 3: //REMOVE OTHER
								GetComponent<World>().RemovePlayer(data.ReadInt32());
								break;
						}
						break;
					case 2: //PLAYER
						switch(data.ReadByte())
						{
							case 1: //POSITION
								GetComponent<World>().SetPlayerPosition(data.ReadInt32(), data.ReadBoolean(), data.ReadFloat(), data.ReadFloat());
								break;
							case 2: //LOOK
								GetComponent<World>().SetPlayerLook(data.ReadInt32(), data.ReadFloat(), data.ReadBoolean());
								break;
						}
						break;
					case 9: //CHAT
						GetComponent<World>().ChatMessage(data.ReadByte(), data.ReadInt32(), data.ReadString(), data.ReadString());
						break;
					case (byte)255:
						Rater193.handleData(data);
						break;
				}
				break;
		}
	}
}