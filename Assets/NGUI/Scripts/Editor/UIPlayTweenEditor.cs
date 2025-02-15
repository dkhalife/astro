//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIPlayTween))]
public class UIPlayTweenEditor : Editor
{
	enum ResetOnPlay
	{
		Continue,
		Restart,
		RestartIfNotPlaying,
	}

	enum SelectedObject
	{
		KeepCurrent,
		SetToNothing,
	}

	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.SetLabelWidth(120f);
		UIPlayTween tw = target as UIPlayTween;
		GUILayout.Space(6f);

		GUI.changed = false;
		GameObject tt = (GameObject)EditorGUILayout.ObjectField("Tween Target", tw.tweenTarget, typeof(GameObject), true);

		bool inc = EditorGUILayout.Toggle("Include Children", tw.includeChildren);
		int group = EditorGUILayout.IntField("Tween Group", tw.tweenGroup, GUILayout.Width(160f));

		AnimationOrTween.Trigger trigger = (AnimationOrTween.Trigger)EditorGUILayout.EnumPopup("Trigger condition", tw.trigger);
		AnimationOrTween.Direction dir = (AnimationOrTween.Direction)EditorGUILayout.EnumPopup("Play direction", tw.playDirection);
		AnimationOrTween.EnableCondition enab = (AnimationOrTween.EnableCondition)EditorGUILayout.EnumPopup("If target is disabled", tw.ifDisabledOnPlay);
		ResetOnPlay rs = tw.resetOnPlay ? ResetOnPlay.Restart : (tw.resetIfDisabled ? ResetOnPlay.RestartIfNotPlaying : ResetOnPlay.Continue);
		ResetOnPlay reset = (ResetOnPlay)EditorGUILayout.EnumPopup("If already playing", rs);
		AnimationOrTween.DisableCondition dis = (AnimationOrTween.DisableCondition)EditorGUILayout.EnumPopup("When finished", tw.disableWhenFinished);

		if (GUI.changed)
		{
			NGUIEditorTools.RegisterUndo("Tween Change", tw);
			tw.tweenTarget = tt;
			tw.tweenGroup = group;
			tw.includeChildren = inc;
			tw.trigger = trigger;
			tw.playDirection = dir;
			tw.ifDisabledOnPlay = enab;
			tw.resetOnPlay = (reset == ResetOnPlay.Restart);
			tw.resetIfDisabled = (reset == ResetOnPlay.RestartIfNotPlaying);
			tw.disableWhenFinished = dis;
			UnityEditor.EditorUtility.SetDirty(tw);
		}

		NGUIEditorTools.SetLabelWidth(80f);
		NGUIEditorTools.DrawEvents("On Finished", tw, tw.onFinished);
	}
}
