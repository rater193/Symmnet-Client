using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mmo.gui.spelleditor
{
    public class SpellGridNode : MonoBehaviour
    {

        public static Dictionary<string, SpellGridNode> spellgrid = new Dictionary<string, SpellGridNode>();

        public int posX = 0;
        public int posY = 0;

#region Point Positions
        //north
        public int northPosX { get { return posX; } }
        public int northPosY { get { return posY + 2; } }

        //south
        public int southPosX { get { return posX; } }
        public int southPosY { get { return posY - 2; } }

        //northeast
        public int northeastPosX { get { return posX + 1; } }
        public int northeastPosY { get { return posY + 1; } }

        //southeast
        public int southeastPosX { get { return posX + 1; } }
        public int southeastPosY { get { return posY - 1; } }

        //northwest
        public int northwestPosX { get { return posX - 1; } }
        public int northwestPosY { get { return posY + 1; } }

        //southwest
        public int southwestPosX { get { return posX - 1; } }
        public int southwestPosY { get { return posY - 1; } }
#endregion

        // Use this for initialization
        void Init()
        {
            if(spellgrid.ContainsKey(getNodeKey()))
            {
                //Removing duplicate node
                Debug.Log("Node already exists, removing new node.");
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("Registering new node: " + getNodeKey());
                spellgrid.Add(getNodeKey(), this);
            }

            gameObject.name = getNodeKey();
        }


        public void createSurroundings()
        {
            //Creating the surounding nodes
            createNode(northPosX, northPosY);
            createNode(northwestPosX, northwestPosY);
            createNode(northeastPosX, northeastPosY);
            createNode(southPosX, southPosY);
            createNode(southeastPosX, southeastPosY);
            createNode(southwestPosX, southwestPosY);
        }

        public bool isBorder()
        {
            return
                !spellgrid.ContainsKey(getNodeKey(northPosX, northPosY)) ||
                !spellgrid.ContainsKey(getNodeKey(northwestPosX, northwestPosY)) ||
                !spellgrid.ContainsKey(getNodeKey(northeastPosX, northeastPosY)) ||
                !spellgrid.ContainsKey(getNodeKey(southPosX, southPosY)) ||
                !spellgrid.ContainsKey(getNodeKey(southeastPosX, southeastPosY)) ||
                !spellgrid.ContainsKey(getNodeKey(southwestPosX, southwestPosY))
                ? true : false
                ;
        }

        public string getNodeKey()
        {
            return getNodeKey(posX, posY);
        }


        public static string getNodeKey(int x, int y)
        {
            return x + "," + y;
        }

        /// <summary>
        /// Call this to expand the spell crafting grid size by 1
        /// </summary>
        public static void expand()
        {
            //This is the list of border tiles to try to expand on to save on performance.
            List<SpellGridNode> bordersToExpand = new List<SpellGridNode>();
            
            //Calculating the border tiles
            //WARNING: iterating through each tile to try to find the border
            //If this becomes a problem, then i will optimize it later
            //This shouldnt be a problem for the client unless they are over lv 20 in the grid size
            foreach(SpellGridNode node in spellgrid.Values)
            {
                if(node.isBorder())
                {
                    bordersToExpand.Add(node);
                }
            }

            foreach(var node in bordersToExpand)
            {
                node.createSurroundings();
            }
        }


        /// <summary>
        /// Call this to create a new node on the gui spell editor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static SpellGridNode createNode(int x, int y)
        {
            //returning null if it already exists
            if (spellgrid.ContainsKey(getNodeKey(x, y))) return null;

            //Otherwise, continue the code
            GameObject newNode = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GUI/SpellEditor/SpellNode"));
            var node = newNode.GetComponent<SpellGridNode>();

            //Setting the newly created node's UI position
            node.transform.parent = SpellGridManager.instance.spellNodeStorage.transform;
            node.transform.localPosition = new Vector2(x * 193, y * 112);
            node.posX = x;
            node.posY = y;

            node.transform.localScale = new Vector2(1,1);

            //Initializing the node
            node.Init();


            return node;
        }
    }
}