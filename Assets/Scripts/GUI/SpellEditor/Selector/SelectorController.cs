using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mmo.gui.spelleditor
{
    public class SelectorController : MonoBehaviour
    {
        public float maxTransitionSpeed = 100;
        public float transitionAcceleration = 1.1f;
        public int selectedIndex = 0;

        private float speed = 0;
        private float posX = 0;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetAxis("Horizontal") > 0)
            {
                selectedIndex++;
            }
            if (Input.GetAxis("Horizontal") > 0)
            {
                selectedIndex--;
            }

            float targetPos = selectedIndex * 256f;
            float distance = Mathf.Abs(targetPos - posX);
            float s = targetPos > posX ? distance * 0.1f : -distance * 0.1f;

            int index = Mathf.RoundToInt(posX / 256f);

            for (int pos = 0; pos < transform.childCount; pos++)
            {
                Transform child = transform.GetChild(pos);
                if (child)
                {

                    child = child.Find("Storage");

                    Image image = child.GetComponent<Image>();

                    if (image)
                    {
                        //Debug.Log("1");
                        float curPos = pos * 256f;
                        float difference = Mathf.Abs((curPos - posX) / 256f);
                        image.color = new Color(1, 1, 1, difference / 4f * -1f + 1f);
                        float scale = 0.5f + (image.color.a / 2f);
                        image.transform.localScale = new Vector3(scale, scale, scale);
                    }
                    else
                    {
                    }
                }
            }

            #region Alternate formula (broken but more performance effective
            /*
            for (int pos = index - 5; pos < index + 5; pos++)
            {
                if (pos >= 0 && pos < transform.childCount)
                {
                    Transform child = transform.GetChild(pos);
                    if (child)
                    {

                        child = child.Find("Storage");

                        Image image = child.GetComponent<Image>();

                        if (image)
                        {
                            //Debug.Log("1");
                            float curPos = pos * 256f;
                            float difference = Mathf.Abs((curPos - posX) / 256f);
                            image.color = new Color(1, 1, 1, difference / 4f * -1f + 1f);
                            float scale = 0.5f + (image.color.a / 2f);
                            image.transform.localScale = new Vector3(scale, scale, scale);
                        }
                        else
                        {
                            Debug.Log("2");
                        }
                    }
                }
            }
            */
            #endregion

            if (distance > s && distance > 1)
            {
                //Debug.Log("Distance: " + distance);
                posX += s;
            }
            else
            {
                //Debug.Log("Done? Distance: " + distance);
                posX = targetPos;
            }


            GetComponent<RectTransform>().localPosition = new Vector2(-posX, 0);

        }
    }
}