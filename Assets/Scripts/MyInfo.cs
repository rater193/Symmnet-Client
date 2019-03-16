using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class MyInfo
{
	public static int playerId;
	public static string username;
	public static bool mapEditor = false;
	public static int authority;
	public static int level;
	public static string skinColour;
	public static string hairColour;
	public static string clothingColour;
	public static float xp;
	public static float health;
	public static float mana;
	public static GameObject player;
	public static List<GameObject> partyList = new List<GameObject>();
	public static byte[] avatar = new byte[0];
}
