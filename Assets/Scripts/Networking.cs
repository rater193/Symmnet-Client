using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
using System.Linq;
using UnityEngine.SceneManagement;
using Lidgren.Network;
using System.IO;

public class Networking : MonoBehaviour
{
	public string serverIp;
	private int serverId;
	public bool mapEditor = false;
	public NetClient BESClient = null;
	public NetClient client = null;

	private void Awake()
	{
		Application.targetFrameRate = 60;
		DontDestroyOnLoad(this);
	}

	private void Start()
	{
		NetPeerConfiguration config = new NetPeerConfiguration("MMO");
		//config.EnableUPnP = true;
		BESClient = new NetClient(config);
		//BESClient.UPnP.ForwardPort(8002, "Forwarded the main server's port to connect to");
		BESClient.Start();
		NetOutgoingMessage message = BESClient.CreateMessage();
		message.Write((byte)2); //CONNECT
#if UNITY_EDITOR
		serverIp = File.ReadAllText("../ip.txt");
		BESClient.Connect(serverIp, 8002, message);
#else
		BESClient.Connect(serverIp, 8002, message);
#endif
		InvokeRepeating("PingBES", 5, 5);
	}

	private void Update()
	{
		NetIncomingMessage data;

		if((data = BESClient.ReadMessage()) != null)
		{
			switch(data.MessageType)
			{
				case NetIncomingMessageType.StatusChanged:
					if((NetConnectionStatus)data.ReadByte() == NetConnectionStatus.Connected) //CONNECTED
					{
						NetOutgoingMessage message = BESClient.CreateMessage(2);
						message.Write((byte)2); //CLIENT ID
						BESClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
					}
					break;
				case NetIncomingMessageType.Data:
					GetComponent<DataHandle>().HandleDataBES(data);
					break;
			}
		}

		if(client != null)
		{
			if((data = client.ReadMessage()) != null)
			{
				switch(data.MessageType)
				{
					case NetIncomingMessageType.StatusChanged:
						if((NetConnectionStatus)data.ReadByte() == NetConnectionStatus.Connected) //CONNECTED
						{
							if(serverId == 0)
							{
								SceneManager.LoadScene("MapEditor");
								mapEditor = true;
							}
							else
								SceneManager.LoadScene("World");
							Invoke("JoinedWorld", 1);
						}
						break;
					case NetIncomingMessageType.Data:
						GetComponent<DataHandle>().HandleDataServer(data);
						break;
				}
			}
		}
	}

	public void ServerConnect(int serverId)
	{
		Debug.Log("CONNECTING TO REGIONAL SERVER");
		this.serverId = serverId;

		NetPeerConfiguration config = new NetPeerConfiguration("MMO");
		//config.EnableUPnP = true;
		client = new NetClient(config);
		//client.UPnP.ForwardPort(8001, "Forwarded the regional server's port to connect to");
		client.Start();
		NetOutgoingMessage message = client.CreateMessage();
		message.Write(MyInfo.playerId);

		switch(serverId)
		{
			case 0:
				client.Connect(serverIp, 8001, message);
				break;
			case 1:
				client.Connect(serverIp, 8001, message);
				break;
		}

		InvokeRepeating("PingServer", 5, 5);
	}

	private void JoinedWorld()
	{
		NetOutgoingMessage message = client.CreateMessage();
		message.Write((byte)2); //WORLD
		message.Write((byte)1); //JOINED WORLD
		client.SendMessage(message, NetDeliveryMethod.ReliableOrdered, 0);
	}

	private void PingBES()
	{
		NetOutgoingMessage message = BESClient.CreateMessage();
		message.Write((byte)1); //PING
		BESClient.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
	}

	private void PingServer()
	{
		NetOutgoingMessage message = client.CreateMessage();
		message.Write((byte)1); //PING
		client.SendMessage(message, NetDeliveryMethod.ReliableOrdered);
	}

	private void OnApplicationQuit()
	{
		BESClient.Disconnect("Quit");
		if(client != null)
			client.Disconnect("Quit");
	}
}