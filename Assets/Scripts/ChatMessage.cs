using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessage : MonoBehaviour
{
	private RectTransform rect;
	public GameObject prefix;
	public Text prefixText;
	public GameObject message;
	public Text messageText;
	public float height = 0;
	public float width = 0;

	public void SetMessage(string prefix, string message, Color prefixCol, Color messageCol)
	{
		prefixText.text = prefix;
		messageText.text = message;
		prefixText.color = prefixCol;
		messageText.color = messageCol;

		RectTransform prefixRect = this.prefix.GetComponent<RectTransform>();
		RectTransform messageRect = this.message.GetComponent<RectTransform>();

		ContentSizeFitter content = this.prefix.GetComponent<ContentSizeFitter>();
		content.enabled = false;
		content.SetLayoutHorizontal();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.prefix.transform);
		content.enabled = true;

		content = this.message.GetComponent<ContentSizeFitter>();
		content.enabled = false;
		content.SetLayoutHorizontal();
		content.SetLayoutVertical();
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.message.transform);
		content.enabled = true;

		messageRect.anchoredPosition = new Vector2(prefixRect.rect.width, 0);

		RectTransform rect = gameObject.AddComponent<RectTransform>();
		rect.sizeDelta = new Vector2(prefixRect.rect.width + messageRect.rect.width, messageRect.rect.height);
		height = rect.sizeDelta.y;
		width = prefixRect.rect.width / 2;
	}
}
