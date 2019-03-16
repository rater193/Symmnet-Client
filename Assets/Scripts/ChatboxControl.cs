using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class ChatboxControl : MonoBehaviour
{
	private int maxChatCount = 16;
	public bool mapEditor = false;
	public GameObject networking;
	public GameObject chatMode;
	public GameObject allChat;
	public GameObject globalChat;
	public GameObject clanChat;
	public GameObject partyChat;
	public GameObject privateChat;
	public InputField input;
	private List<object[]> allChatList = new List<object[]>();

	private void Start()
	{
		networking = GameObject.Find("Networking");
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			if(input.text != "" && input.text.Length <= 128)
			{
				NetOutgoingMessage message = networking.GetComponent<Networking>().client.CreateMessage();
				message.Write((byte)2); //WORLD
				message.Write((byte)9); //CHAT
				if(mapEditor == false)
					message.Write((byte)(chatMode.GetComponent<Dropdown>().value + 1));
				else
					message.Write((byte)1);
				message.Write(input.text);
				networking.GetComponent<Networking>().client.SendMessage(message, NetDeliveryMethod.Unreliable);
				input.text = "";
			}
		}
	}

	public void AddMessage(int mode, int authority, string username, string message)
	{
		GameObject chat = allChat;
		Color usernameColour = Color.white;
		Color messageColour = Color.white;
		switch(authority)
		{
			case 2:
				usernameColour = Color.green;
				break;
			case 3:
				usernameColour = Color.red;
				break;
		}

		if(mapEditor == false)
		{
			switch(mode)
			{
				case 2:
					chat = globalChat;
					break;
				case 3:
					chat = clanChat;
					messageColour = new Color(1, 0.7f, 0);
					break;
				case 4:
					chat = partyChat;
					messageColour = new Color(1, 0, 0.5f);
					break;
				case 5:
					chat = privateChat;
					messageColour = new Color(0, 1, 0.3f);
					break;
			}

			chat.transform.parent.parent.gameObject.SetActive(true);
			chat.gameObject.SetActive(true);
			GameObject text = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/ChatMessage"), chat.transform);
			text.GetComponent<ChatMessage>().SetMessage(username + ": ", message, usernameColour, messageColour);
			text.transform.SetAsFirstSibling();
			float height = 0;
			float textCount = 0;
			foreach(Transform child in chat.transform)
			{
				textCount += 1;
				if(textCount <= maxChatCount)
				{
					float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
					height += textHeight;
				}
				else
					Destroy(child.gameObject);
			}

			chat.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
			height = 0;

			foreach(Transform child in chat.transform)
			{
				float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
				float textWidth = child.gameObject.GetComponent<ChatMessage>().width;
				child.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(textWidth, (height + textHeight) - 8);
				height += textHeight;
			}

			if((chatMode.GetComponent<Dropdown>().value + 1) != mode)
				chat.transform.parent.parent.gameObject.SetActive(false);

			if(mode != 1)
			{
				allChat.transform.parent.parent.gameObject.SetActive(true);
				allChat.gameObject.SetActive(true);
				text = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/ChatMessage"), allChat.transform);
				text.GetComponent<ChatMessage>().SetMessage(username + ": ", message, usernameColour, messageColour);
				text.transform.SetAsFirstSibling();
				height = 0;
				textCount = 0;
				foreach(Transform child in allChat.transform)
				{
					textCount += 1;
					if(textCount <= maxChatCount)
					{
						float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
						height += textHeight;
					}
					else
						Destroy(child.gameObject);
				}

				allChat.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
				height = 0;

				foreach(Transform child in allChat.transform)
				{
					float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
					float textWidth = child.gameObject.GetComponent<ChatMessage>().width;
					child.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(textWidth, (height + textHeight) - 8);
					height += textHeight;
				}

				if(chatMode.GetComponent<Dropdown>().value > 0)
					allChat.transform.parent.parent.gameObject.SetActive(false);
			}
		}
		else
		{
			GameObject text = (GameObject)Instantiate(Resources.Load("Prefabs/GUI/ChatMessage"), chat.transform);
			text.GetComponent<ChatMessage>().SetMessage(username + ": ", message, usernameColour, messageColour);
			text.transform.SetAsFirstSibling();
			float height = 0;
			float textCount = 0;
			foreach(Transform child in chat.transform)
			{
				textCount += 1;
				if(textCount <= maxChatCount)
				{
					float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
					height += textHeight;
				}
				else
					Destroy(child.gameObject);
			}

			chat.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, height);
			height = 0;

			foreach(Transform child in chat.transform)
			{
				float textHeight = child.gameObject.GetComponent<ChatMessage>().height;
				float textWidth = child.gameObject.GetComponent<ChatMessage>().width;
				child.gameObject.GetComponent<RectTransform>().localPosition = new Vector2(textWidth, (height + textHeight) - 8);
				height += textHeight;
			}
		}
	}

	public void ActivateChat()
	{
		GameObject chat = null;

		if(mapEditor == false)
		{
			switch(chatMode.GetComponent<Dropdown>().value)
			{
				case 0:
					chat = allChat;
					globalChat.SetActive(false);
					globalChat.transform.parent.parent.gameObject.SetActive(false);
					clanChat.SetActive(false);
					clanChat.transform.parent.parent.gameObject.SetActive(false);
					partyChat.SetActive(false);
					partyChat.transform.parent.parent.gameObject.SetActive(false);
					privateChat.SetActive(false);
					privateChat.transform.parent.parent.gameObject.SetActive(false);
					break;
				case 1:
					chat = globalChat;
					allChat.SetActive(false);
					allChat.transform.parent.parent.gameObject.SetActive(false);
					clanChat.SetActive(false);
					clanChat.transform.parent.parent.gameObject.SetActive(false);
					partyChat.SetActive(false);
					partyChat.transform.parent.parent.gameObject.SetActive(false);
					privateChat.SetActive(false);
					privateChat.transform.parent.parent.gameObject.SetActive(false);
					break;
				case 2:
					chat = clanChat;
					allChat.SetActive(false);
					allChat.transform.parent.parent.gameObject.SetActive(false);
					globalChat.SetActive(false);
					globalChat.transform.parent.parent.gameObject.SetActive(false);
					partyChat.SetActive(false);
					partyChat.transform.parent.parent.gameObject.SetActive(false);
					privateChat.SetActive(false);
					privateChat.transform.parent.parent.gameObject.SetActive(false);
					break;
				case 3:
					chat = partyChat;
					allChat.SetActive(false);
					allChat.transform.parent.parent.gameObject.SetActive(false);
					globalChat.SetActive(false);
					globalChat.transform.parent.parent.gameObject.SetActive(false);
					clanChat.SetActive(false);
					clanChat.transform.parent.parent.gameObject.SetActive(false);
					privateChat.SetActive(false);
					privateChat.transform.parent.parent.gameObject.SetActive(false);
					break;
				case 4:
					chat = privateChat;
					allChat.SetActive(false);
					allChat.transform.parent.parent.gameObject.SetActive(false);
					globalChat.SetActive(false);
					globalChat.transform.parent.parent.gameObject.SetActive(false);
					clanChat.SetActive(false);
					clanChat.transform.parent.parent.gameObject.SetActive(false);
					partyChat.SetActive(false);
					partyChat.transform.parent.parent.gameObject.SetActive(false);
					break;
			}
			chat.SetActive(true);
			chat.transform.parent.parent.gameObject.SetActive(true);
			chat.transform.parent.parent.GetChild(1).GetComponent<Scrollbar>().value = 0;
		}
	}
}
