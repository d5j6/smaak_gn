﻿#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Slate{

	///A popup window to select a camera shot with a preview
	public class PreferencesWindow : PopupWindowContent {

		private static Rect myRect;
		private bool firstPass = true;
		
		///Shows the popup menu at position and with title
		public static void Show(Rect rect){
			myRect = rect;
			PopupWindow.Show( new Rect(rect.x, rect.y, 0, 0), new PreferencesWindow() );
		}

		public override Vector2 GetWindowSize(){ return new Vector2(myRect.width, myRect.height); }
		public override void OnGUI(Rect rect){

			GUILayout.BeginVertical("box");

			GUI.color = new Color(0,0,0,0.3f);
			GUILayout.BeginHorizontal(Slate.Styles.headerBoxStyle);
			GUI.color = Color.white;
			GUILayout.Label("<size=22><b>Global Editor Preferences</b></size>");
			GUILayout.EndHorizontal();
			GUILayout.Space(2);

			GUILayout.BeginVertical("box");
			Prefs.timeStepMode = (Prefs.TimeStepMode)EditorGUILayout.EnumPopup("Time Step Mode", Prefs.timeStepMode);
			if (Prefs.timeStepMode == Prefs.TimeStepMode.Seconds){
				Prefs.snapInterval = EditorTools.CleanPopup<float>("Working Snap Interval", Prefs.snapInterval, Prefs.snapIntervals.ToList());
			} else {
				Prefs.frameRate = EditorTools.CleanPopup<int>("Working Frame Rate", Prefs.frameRate, Prefs.frameRates.ToList());
			}
			GUILayout.EndVertical();

			GUILayout.BeginVertical("box");
			Prefs.lockHorizontalCurveEditing = EditorGUILayout.Toggle(new GUIContent("Lock xAxis Curve Editing", "Disallows moving keys in x in CurveEditor. They can still be moved in DopeSheetEditor"), Prefs.lockHorizontalCurveEditing);
			Prefs.showDopesheetKeyValues     = EditorGUILayout.Toggle("Show Keyframe Values", Prefs.showDopesheetKeyValues);
			Prefs.defaultTangentMode         = (TangentMode)EditorGUILayout.EnumPopup("Initial Keyframe Tangent", Prefs.defaultTangentMode);
			Prefs.keyframesStyle             = (Prefs.KeyframesStyle)EditorGUILayout.EnumPopup("Keyframes Style", Prefs.keyframesStyle);
			GUILayout.EndVertical();

			GUILayout.BeginVertical("box");
			Prefs.showShotThumbnails = EditorGUILayout.Toggle("Show Shot Thumbnails", Prefs.showShotThumbnails);
			if (Prefs.showShotThumbnails){
				Prefs.thumbnailsRefreshInterval = EditorGUILayout.IntSlider(new GUIContent("Thumbnails Refresh", "The interval between which thumbnails refresh in editor frames"), Prefs.thumbnailsRefreshInterval, 2, 100);
			}
			Prefs.showRuleOfThirds = EditorGUILayout.Toggle(new GUIContent("Show Rule Of Thirds"), Prefs.showRuleOfThirds);
			GUILayout.EndVertical();

			GUILayout.BeginVertical("box");
			Prefs.scrollWheelZooms       = EditorGUILayout.Toggle("Scroll Wheel Zooms", Prefs.scrollWheelZooms);
			Prefs.showDescriptions       = EditorGUILayout.Toggle("Show Help Descriptions", Prefs.showDescriptions);
			Prefs.gizmosLightness        = EditorGUILayout.Slider("Gizmos Lightness", Prefs.gizmosLightness, 0, 1);
			Prefs.motionPathsColor       = EditorGUILayout.ColorField("Motion Paths Color", Prefs.motionPathsColor);
			GUILayout.EndVertical();


			GUI.backgroundColor = Prefs.autoKey? new Color(1,0.5f,0.5f) : Color.white;
			if ( GUILayout.Button( new GUIContent(Prefs.autoKey? "AUTOKEY IS ENABLED" : "AUTOKEY IS DISABLED", Styles.keyIcon) ) ){
				Prefs.autoKey = !Prefs.autoKey;
			}
			GUI.backgroundColor = Color.white;


			GUILayout.EndVertical();

			if (firstPass || Event.current.type == EventType.Repaint){
				firstPass = false;
				myRect.height = GUILayoutUtility.GetLastRect().yMax + 5;
			}
		}
	}
}

#endif