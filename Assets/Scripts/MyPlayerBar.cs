using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerBar : MonoBehaviour
{
	public Text username;
	public Text level;
	public GameObject avatar;
	public GameObject healthBar;
	public GameObject manaBar;

	private void Start()
	{
		username.text = MyInfo.username;
		level.text = MyInfo.level.ToString();
		if(MyInfo.avatar.Length > 0)
		{
			Texture2D image = new Texture2D(48, 48);
			image.LoadImage(MyInfo.avatar);
			Sprite sprite = Sprite.Create(image, new Rect(0.0f, 0.0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
			avatar.GetComponent<Image>().sprite = sprite;
		}
	}
}
