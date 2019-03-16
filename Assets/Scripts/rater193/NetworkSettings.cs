using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace symmnet.client.rater193
{
	public class NetworkSettings : MonoBehaviour
	{
		public static NetworkSettings instance;
		public static int chunkSize = 16;

		public Dictionary<int, GameObject> tileEntityPrefabs = new Dictionary<int, GameObject>();


		private void Start()
		{
			if(GameObject.FindObjectOfType<NetworkSettings>())
			{
				if (GameObject.FindObjectOfType<NetworkSettings>() != this) {
					Destroy(gameObject);
					return;
				}
			}

			instance = this;
			DontDestroyOnLoad(this);
			Init();
		}

		void Init()
		{
			#region ores
			Debug.Log("Registering ores");
			tileEntityPrefabs.Add(100, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/CopperRock1"));
			tileEntityPrefabs.Add(101, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/CopperRock2"));
			tileEntityPrefabs.Add(102, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/TinRock1"));
			tileEntityPrefabs.Add(103, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/TinRock2"));
			tileEntityPrefabs.Add(104, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/IronRock1"));
			tileEntityPrefabs.Add(105, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/IronRock2"));
			tileEntityPrefabs.Add(106, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/MithrilRock1"));
			tileEntityPrefabs.Add(107, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/MithrilRock2"));
			tileEntityPrefabs.Add(108, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/AdamantiteRock1"));
			tileEntityPrefabs.Add(109, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/AdamantiteRock2"));
			tileEntityPrefabs.Add(110, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/ArcaniteRock1"));
			tileEntityPrefabs.Add(111, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/ArcaniteRock2"));
			tileEntityPrefabs.Add(112, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/StellariteRock1"));
			tileEntityPrefabs.Add(113, Resources.Load<GameObject>("Prefabs/Environment/OreRocks/StellariteRock2"));
			#endregion
			#region trees
			tileEntityPrefabs.Add(4000, Resources.Load<GameObject>("Prefabs/Environment/Trees/NormalTree"));
			tileEntityPrefabs.Add(4001, Resources.Load<GameObject>("Prefabs/Environment/Trees/BirchTree"));
			tileEntityPrefabs.Add(4002, Resources.Load<GameObject>("Prefabs/Environment/Trees/MapleTree"));
			tileEntityPrefabs.Add(4003, Resources.Load<GameObject>("Prefabs/Environment/Trees/OakTree"));
			tileEntityPrefabs.Add(4004, Resources.Load<GameObject>("Prefabs/Environment/Trees/PineTree"));
			tileEntityPrefabs.Add(4005, Resources.Load<GameObject>("Prefabs/Environment/Trees/ShadowbarkTree"));
			tileEntityPrefabs.Add(4006, Resources.Load<GameObject>("Prefabs/Environment/Trees/ElderwoodTree"));
			#endregion
		}

		public GameObject getTileentityPrefab(int id)
		{
			GameObject ret;
			tileEntityPrefabs.TryGetValue(id, out ret);
			//Debug.Log("id: " + id + ", " + ret);
			return ret;
		}

	}
}