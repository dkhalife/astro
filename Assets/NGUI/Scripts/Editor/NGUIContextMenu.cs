using UnityEngine;
using UnityEditor;

/// <summary>
/// This editor helper class makes it easy to create and show a context menu.
/// It ensures that it's possible to add multiple items with the same name.
/// </summary>

public static class NGUIContextMenu
{
	[MenuItem("Help/NGUI Documentation")]
	static void ShowHelp0 (MenuCommand command) { NGUIHelp.Show(); }

	[MenuItem("CONTEXT/UIWidget/Copy Widget")]
	static void CopyStyle (MenuCommand command) { NGUISettings.CopyWidget(command.context as UIWidget); }

	[MenuItem("CONTEXT/UIWidget/Paste Widget Values")]
	static void PasteStyle (MenuCommand command) { NGUISettings.PasteWidget(command.context as UIWidget, true); }

	[MenuItem("CONTEXT/UIWidget/Paste Widget Style")]
	static void PasteStyle2 (MenuCommand command) { NGUISettings.PasteWidget(command.context as UIWidget, false); }

	[MenuItem("CONTEXT/UIWidget/Help")]
	static void ShowHelp1 (MenuCommand command) { NGUIHelp.Show(command.context); }

	[MenuItem("CONTEXT/UIButton/Help")]
	static void ShowHelp2 (MenuCommand command) { NGUIHelp.Show(typeof(UIButton)); }

	[MenuItem("CONTEXT/UIToggle/Help")]
	static void ShowHelp3 (MenuCommand command) { NGUIHelp.Show(typeof(UIToggle)); }

	[MenuItem("CONTEXT/UIRoot/Help")]
	static void ShowHelp4 (MenuCommand command) { NGUIHelp.Show(typeof(UIRoot)); }

	[MenuItem("CONTEXT/UICamera/Help")]
	static void ShowHelp5 (MenuCommand command) { NGUIHelp.Show(typeof(UICamera)); }

	[MenuItem("CONTEXT/UIAnchor/Help")]
	static void ShowHelp6 (MenuCommand command) { NGUIHelp.Show(typeof(UIAnchor)); }

	[MenuItem("CONTEXT/UIStretch/Help")]
	static void ShowHelp7 (MenuCommand command) { NGUIHelp.Show(typeof(UIStretch)); }

	[MenuItem("CONTEXT/UISlider/Help")]
	static void ShowHelp8 (MenuCommand command) { NGUIHelp.Show(typeof(UISlider)); }

#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
	[MenuItem("CONTEXT/UI2DSprite/Help")]
	static void ShowHelp9 (MenuCommand command) { NGUIHelp.Show(typeof(UI2DSprite)); }
#endif

	[MenuItem("CONTEXT/UIScrollBar/Help")]
	static void ShowHelp10 (MenuCommand command) { NGUIHelp.Show(typeof(UIScrollBar)); }

	[MenuItem("CONTEXT/UIProgressBar/Help")]
	static void ShowHelp11 (MenuCommand command) { NGUIHelp.Show(typeof(UIProgressBar)); }

	[MenuItem("CONTEXT/UIPopupList/Help")]
	static void ShowHelp12 (MenuCommand command) { NGUIHelp.Show(typeof(UIPopupList)); }

	[MenuItem("CONTEXT/UIInput/Help")]
	static void ShowHelp13 (MenuCommand command) { NGUIHelp.Show(typeof(UIInput)); }

	[MenuItem("CONTEXT/UIKeyBinding/Help")]
	static void ShowHelp14 (MenuCommand command) { NGUIHelp.Show(typeof(UIKeyBinding)); }

	[MenuItem("CONTEXT/UIGrid/Help")]
	static void ShowHelp15 (MenuCommand command) { NGUIHelp.Show(typeof(UIGrid)); }

	[MenuItem("CONTEXT/UITable/Help")]
	static void ShowHelp16 (MenuCommand command) { NGUIHelp.Show(typeof(UITable)); }

	[MenuItem("CONTEXT/UIPlayTween/Help")]
	static void ShowHelp17 (MenuCommand command) { NGUIHelp.Show(typeof(UIPlayTween)); }

	[MenuItem("CONTEXT/UIPlayAnimation/Help")]
	static void ShowHelp18 (MenuCommand command) { NGUIHelp.Show(typeof(UIPlayAnimation)); }

	[MenuItem("CONTEXT/UIPlaySound/Help")]
	static void ShowHelp19 (MenuCommand command) { NGUIHelp.Show(typeof(UIPlaySound)); }

	[MenuItem("CONTEXT/UIScrollView/Help")]
	static void ShowHelp20 (MenuCommand command) { NGUIHelp.Show(typeof(UIScrollView)); }

	[MenuItem("CONTEXT/UIDragScrollView/Help")]
	static void ShowHelp21 (MenuCommand command) { NGUIHelp.Show(typeof(UIDragScrollView)); }

	[MenuItem("CONTEXT/UICenterOnChild/Help")]
	static void ShowHelp22 (MenuCommand command) { NGUIHelp.Show(typeof(UICenterOnChild)); }

	[MenuItem("CONTEXT/UICenterOnClick/Help")]
	static void ShowHelp23 (MenuCommand command) { NGUIHelp.Show(typeof(UICenterOnClick)); }

	[MenuItem("CONTEXT/UITweener/Help")]
	[MenuItem("CONTEXT/UIPlayTween/Help")]
	static void ShowHelp24 (MenuCommand command) { NGUIHelp.Show(typeof(UITweener)); }

	[MenuItem("CONTEXT/ActiveAnimation/Help")]
	[MenuItem("CONTEXT/UIPlayAnimation/Help")]
	static void ShowHelp25 (MenuCommand command) { NGUIHelp.Show(typeof(UIPlayAnimation)); }

	[MenuItem("CONTEXT/UIScrollView/Help")]
	[MenuItem("CONTEXT/UIDragScrollView/Help")]
	static void ShowHelp26 (MenuCommand command) { NGUIHelp.Show(typeof(UIScrollView)); }

	[MenuItem("CONTEXT/UIPanel/Help")]
	static void ShowHelp27 (MenuCommand command) { NGUIHelp.Show(typeof(UIPanel)); }

	public delegate UIWidget AddFunc (GameObject go);

	static BetterList<string> mEntries = new BetterList<string>();
	static GenericMenu mMenu;

	/// <summary>
	/// Clear the context menu list.
	/// </summary>

	static public void Clear ()
	{
		mEntries.Clear();
		mMenu = null;
	}

	/// <summary>
	/// Add a new context menu entry.
	/// </summary>

	static public void AddItem (string item, bool isChecked, GenericMenu.MenuFunction2 callback, object param)
	{
		if (callback != null)
		{
			if (mMenu == null) mMenu = new GenericMenu();
			int count = 0;

			for (int i = 0; i < mEntries.size; ++i)
			{
				string str = mEntries[i];
				if (str == item) ++count;
			}
			mEntries.Add(item);

			if (count > 0) item += " [" + count + "]";
			mMenu.AddItem(new GUIContent(item), isChecked, callback, param);
		}
		else AddDisabledItem(item);
	}

	/// <summary>
	/// Wrapper function called by the menu that in turn calls the correct callback.
	/// </summary>

	static void AddChild (object obj)
	{
		AddFunc func = obj as AddFunc;
		UIWidget widget = func(Selection.activeGameObject);
		if (widget != null) Selection.activeGameObject = widget.gameObject;
	}

	/// <summary>
	/// Add a new context menu entry.
	/// </summary>

	static void AddChildWidget (string item, bool isChecked, AddFunc callback)
	{
		if (callback != null)
		{
			if (mMenu == null) mMenu = new GenericMenu();
			int count = 0;

			for (int i = 0; i < mEntries.size; ++i)
			{
				string str = mEntries[i];
				if (str == item) ++count;
			}
			mEntries.Add(item);

			if (count > 0) item += " [" + count + "]";
			mMenu.AddItem(new GUIContent(item), isChecked, AddChild, callback);
		}
		else AddDisabledItem(item);
	}

	/// <summary>
	/// Wrapper function called by the menu that in turn calls the correct callback.
	/// </summary>

	static void AddSibling (object obj)
	{
		AddFunc func = obj as AddFunc;
		UIWidget widget = func(Selection.activeTransform.parent.gameObject);
		if (widget != null) Selection.activeGameObject = widget.gameObject;
	}

	/// <summary>
	/// Add a new context menu entry.
	/// </summary>

	static void AddSiblingWidget (string item, bool isChecked, AddFunc callback)
	{
		if (callback != null)
		{
			if (mMenu == null) mMenu = new GenericMenu();
			int count = 0;

			for (int i = 0; i < mEntries.size; ++i)
			{
				string str = mEntries[i];
				if (str == item) ++count;
			}
			mEntries.Add(item);

			if (count > 0) item += " [" + count + "]";
			mMenu.AddItem(new GUIContent(item), isChecked, AddSibling, callback);
		}
		else AddDisabledItem(item);
	}

	/// <summary>
	/// Add commonly NGUI context menu options.
	/// </summary>

	static public void AddCommonItems (GameObject target)
	{
		if (target != null)
		{
			UIWidget widget = target.GetComponent<UIWidget>();

			string myName = string.Format("Selected {0}", (widget != null) ? NGUITools.GetTypeName(widget) : "Object");

			AddItem(myName + "/Bring to Front", false,
				delegate(object obj) { NGUITools.BringForward(Selection.activeGameObject); }, null);

			AddItem(myName + "/Push to Back", false,
				delegate(object obj) { NGUITools.PushBack(Selection.activeGameObject); }, null);

			AddItem(myName + "/Nudge Forward", false,
				delegate(object obj) { NGUITools.AdjustDepth(Selection.activeGameObject, 1); }, null);

			AddItem(myName + "/Nudge Back", false,
				delegate(object obj) { NGUITools.AdjustDepth(Selection.activeGameObject, -1); }, null);

			if (widget != null)
			{
				NGUIContextMenu.AddSeparator(myName + "/");

				AddItem(myName + "/Make Pixel-Perfect", false, OnMakePixelPerfect, Selection.activeTransform);

				if (target.GetComponent<BoxCollider>() != null)
				{
					AddItem(myName + "/Reset Collider Size", false, OnBoxCollider, target);
				}
			}

			NGUIContextMenu.AddSeparator(myName + "/");
			AddItem(myName + "/Delete", false, OnDelete, target);
			NGUIContextMenu.AddSeparator("");

			if (Selection.activeTransform.parent != null && widget != null)
			{
				AddChildWidget("Create/Sprite/Child", false, NGUISettings.AddSprite);
				AddChildWidget("Create/Label/Child", false, NGUISettings.AddLabel);
				AddChildWidget("Create/Invisible Widget/Child", false, NGUISettings.AddWidget);
				AddChildWidget("Create/Simple Texture/Child", false, NGUISettings.AddTexture);
#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				AddChildWidget("Create/Unity 2D Sprite/Child", false, NGUISettings.Add2DSprite);
#endif
				AddSiblingWidget("Create/Sprite/Sibling", false, NGUISettings.AddSprite);
				AddSiblingWidget("Create/Label/Sibling", false, NGUISettings.AddLabel);
				AddSiblingWidget("Create/Invisible Widget/Sibling", false, NGUISettings.AddWidget);
				AddSiblingWidget("Create/Simple Texture/Sibling", false, NGUISettings.AddTexture);
#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				AddSiblingWidget("Create/Unity 2D Sprite/Sibling", false, NGUISettings.Add2DSprite);
#endif
			}
			else
			{
				AddChildWidget("Create/Sprite", false, NGUISettings.AddSprite);
				AddChildWidget("Create/Label", false, NGUISettings.AddLabel);
				AddChildWidget("Create/Invisible Widget", false, NGUISettings.AddWidget);
				AddChildWidget("Create/Simple Texture", false, NGUISettings.AddTexture);
#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2
				AddChildWidget("Create/Unity 2D Sprite", false, NGUISettings.Add2DSprite);
#endif
			}

			NGUIContextMenu.AddSeparator("Create/");

			AddItem("Create/Panel", false, AddPanel, target);
			AddItem("Create/Scroll View", false, AddScrollView, target);
			AddItem("Create/Grid", false, AddChild<UIGrid>, target);
			AddItem("Create/Table", false, AddChild<UITable>, target);
			AddItem("Create/Anchor (Legacy)", false, AddChild<UIAnchor>, target);

			if (target.GetComponent<UIPanel>() != null)
			{
				if (target.GetComponent<UIScrollView>() == null)
				{
					AddItem("Attach/Scroll View", false, delegate(object obj) { target.AddComponent<UIScrollView>(); }, null);
					NGUIContextMenu.AddSeparator("Attach/");
				}
			}
			else if (target.collider == null)
			{
				AddItem("Attach/Box Collider", false, delegate(object obj) { NGUITools.AddWidgetCollider(target); }, null);
				NGUIContextMenu.AddSeparator("Attach/");
			}

			bool header = false;
			UIScrollView scrollView = NGUITools.FindInParents<UIScrollView>(target);

			if (scrollView != null)
			{
				if (scrollView.GetComponentInChildren<UICenterOnChild>() == null)
				{
					AddItem("Attach/Center Scroll View on Child", false, delegate(object obj) { target.AddComponent<UICenterOnChild>(); }, null);
					header = true;
				}
			}

			if (target.collider != null)
			{
				if (scrollView != null)
				{
					if (target.GetComponent<UIDragScrollView>() == null)
					{
						AddItem("Attach/Drag Scroll View", false, delegate(object obj) { target.AddComponent<UIDragScrollView>(); }, null);
						header = true;
					}

					if (target.GetComponent<UICenterOnClick>() == null && NGUITools.FindInParents<UICenterOnChild>(target) != null)
					{
						AddItem("Attach/Center Scroll View on Click", false, delegate(object obj) { target.AddComponent<UICenterOnClick>(); }, null);
						header = true;
					}
				}

				if (header) NGUIContextMenu.AddSeparator("Attach/");

				AddItem("Attach/Button Script", false, delegate(object obj) { target.AddComponent<UIButton>(); }, null);
				AddItem("Attach/Toggle Script", false, delegate(object obj) { target.AddComponent<UIToggle>(); }, null);
				AddItem("Attach/Slider Script", false, delegate(object obj) { target.AddComponent<UISlider>(); }, null);
				AddItem("Attach/Scroll Bar Script", false, delegate(object obj) { target.AddComponent<UIScrollBar>(); }, null);
				AddItem("Attach/Progress Bar Script", false, delegate(object obj) { target.AddComponent<UISlider>(); }, null);
				AddItem("Attach/Popup List Script", false, delegate(object obj) { target.AddComponent<UIPopupList>(); }, null);
				AddItem("Attach/Input Field Script", false, delegate(object obj) { target.AddComponent<UIInput>(); }, null);
				NGUIContextMenu.AddSeparator("Attach/");
				
				if (target.GetComponent<UIDragResize>() == null) AddItem("Attach/Drag Resize Script", false, delegate(object obj) { target.AddComponent<UIDragResize>(); }, null);

				if (target.GetComponent<UIDragScrollView>() == null)
				{
					for (int i = 0; i < UIPanel.list.size; ++i)
					{
						UIPanel pan = UIPanel.list[i];
						if (pan.clipping == UIDrawCall.Clipping.None) continue;

						UIScrollView dr = pan.GetComponent<UIScrollView>();
						if (dr == null) continue;

						AddItem("Attach/Drag Scroll View", false, delegate(object obj)
						{ target.AddComponent<UIDragScrollView>().scrollView = dr; }, null);

						header = true;
						break;
					}
				}

				AddItem("Attach/Key Binding Script", false, delegate(object obj) { target.AddComponent<UIKeyBinding>(); }, null);

				NGUIContextMenu.AddSeparator("Attach/");

				AddItem("Attach/Play Tween Script", false, delegate(object obj) { target.AddComponent<UIPlayTween>(); }, null);
				AddItem("Attach/Play Animation Script", false, delegate(object obj) { target.AddComponent<UIPlayAnimation>(); }, null);
				AddItem("Attach/Play Sound Script", false, delegate(object obj) { target.AddComponent<UIPlaySound>(); }, null);
			}

			if (widget != null)
			{
				AddMissingItem<TweenAlpha>(target, "Tween/Alpha");
				AddMissingItem<TweenColor>(target, "Tween/Color");
				AddMissingItem<TweenWidth>(target, "Tween/Width");
				AddMissingItem<TweenHeight>(target, "Tween/Height");
			}
			else if (target.GetComponent<UIPanel>() != null)
			{
				AddMissingItem<TweenAlpha>(target, "Tween/Alpha");
			}

			NGUIContextMenu.AddSeparator("Tween/");

			AddMissingItem<TweenPosition>(target, "Tween/Position");
			AddMissingItem<TweenRotation>(target, "Tween/Rotation");
			AddMissingItem<TweenScale>(target, "Tween/Scale");
			AddMissingItem<TweenTransform>(target, "Tween/Transform");

			if (target.GetComponent<AudioSource>() != null)
				AddMissingItem<TweenVolume>(target, "Tween/Volume");

			if (target.GetComponent<Camera>() != null)
			{
				AddMissingItem<TweenFOV>(target, "Tween/Field of View");
				AddMissingItem<TweenOrthoSize>(target, "Tween/Orthographic Size");
			}
		}
	}

	/// <summary>
	/// Helper function.
	/// </summary>

	static void AddMissingItem<T> (GameObject target, string name) where T : MonoBehaviour
	{
		if (target.GetComponent<T>() == null)
			AddItem(name, false, delegate(object obj) { target.AddComponent<T>(); }, null);
	}

	/// <summary>
	/// Helper function for menu creation.
	/// </summary>

	static void AddChild<T> (object obj) where T : MonoBehaviour
	{
		GameObject go = obj as GameObject;
		T t = NGUITools.AddChild<T>(go);
		Selection.activeGameObject = t.gameObject;
	}

	/// <summary>
	/// Helper function for menu creation.
	/// </summary>

	static void AddPanel (object obj)
	{
		GameObject go = obj as GameObject;
		if (go.GetComponent<UIWidget>() != null) go = go.transform.parent.gameObject;
		UIPanel panel = NGUISettings.AddPanel(go);
		Selection.activeGameObject = panel.gameObject;
	}

	/// <summary>
	/// Helper function for menu creation.
	/// </summary>

	static void AddScrollView (object obj)
	{
		GameObject go = obj as GameObject;
		if (go.GetComponent<UIWidget>() != null) go = go.transform.parent.gameObject;
		UIPanel panel = NGUISettings.AddPanel(go);
		panel.clipping = UIDrawCall.Clipping.SoftClip;
		panel.gameObject.AddComponent<UIScrollView>();
		panel.name = "Scroll View";
		Selection.activeGameObject = panel.gameObject;
	}

	/// <summary>
	/// Add help options based on the components present on the specified game object.
	/// </summary>

	static public void AddHelp (GameObject go, bool addSeparator)
	{
		MonoBehaviour[] comps = Selection.activeGameObject.GetComponents<MonoBehaviour>();

		bool addedSomething = false;

		for (int i = 0; i < comps.Length; ++i)
		{
			System.Type type = comps[i].GetType();
			string url = NGUIHelp.GetHelpURL(type);
			
			if (url != null)
			{
				if (addSeparator)
				{
					addSeparator = false;
					AddSeparator("");
				}

				AddItem("Help/" + type, false, delegate(object obj) { Application.OpenURL(url); }, null);
				addedSomething = true;
			}
		}

		if (addedSomething) AddSeparator("Help/");
		AddItem("Help/All Topics", false, delegate(object obj) { NGUIHelp.Show(); }, null);
	}

	static void OnHelp (object obj) { NGUIHelp.Show(obj); }
	static void OnMakePixelPerfect (object obj) { NGUITools.MakePixelPerfect(obj as Transform); }
	static void OnBoxCollider (object obj) { NGUITools.AddWidgetCollider(obj as GameObject); }
	static void OnDelete (object obj)
	{
		GameObject go = obj as GameObject;
		Selection.activeGameObject = go.transform.parent.gameObject;
#if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
		NGUITools.Destroy(go);
#else
		Undo.DestroyObjectImmediate(go);
#endif
	}

	/// <summary>
	/// Add a new disabled context menu entry.
	/// </summary>

	static public void AddDisabledItem (string item)
	{
		if (mMenu == null) mMenu = new GenericMenu();
		mMenu.AddDisabledItem(new GUIContent(item));
	}

	/// <summary>
	/// Add a separator to the menu.
	/// </summary>

	static public void AddSeparator (string path)
	{
		if (mMenu == null) mMenu = new GenericMenu();

		// For some weird reason adding separators on OSX causes the entire menu to be disabled. Wtf?
		if (Application.platform != RuntimePlatform.OSXEditor)
			mMenu.AddSeparator(path);
	}

	/// <summary>
	/// Show the context menu with all the added items.
	/// </summary>

	static public void Show ()
	{
		if (mMenu != null)
		{
			mMenu.ShowAsContext();
			mMenu = null;
			mEntries.Clear();
		}
	}
}
