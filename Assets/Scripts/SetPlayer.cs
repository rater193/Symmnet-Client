using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayer : MonoBehaviour
{
	public GameObject[] parts;
	public GameObject username;

	public void SetInfo(string username, string skinColour, string hairColour, string clothingColour, float xPos, float yPos)
	{
		Color colour1;
		ColorUtility.TryParseHtmlString(skinColour, out colour1);
		Color colour2;
		ColorUtility.TryParseHtmlString(hairColour, out colour2);
		Color colour3;
		ColorUtility.TryParseHtmlString(clothingColour, out colour3);

		for(int i = 0; i < parts.Length; i++)
		{
			Texture2D texture = CopyTexture(parts[i].GetComponent<SpriteRenderer>().sprite.texture, colour1, colour2, colour3);

			SpriteRenderer renderer = parts[i].GetComponent<SpriteRenderer>();
			renderer.sprite = Sprite.Create(texture, renderer.sprite.rect, new Vector2(0.5f, 0.5f));
			renderer.material.mainTexture = texture;
		}

		this.username.GetComponent<Text>().text = username;
		transform.position = new Vector2(xPos, yPos);
	}

	private Texture2D CopyTexture(Texture2D copiedTexture, Color skinColour, Color hairColour, Color shirtColour)
	{
		Texture2D texture = new Texture2D(copiedTexture.width, copiedTexture.height);
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		for(int y = 0; y < texture.height; y++)
		{
			for(int x = 0; x < texture.width; x++)
			{
				if(copiedTexture.GetPixel(x, y).r > 0)
				{
					float value = (1 - copiedTexture.GetPixel(x, y).r) / 2;
					Color newColour = new Color(skinColour.r - value, skinColour.g - value, skinColour.b - value);
					texture.SetPixel(x, y, newColour);
				}

				if(copiedTexture.GetPixel(x, y).g > 0)
				{
					float value = (1 - copiedTexture.GetPixel(x, y).g) / 2;
					Color newColour = new Color(hairColour.r - value, hairColour.g - value, hairColour.b - value);
					texture.SetPixel(x, y, newColour);
				}

				if(copiedTexture.GetPixel(x, y).b > 0)
				{
					float value = (1 - copiedTexture.GetPixel(x, y).b) / 2;
					Color newColour = new Color(shirtColour.r - value, shirtColour.g - value, shirtColour.b - value);
					texture.SetPixel(x, y, newColour);
				}

				if(copiedTexture.GetPixel(x, y) == Color.black || copiedTexture.GetPixel(x, y) == Color.clear)
					texture.SetPixel(x, y, copiedTexture.GetPixel(x, y));
			}
		}

		texture.Apply();
		return texture;
	}
}