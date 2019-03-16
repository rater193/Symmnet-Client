using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
	public int playerId;
	public float health;
	public float mana;
	public Text username;
	public Text level;
	public GameObject avatar;
	public GameObject healthBar;
	public GameObject manaBar;

	public void SetInfo(int playerId, byte[]array, string username, int level, float health, float mana)
	{
		this.playerId = playerId;

		if(array.Length > 0)
		{
			Texture2D image = new Texture2D(40, 40);
			image.LoadImage(array);
			Sprite sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
			avatar.GetComponent<Image>().sprite = sprite;
		}

		this.username.text = username;
		this.level.text = level.ToString();
		this.health = health;
		this.mana = mana;
	}
}
