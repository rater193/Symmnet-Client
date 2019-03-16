using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.gui.spelleditor
{
    public class SpellGridManager : MonoBehaviour
    {

        public static SpellGridManager instance;

        public RectTransform spellNodeStorage;

        SpellGridNode initialNode;

        // Use this for initialization
        void Start()
        {
            instance = this;
            initialNode = SpellGridNode.createNode(0, 0);

            SpellGridNode.expand();//1
            //SpellGridNode.expand();//20
            //SpellGridNode.expand();//40
            //SpellGridNode.expand();//80
            //SpellGridNode.expand();//40
            //SpellGridNode.expand();//80
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.LeftControl))
            {
                SpellGridNode.expand();
            }
        }
    }
}