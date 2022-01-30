using UnityEngine;

namespace Utility.Extensions
{

	public static class UnityExtensions
	{
		public static void SafeDestroy<T>(this T obj, bool assetDelete = false) where T : Object
		{
			if (obj == null) return;
		
#if UNITY_EDITOR
			if (UnityEditor.AssetDatabase.IsMainAsset(obj))
			{
				return; // cannot destroy main asset
			}
#endif
			(obj as RenderTexture)?.Release();
		
			if (Application.isPlaying)
			{
				Object.Destroy(obj);
			} else
			{
				Object.DestroyImmediate(obj, assetDelete);
			}
		}

		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			var c = gameObject.GetComponent<T>();
			if (c == null)
				c = gameObject.AddComponent<T>();
			return c;
		}
	
		public static T InstantiateTemplate<T>(this T template, Transform parent = null) where T : Component
		{
			Debug.Assert(template != null);

			if (parent == null) parent = template.transform.parent;
			var componentInstance      = Object.Instantiate(template, parent);
			componentInstance.gameObject.SetActive(true);
		
			return componentInstance;
		}
	}

}