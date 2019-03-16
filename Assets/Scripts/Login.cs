using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lidgren.Network;

public class Login : MonoBehaviour
{
	public GameObject networking;
	public InputField username;
	public Dropdown region;
	private bool sentRequest = false;

	public void LoginRequest()
	{
		if(sentRequest == false)
		{
			NetOutgoingMessage message = networking.GetComponent<Networking>().BESClient.CreateMessage();
			message.Write((byte)3); //ACCOUNT
			message.Write((byte)1); //LOGIN
			message.Write(username.text);
			message.Write(region.value);
			networking.GetComponent<Networking>().BESClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
			sentRequest = true;
		}
	}
}
