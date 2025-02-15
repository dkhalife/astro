//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIScrollView))]
public class UIScrollViewEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(130f);

		GUILayout.Space(3f);
		serializedObject.Update();

		SerializedProperty sp = NGUIEditorTools.DrawProperty("Movement", serializedObject, "movement");

		if (((UIScrollView.Movement)sp.intValue) == UIScrollView.Movement.Custom)
		{
			NGUIEditorTools.SetLabelWidth(20f);

			GUILayout.BeginHorizontal();
			GUILayout.Space(114f);
			NGUIEditorTools.DrawProperty("X", serializedObject, "customMovement.x", GUILayout.MinWidth(20f));
			NGUIEditorTools.DrawProperty("Y", serializedObject, "customMovement.y", GUILayout.MinWidth(20f));
			GUILayout.EndHorizontal();
		}

		NGUIEditorTools.SetLabelWidth(130f);

		NGUIEditorTools.DrawProperty("Drag Effect", serializedObject, "dragEffect");
		NGUIEditorTools.DrawProperty("Scroll Wheel Factor", serializedObject, "scrollWheelFactor");
		NGUIEditorTools.DrawProperty("Momentum Amount", serializedObject, "momentumAmount");

		NGUIEditorTools.SetLabelWidth(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Reset Position", GUILayout.Width(106f));
		NGUIEditorTools.DrawProperty("X", serializedObject, "relativePositionOnReset.x", GUILayout.MinWidth(20f));
		NGUIEditorTools.DrawProperty("Y", serializedObject, "relativePositionOnReset.y", GUILayout.MinWidth(20f));
		GUILayout.EndHorizontal();

		NGUIEditorTools.SetLabelWidth(130f);

		NGUIEditorTools.DrawProperty("Restrict Within Panel", serializedObject, "restrictWithinPanel");
		NGUIEditorTools.DrawProperty("Cancel Drag If Fits", serializedObject, "disableDragIfFits");
		NGUIEditorTools.DrawProperty("Smooth Drag Start", serializedObject, "smoothDragStart");
		NGUIEditorTools.DrawProperty("IOS Drag Emulation", serializedObject, "iOSDragEmulation");

		NGUIEditorTools.SetLabelWidth(100f);

		if (NGUIEditorTools.DrawHeader("Scroll Bars"))
		{
			NGUIEditorTools.BeginContents();
			NGUIEditorTools.DrawProperty("Horizontal", serializedObject, "horizontalScrollBar");
			NGUIEditorTools.DrawProperty("Vertical", serializedObject, "verticalScrollBar");
			NGUIEditorTools.DrawProperty("Show Condition", serializedObject, "showScrollBars");
			NGUIEditorTools.EndContents();
		}
		serializedObject.ApplyModifiedProperties();
	}
}
