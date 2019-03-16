using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.misc
{
    public class CanvasGroupAutohide : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            GetComponent<CanvasGroup>().alpha = 0f;
            GetComponent<CanvasGroup>().interactable = false;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }
}