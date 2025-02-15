//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// EditorGUILayout.ObjectField doesn't support custom components, so a custom wizard saves the day.
/// Unfortunately this tool only shows components that are being used by the scene, so it's a "recently used" selection tool.
/// </summary>

public class ComponentSelector : ScriptableWizard
{
	public delegate void OnSelectionCallback (Object obj);

	System.Type mType;
	string mTitle;
	OnSelectionCallback mCallback;
	Object[] mObjects;
	bool mSearched = false;

	static string GetName (System.Type t)
	{
		string s = t.ToString();
		s = s.Replace("UnityEngine.", "");
		if (s.StartsWith("UI")) s = s.Substring(2);
		return s;
	}

	/// <summary>
	/// Draw a button + object selection combo filtering specified types.
	/// </summary>

	static public void Draw<T> (string buttonName, T obj, OnSelectionCallback cb, bool editButton, params GUILayoutOption[] options) where T : Object
	{
		GUILayout.BeginHorizontal();
		bool show = NGUIEditorTools.DrawPrefixButton(buttonName);
		T o = EditorGUILayout.ObjectField(obj, typeof(T), false, options) as T;

		if (editButton && o != null && o is MonoBehaviour)
		{
			Component mb = o as Component;
			if (Selection.activeObject != mb.gameObject && GUILayout.Button("Edit", GUILayout.Width(40f)))
				Selection.activeObject = mb.gameObject;
		}
		GUILayout.EndHorizontal();
		if (show) Show<T>(cb);
		else if (o != obj) cb(o);
	}

	/// <summary>
	/// Draw a button + object selection combo filtering specified types.
	/// </summary>

	static public void Draw<T> (T obj, OnSelectionCallback cb, bool editButton, params GUILayoutOption[] options) where T : Object
	{
		Draw<T>(NGUITools.GetTypeName<T>(), obj, cb, editButton, options);
	}

	/// <summary>
	/// Show the selection wizard.
	/// </summary>

	static public void Show<T> (OnSelectionCallback cb) where T : Object
	{
		System.Type type = typeof(T);
		string title = (type == typeof(UIAtlas) ? "Select an " : "Select a ") + GetName(type);
		ComponentSelector comp = ScriptableWizard.DisplayWizard<ComponentSelector>(title);
		comp.mTitle = title;
		comp.mType = type;
		comp.mCallback = cb;
		comp.mObjects = Resources.FindObjectsOfTypeAll(typeof(T));

		if (comp.mObjects == null || comp.mObjects.Length == 0)
		{
			comp.Search();
		}
		else
		{
			System.Array.Sort(comp.mObjects,
				delegate(Object a, Object b) { return a.name.CompareTo(b.name); });
		}
	}

	/// <summary>
	/// Search the entire project for required assets.
	/// </summary>

	void Search ()
	{
		mSearched = true;
		string[] paths = AssetDatabase.GetAllAssetPaths();
		bool isComponent = mType.IsSubclassOf(typeof(Component));
		BetterList<Object> list = new BetterList<Object>();

		for (int i = 0; i < paths.Length; ++i)
		{
			string path = paths[i];

			if (path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase))
			{
				EditorUtility.DisplayProgressBar("Loading", "Searching assets, please wait...", (float)i / paths.Length);
				Object obj = AssetDatabase.LoadMainAssetAtPath(path);
				if (obj == null) continue;

				if (!isComponent)
				{
					System.Type t = obj.GetType();
					if (t == mType || t.IsSubclassOf(mType))
						list.Add(obj);
				}
				else if (PrefabUtility.GetPrefabType(obj) == PrefabType.Prefab)
				{
					Object t = (obj as GameObject).GetComponent(mType);
					if (t != null) list.Add(t);
				}
			}
		}
		list.Sort(delegate(Object a, Object b) { return a.name.CompareTo(b.name); });
		mObjects = list.ToArray();
		EditorUtility.ClearProgressBar();
	}

	/// <summary>
	/// Draw the custom wizard.
	/// </summary>

	void OnGUI ()
	{
		NGUIEditorTools.SetLabelWidth(80f);
		GUILayout.Label(mTitle, "LODLevelNotifyText");
		GUILayout.Space(6f);

		if (mObjects.Length == 0)
		{
			EditorGUILayout.HelpBox("No " + GetName(mType) + " components found.\nTry creating a new one.", MessageType.Info);

			bool isDone = false;

			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			if (mType == typeof(UIFont))
			{
				if (GUILayout.Button("Open the Font Maker", GUILayout.Width(150f)))
				{
					EditorWindow.GetWindow<UIFontMaker>(false, "Font Maker", true);
					isDone = true;
				}
			}
			else if (mType == typeof(UIAtlas))
			{
				if (GUILayout.Button("Open the Atlas Maker", GUILayout.Width(150f)))
				{
					EditorWindow.GetWindow<UIAtlasMaker>(false, "Atlas Maker", true);
					isDone = true;
				}
			}

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (isDone) Close();
		}
		else
		{
			Object sel = null;

			foreach (Object o in mObjects)
			{
				if (DrawObject(o))
				{
					sel = o;
				}
			}

			if (sel != null)
			{
				mCallback(sel);
				Close();
			}
		}

		if (!mSearched)
		{
			GUILayout.Space(6f);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			bool search = GUILayout.Button("Show All", "LargeButton", GUILayout.Width(120f));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (search) Search();
		}
	}

	/// <summary>
	/// Draw details about the specified object in column format.
	/// </summary>

	bool DrawObject (Object obj)
	{
		bool retVal = false;
		Component comp = obj as Component;

		GUILayout.BeginHorizontal();
		{
			if (comp != null && EditorUtility.IsPersistent(comp.gameObject))
				GUI.contentColor = new Color(0.6f, 0.8f, 1f);

			retVal |= GUILayout.Button(obj.name, "AS TextArea", GUILayout.Width(120f), GUILayout.Height(20f));
			retVal |= GUILayout.Button(AssetDatabase.GetAssetPath(obj).Replace("Assets/", ""), "AS TextArea", GUILayout.Height(20f));
			GUI.contentColor = Color.white;

			retVal |= GUILayout.Button("Select", "ButtonLeft", GUILayout.Width(60f), GUILayout.Height(16f));
		}
		GUILayout.EndHorizontal();
		return retVal;
	}
}
