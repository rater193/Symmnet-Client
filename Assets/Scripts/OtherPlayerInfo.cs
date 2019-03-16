using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerInfo : MonoBehaviour
{
	public int playerId;
	public string username;
	public int authority;
	public int level;
	public float health;
	public float mana;
	public static byte[] avatar = new byte[0];

	public void SetInfo(int playerId, string username, int authority, int level)
	{
		this.playerId = playerId;
		this.username = username;
		this.authority = authority;
		this.level = level;
	}
}
