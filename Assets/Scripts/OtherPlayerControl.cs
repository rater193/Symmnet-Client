using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerControl : MonoBehaviour
{
	public float moveSpeed;
	public float lookAngle = 0;
	public float lookPos = 0;
	public float gotoX;
	public float gotoY;
	public bool flip;
	private bool sway = false;
	private bool bounce = false;
	private float swayAmount = 0;
	private float bounceAmount = 0;
	public Rigidbody2D player;
	public GameObject body;
	public GameObject hand;
	public GameObject rightHand;
	public GameObject leftHand;

	private void Start()
	{
		player = GetComponent<Rigidbody2D>();
		gotoX = player.position.x;
		gotoY = player.position.y;
	}

	public void Update()
	{
		Movement();
		MoveAnimation();
	}

	#region Player movement
	private void Movement()
	{
		player.position = Vector2.Lerp(player.position, new Vector2(gotoX, gotoY), 0.1f);

		Quaternion angle = Quaternion.Euler(new Vector3(0, 0, lookAngle));
		hand.transform.localRotation = Quaternion.Lerp(hand.transform.localRotation, angle, 0.1f);

		if(flip == true)
		{
			body.transform.localScale = new Vector3(1, 1, 1);
			hand.transform.localScale = new Vector3(1, 1, 1);
			rightHand.transform.localPosition = new Vector3(-0.02f, -0.08f, -0.02f);
			leftHand.transform.localPosition = new Vector3(0.02f, -0.08f, 0.02f);
		}
		else
		{
			body.transform.localScale = new Vector3(-1, 1, 1);
			hand.transform.localScale = new Vector3(1, -1, 1);
			rightHand.transform.localPosition = new Vector3(0.02f, -0.08f, 0.02f);
			leftHand.transform.localPosition = new Vector3(-0.02f, -0.08f, -0.02f);
		}
	}
	#endregion

	#region Movement animation
	private void MoveAnimation()
	{
		if(Vector2.Distance(player.position, new Vector2(gotoX, gotoY)) > 0.1f)
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

		body.transform.rotation = Quaternion.Euler(new Vector3(0, 0, swayAmount / 2));
		body.transform.localPosition = new Vector2(0, bounceAmount / 250 * Time.deltaTime * 60);
		hand.transform.localPosition = new Vector2(0, bounceAmount / 250 * Time.deltaTime * 60);
	}
	#endregion
}