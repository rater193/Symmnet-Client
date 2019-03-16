using symmnet.client.rater193;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
	public GameObject chatBox;
	public List<GameObject> playerList = new List<GameObject>();

	public void SpawnSelf()
	{
		GameObject player = (GameObject)Instantiate(Resources.Load("Prefabs/Player/Player"));
		MyInfo.player = player;
		player.GetComponent<PlayerControl>().networking = gameObject;
		player.GetComponent<SetPlayer>().SetInfo(MyInfo.username, MyInfo.skinColour, MyInfo.hairColour, MyInfo.clothingColour, 0, 0);

		Rater193.networking = gameObject.GetComponent<Networking>();
		Rater193.Init();
		chatBox = GameObject.Find("Canvas/Chatbox");
		chatBox.GetComponent<ChatboxControl>().mapEditor = GetComponent<Networking>().mapEditor;
	}

	public void SpawnOther(int playerId, string username, int authority, int level, string skinCol, string hairCol, string clothCol, float xPos, float yPos)
	{
		GameObject otherPlayer = (GameObject)Instantiate(Resources.Load("Prefabs/Player/OtherPlayer"));
		otherPlayer.GetComponent<SetPlayer>().SetInfo(username, skinCol, hairCol, clothCol, xPos, yPos);
		otherPlayer.GetComponent<OtherPlayerInfo>().SetInfo(playerId, username, authority, level);
		playerList.Add(otherPlayer);
	}

	public void SetPlayerPosition(int playerId, bool illegalPos, float xPos, float yPos)
	{
		if(MyInfo.playerId == playerId)
		{
			if(illegalPos == true)
				MyInfo.player.GetComponent<PlayerControl>().player.position = new Vector2(xPos, yPos);
		}
		else
		{
			for(int i = 0; i < playerList.Count; i++)
			{
				if(playerList[i].GetComponent<OtherPlayerInfo>().playerId == playerId)
				{
					playerList[i].GetComponent<OtherPlayerControl>().gotoX = xPos;
					playerList[i].GetComponent<OtherPlayerControl>().gotoY = yPos;
				}
			}
		}
	}

	public void SetPlayerLook(int playerId, float angle, bool flip)
	{
		for(int i = 0; i < playerList.Count; i++)
		{
			if(playerList[i].GetComponent<OtherPlayerInfo>().playerId == playerId)
			{
				playerList[i].GetComponent<OtherPlayerControl>().lookAngle = angle;
				playerList[i].GetComponent<OtherPlayerControl>().flip = flip;
			}
		}
	}

	public void RemovePlayer(int playerId)
	{
		for(int i = 0; i < playerList.Count; i++)
		{
			if(playerList[i].GetComponent<OtherPlayerInfo>().playerId == playerId)
			{
				Destroy(playerList[i]);
				playerList.Remove(playerList[i]);
			}
		}
	}

	public void ChatMessage(byte mode, int authority, string username, string message)
	{
		int chatMode = 0;
		switch(mode)
		{
			case 0:
				chatMode = 0;
				break;
			case 1:
				chatMode = 1;
				break;
			case 2:
				chatMode = 2;
				break;
			case 3:
				chatMode = 3;
				break;
			case 4:
				chatMode = 4;
				break;
		}

		chatBox.GetComponent<ChatboxControl>().AddMessage(chatMode, authority, username, message);
	}
}
