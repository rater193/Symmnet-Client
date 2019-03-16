using Lidgren.Network;
using symmnet.client.rater193;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
	public float moveSpeed;
	public float oldPosX;
	public float oldPosY;
	private float look;
	private bool flip;
	private bool sway = false;
	private bool bounce = false;
	private float swayAmount = 0;
	private float bounceAmount = 0;
	public GameObject networking;
	public Rigidbody2D player;
	public GameObject body;
	public GameObject hand;
	public GameObject rightHand;
	public GameObject leftHand;

	private void Start()
	{
		player = GetComponent<Rigidbody2D>();
		InvokeRepeating("SendPosition", 0.05f, 0.05f);
		InvokeRepeating("SendLook", 0.05f, 0.05f);
		Rater193.LocalPlayerStart(this);
	}

	private void Update()
	{
		//Camera.main.transform.position = new Vector3(player.position.x, player.position.y, Camera.main.transform.position.z);

		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Movement(horizontal, vertical);
		MoveAnimation(horizontal, vertical);

		if(MyInfo.mapEditor == true && networking.GetComponent<Networking>().mapEditor == true)
			Rater193.LocalPlayerUpdate(this);
	}

	private void FixedUpdate()
	{
		if(MyInfo.mapEditor == true && networking.GetComponent<Networking>().mapEditor == true)
			Rater193.LocalPlayerFixedUpdate(this);
	}

	#region Player movement
	private void Movement(float horizontal, float vertical)
	{
		Vector3 direction = new Vector3(horizontal, vertical, 0);
		direction = direction.normalized * moveSpeed;
		player.velocity = direction;

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 lookMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lookMousePos.z = 0;
		lookMousePos.x = lookMousePos.x - transform.position.x;
		lookMousePos.y = lookMousePos.y - transform.position.y;
		look = Mathf.Atan2(lookMousePos.y, lookMousePos.x) * Mathf.Rad2Deg;
		hand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, look));

		if(mousePos.x > transform.position.x)
		{
			flip = true;
			body.transform.localScale = new Vector3(1, 1, 1);
			hand.transform.localScale = new Vector3(1, 1, 1);
			rightHand.transform.localPosition = new Vector3(-0.02f, -0.08f, -0.02f);
			leftHand.transform.localPosition = new Vector3(0.02f, -0.08f, 0.02f);
		}
		else
		{
			flip = false;
			body.transform.localScale = new Vector3(-1, 1, 1);
			hand.transform.localScale = new Vector3(1, -1, 1);
			rightHand.transform.localPosition = new Vector3(0.02f, -0.08f, 0.02f);
			leftHand.transform.localPosition = new Vector3(-0.02f, -0.08f, -0.02f);
		}
	}
	#endregion

	#region Movement animation
	private void MoveAnimation(float horizontal, float vertical)
	{
		if(horizontal > 0 || vertical > 0
		|| horizontal < 0 || vertical < 0)
		{
			if(sway == true)
				swayAmount += moveSpeed / 2;
			else
				swayAmount -= moveSpeed / 2;

			if(swayAmount >= 10)
				sway = false;
			if(swayAmount <= -10)
				sway = true;

			if(bounce == true)
				bounceAmount += moveSpeed / 2;
			else
				bounceAmount -= moveSpeed / 2;

			if(bounceAmount <= 0)
				bounce = true;
			if(bounceAmount >= 10)
				bounce = false;
		}
		else
		{
			if(swayAmount < 0)
				swayAmount += moveSpeed / 2;
			if(swayAmount > 0)
				swayAmount -= moveSpeed / 2;

			if(bounceAmount < 0)
				bounceAmount += moveSpeed / 2;
			if(bounceAmount > 0)
				bounceAmount -= moveSpeed / 2;
		}

		body.transform.rotation = Quaternion.Euler(new Vector3(0, 0, swayAmount / 2 * Time.deltaTime * 60));
		body.transform.localPosition = new Vector2(0, bounceAmount / 250 * Time.deltaTime * 60);
		hand.transform.localPosition = new Vector2(0, bounceAmount / 250 * Time.deltaTime * 60);
	}
	#endregion

	#region Sending updates to the server
	private void SendPosition()
	{
		if(Vector2.Distance(player.position, new Vector2(oldPosX, oldPosY)) > 0.01f)
		{
			string posX = player.position.x.ToString("0.00");
			string posY = player.position.y.ToString("0.00");
			NetOutgoingMessage message = networking.GetComponent<Networking>().client.CreateMessage();
			message.Write((byte)2); //WORLD
			message.Write((byte)2); //PLAYER
			message.Write((byte)1); //POSITION
			message.Write(float.Parse(posX));
			message.Write(float.Parse(posY));
			networking.GetComponent<Networking>().client.SendMessage(message, NetDeliveryMethod.Unreliable);
		}

		oldPosX = player.position.x;
		oldPosY = player.position.y;
	}

	private void SendLook()
	{
		string angle = look.ToString("0.00");
		NetOutgoingMessage message = networking.GetComponent<Networking>().client.CreateMessage();
		message.Write((byte)2); //WORLD
		message.Write((byte)2); //PLAYER
		message.Write((byte)2); //POSITION
		message.Write(float.Parse(angle));
		message.Write(flip);
		networking.GetComponent<Networking>().client.SendMessage(message, NetDeliveryMethod.Unreliable);
	}
	#endregion
}