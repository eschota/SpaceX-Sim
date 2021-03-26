using UnityEngine;

namespace eWolfRoadBuilder
{
	/// <summary>
	/// GameObject Helper 
	/// </summary>
	public class GameObjectHelper
	{
		/// <summary>
		/// Find the child object with name
		/// </summary>
		/// <param name="baseObject">The base object to find the child on</param>
		/// <param name="childObjName">Name of the object to look for</param>
		/// <returns>The child object with the name</returns>
		public static GameObject GetChildObject(GameObject baseObject, string childObjName)
		{
			Transform[] allChildren = baseObject.GetComponentsInChildren<Transform>(true);
			foreach (Transform child in allChildren)
			{
				if (child.name == childObjName)
				{
					return child.gameObject;
				}
			}

			return null;
		}

		/// <summary>
		/// Find the object with this name
		/// </summary>
		/// <param name="objectName">The name of the object we want</param>
		/// <returns>The object if found</returns>
		public static GameObject Find(string objectName)
		{
			return GameObject.Find(objectName);
		}
	}
}