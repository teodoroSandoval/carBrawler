using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ButtonManager))]
public class inputConfig : Editor {

    public enum AxisType {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name) {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do {
            if (child.name == name) return child;
        }
        while (child.Next(false));
        return null;
    }

    private static bool AxisDefined(string axisName) {
        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

        axesProperty.Next(true);
        axesProperty.Next(true);
        while (axesProperty.Next(false)) {
            SerializedProperty axis = axesProperty.Copy();
            axis.Next(true);
            if (axis.stringValue == axisName) return true;
        }
        return false;
    }

    public class InputAxis {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;

        public float gravity;
        public float dead;
        public float sensitivity;

        public bool snap = false;
        public bool invert = false;

        public AxisType type;

        public int axis;
        public int joyNum;
    }

    private static void AddAxis(InputAxis axis) {
        if (AxisDefined(axis.name)) return;

        SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");
        
        axesProperty.arraySize++;
        serializedObject.ApplyModifiedProperties();

        SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);

        GetChildProperty(axisProperty, "m_Name").stringValue = axis.name;
        GetChildProperty(axisProperty, "descriptiveName").stringValue = axis.descriptiveName;
        GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
        GetChildProperty(axisProperty, "negativeButton").stringValue = axis.negativeButton;
        GetChildProperty(axisProperty, "positiveButton").stringValue = axis.positiveButton;
        GetChildProperty(axisProperty, "altNegativeButton").stringValue = axis.altNegativeButton;
        GetChildProperty(axisProperty, "altPositiveButton").stringValue = axis.altPositiveButton;
        GetChildProperty(axisProperty, "gravity").floatValue = axis.gravity;
        GetChildProperty(axisProperty, "dead").floatValue = axis.dead;
        GetChildProperty(axisProperty, "sensitivity").floatValue = axis.sensitivity;
        GetChildProperty(axisProperty, "snap").boolValue = axis.snap;
        GetChildProperty(axisProperty, "invert").boolValue = axis.invert;
        GetChildProperty(axisProperty, "type").intValue = (int)axis.type;
        GetChildProperty(axisProperty, "axis").intValue = axis.axis - 1;
        GetChildProperty(axisProperty, "joyNum").intValue = axis.joyNum;

        serializedObject.ApplyModifiedProperties();
    }

    public static float numberOfControllers = 8;
    public static void SetupInputManager() {
        // Add gamepad definitions
        
        for (int i = 1; i <= numberOfControllers; i++) {

            AddAxis(new InputAxis() {
                name = "P" + i + "_LeftHor",
                gravity = 0,
                dead = 0.1f,
                sensitivity = 1f,
                type = AxisType.JoystickAxis,
                axis = 1,
                joyNum = i,
            });
            AddAxis(new InputAxis() {
                name = "P" + i + "_LeftVer",
                gravity = 0,
                dead = 0.1f,
                sensitivity = 1f,
                type = AxisType.JoystickAxis,
                axis = 2,
                joyNum = i,
            });

            AddAxis(new InputAxis() {
                name = "P" + i + "_RightHor",
                gravity = 1,
                dead = 0.001f,
                sensitivity = 1,
                type = AxisType.JoystickAxis,
                axis = 4,
                joyNum = i,
            });
            AddAxis(new InputAxis() {
                name = "P" + i + "_RightVer",
                gravity = 1,
                dead = 0.001f,
                sensitivity = 1,
                type = AxisType.JoystickAxis,
                axis = 5,
                joyNum = i,
            });
            AddAxis(new InputAxis() {
                name = "P" + i + "_Acceleration",
                gravity = 1,
                dead = 0.001f,
                sensitivity = 1,
                type = AxisType.JoystickAxis,
                axis = 3,
                joyNum = i,
                invert = true
            });


            AddAxis(new InputAxis() {
                name = "P" + i + "_Jump",
                positiveButton = "joystick "+ i + " button 0",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });

            AddAxis(new InputAxis() {
                name = "P" + i + "_Fire1",
                positiveButton = "joystick " + i + " button 1",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });
            AddAxis(new InputAxis() {
                name = "P" + i + "_Fire2",
                positiveButton = "joystick " + i + " button 2",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });
            AddAxis(new InputAxis() {
                name = "P" + i + "_Fire3",
                positiveButton = "joystick " + i + " button 3",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });

            AddAxis(new InputAxis() {
                name = "P" + i + "_Launch1",
                positiveButton = "joystick " + i + " button 4",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });

            AddAxis(new InputAxis() {
                name = "P" + i + "_Launch2",
                positiveButton = "joystick " + i + " button 5",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });


            AddAxis(new InputAxis() {
                name = "P" + i + "_Select",
                positiveButton = "joystick " + i + " button 6",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });


            AddAxis(new InputAxis() {
                name = "P" + i + "_Start",
                positiveButton = "joystick " + i + " button 7",
                gravity = 1000,
                dead = 0.001f,
                sensitivity = 1000,
                type = AxisType.KeyOrMouseButton,
                joyNum = i,
            });
        }
        AddAxis(new InputAxis() {
            name = "Submit",
            positiveButton = "joystick 1 button 7",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000,
            type = AxisType.KeyOrMouseButton,
            joyNum = 1,
        });
        AddAxis(new InputAxis() {
            name = "Cancel",
            positiveButton = "joystick 1 button 6",
            gravity = 1000,
            dead = 0.001f,
            sensitivity = 1000,
            type = AxisType.KeyOrMouseButton,
            joyNum = 1,
        });
    }

    //public bool isRun = true;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();


        if (GUILayout.Button("Set Controllers input")) {
            SetupInputManager();
        }
    }

}
