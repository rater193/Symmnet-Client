using mmo.dungeon;
using mmo.misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.player
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour
	{
		//This is used here so we dont have to keep calling GameObject.Find to get the player!
		//yay performance!
		public static PlayerController localPlayerController;

		//This is what we use to determine where to put the camera with the right joystick
		public Vector2 cameraOffset = Vector2.zero;

		private GameObject previouslyTargetedObject;
		private Color previousColor;
		private Vector2 aimDir = Vector2.zero;

		private bool fireballDebounce = false;

		private Rigidbody2D rb;
		// Use this for initialization
		void Start()
		{
			localPlayerController = this;
			rb = GetComponent<Rigidbody2D>();
		}

		// Update is called once per frame
		void Update()
		{
			if (PlayerMenuController.instance.defaultMenu == PlayerMenuController.instance.currentSelectedMenuGroup)
			{
				/////////////////////////////////////////
				//           Camera Control            //
				/////////////////////////////////////////
				cameraOffset = new Vector2(Input.GetAxis("ControllerAimHorizontal"), Input.GetAxis("ControllerAimVertical"));

				/////////////////////////////////////////
				//               Walking               //
				/////////////////////////////////////////
				float hspeed = Input.GetAxis("Horizontal");
				float vspeed = Input.GetAxis("Vertical");
                //Debug.Log("hspeed: " + hspeed);


				Animator animator = GetComponentInChildren<Animator>();
				string animState = "Idle";
				if (Mathf.Abs(cameraOffset.x) > 0 || Mathf.Abs(cameraOffset.y) > 0)
				{
					//Camera control rotation
					if (Mathf.Abs(cameraOffset.x) > Mathf.Abs(cameraOffset.y))
					{
						animState = cameraOffset.x > 0 ? "Right" : "Left";
					}
					else
					{
						animState = cameraOffset.y > 0 ? "Up" : "Down";
					}
					//Making the player move slower when aiming
					hspeed = hspeed / 2f;
					vspeed = vspeed / 2f;
				}
				else
				{
					//Movement control rotation
					if (hspeed != 0 || vspeed != 0)
					{
						if (Mathf.Abs(hspeed) > Mathf.Abs(vspeed))
						{
							animState = hspeed > 0 ? "Right" : "Left";
						}
						else
						{
							animState = vspeed > 0 ? "Up" : "Down";
						}
					}
				}

				rb.velocity = new Vector2(hspeed, vspeed);

				animator.speed = Vector2.Distance(Vector2.zero, rb.velocity);
				animator.Play(animState);

				/////////////////////////////////////////
				//             Interacting             //
				/////////////////////////////////////////
				//We only want to do the raycast if we are moving
				if (Mathf.Abs(hspeed) > 0 || Mathf.Abs(vspeed) > 0 || Mathf.Abs(cameraOffset.x) > 0 || Mathf.Abs(cameraOffset.y) > 0)
				{

					RaycastHit2D hit = new RaycastHit2D();
					if (Mathf.Abs(cameraOffset.x) > 0 || Mathf.Abs(cameraOffset.y) > 0)
					{
						aimDir = cameraOffset;
					}
					else
					{
						aimDir = rb.velocity;
					}
					hit = Physics2D.Raycast(transform.position, aimDir, 0.3f, LayerMask.GetMask("Interactable"));
					if (hit)
					{
						GameObject tar = hit.collider.gameObject;
						if (tar != previouslyTargetedObject)
						{
							if (tar.GetComponent<InteractableBase>())
							{
								if (previouslyTargetedObject)
								{
									previouslyTargetedObject.GetComponent<SpriteRenderer>().color = previousColor;
								}

								previousColor = tar.GetComponent<SpriteRenderer>().color;
								tar.GetComponent<SpriteRenderer>().color = Color.green;
								previouslyTargetedObject = tar;
							}
						}
					}
					else
					{
						if (previouslyTargetedObject)
						{
							previouslyTargetedObject.GetComponent<SpriteRenderer>().color = previousColor;
							previouslyTargetedObject = null;
						}
					}
				}

				if (Input.GetButtonDown("ControllerInteract"))
				{
					if (previouslyTargetedObject)
					{
						if (previouslyTargetedObject.GetComponent<InteractableBase>())
						{
							StartCoroutine(previouslyTargetedObject.GetComponent<InteractableBase>().Interact());
						}
					}
				}

				/////////////////////////////////////////
				//              Fireball               //
				/////////////////////////////////////////
				if (Input.GetAxis("Shoot") >= 0.3f)
				{
					if (fireballDebounce == false)
					{
						GameObject newObj = Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/Fireball"), transform.position, Quaternion.identity);
						newObj.GetComponent<FireballTriggerHandler>().dir = aimDir;
						newObj.GetComponent<FireballTriggerHandler>().speed = 2f;
						fireballDebounce = true;
					}
				}
				else
				{
					fireballDebounce = false;
				}

				//Finally we check for interacting
			}
		}
	}
}