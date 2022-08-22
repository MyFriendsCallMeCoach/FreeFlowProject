using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(FreeflowCombat))]
public class FreeflowCombatEditor : Editor
{
    string[] tabs = {"General", "Attack", "Traversal"};
    int tabSelected = -1;

    // variables
    SerializedProperty autoGetCamera,
    camera,
    xInput,
    yInput,
    enemyLayers,
    detectionRadius,
    showDetectionRadius,
    accuracySize,
    showAccuracy,
    idleAnimName,
    idleAnimTSpeed,
    scriptsToDisable,

    attackAnimations,
    attackAnimsTSpeed,
    randomizeAttackAnim,

    traversalTime,
    useTraversalAnimations,
    applyTraversalAnimDistance,
    traversalAnimations,
    traversalAnimsTSpeed,
    randomizeTraversalAnim,
    maintainYPosition;

    void OnEnable()
    {
        tabSelected = 0;

        // general
        autoGetCamera = serializedObject.FindProperty("autoGetCamera");
        camera = serializedObject.FindProperty("camera");
        xInput = serializedObject.FindProperty("xInput");
        yInput = serializedObject.FindProperty("yInput");
        enemyLayers = serializedObject.FindProperty("enemyLayers");
        detectionRadius = serializedObject.FindProperty("detectionRadius");
        showDetectionRadius = serializedObject.FindProperty("showDetectionRadius");
        accuracySize = serializedObject.FindProperty("accuracySize");
        showAccuracy = serializedObject.FindProperty("showAccuracy");
        idleAnimName = serializedObject.FindProperty("idleAnimName");
        idleAnimTSpeed = serializedObject.FindProperty("idleAnimTSpeed");
        scriptsToDisable = serializedObject.FindProperty("scriptsToDisable");

        // attack
        attackAnimations = serializedObject.FindProperty("attackAnimations");
        attackAnimsTSpeed = serializedObject.FindProperty("attackAnimsTSpeed");
        randomizeAttackAnim = serializedObject.FindProperty("randomizeAttackAnim");

        // traversal
        traversalTime = serializedObject.FindProperty("traversalTime");
        useTraversalAnimations = serializedObject.FindProperty("useTraversalAnimations");
        applyTraversalAnimDistance = serializedObject.FindProperty("applyTraversalAnimDistance");
        traversalAnimations = serializedObject.FindProperty("traversalAnimations");
        traversalAnimsTSpeed = serializedObject.FindProperty("traversalAnimsTSpeed");
        randomizeTraversalAnim = serializedObject.FindProperty("randomizeTraversalAnim");
        maintainYPosition = serializedObject.FindProperty("maintainYPosition");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();
            tabSelected = GUILayout.Toolbar(tabSelected, tabs, ToolbarStyle());
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Hover on any property below for more details");

        FreeflowCombat script = (FreeflowCombat)target;

        switch (tabSelected) {
            case 0:
                GeneralTab(script);
                break;
            case 1:
                AttackTab();
                break;
            case 2:
                TraversalTab(script);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    // render the general tab contents
    void GeneralTab(FreeflowCombat script)
    {
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(autoGetCamera);

        if (!script.autoGetCamera) EditorGUILayout.PropertyField(camera);
        EditorGUILayout.Space(3);

        EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(xInput);
            EditorGUILayout.PropertyField(yInput);
            EditorGUILayout.Space(3);
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.PropertyField(enemyLayers);
        EditorGUILayout.PropertyField(detectionRadius);
        EditorGUILayout.PropertyField(showDetectionRadius);
        EditorGUILayout.Space(3);

        EditorGUILayout.PropertyField(accuracySize);
        EditorGUILayout.PropertyField(showAccuracy);
        EditorGUILayout.Space(3);

        EditorGUILayout.PropertyField(idleAnimName);
        EditorGUILayout.PropertyField(idleAnimTSpeed);
        EditorGUILayout.Space(3);

        EditorGUILayout.PropertyField(scriptsToDisable);
    }

    // render the attack tab contents
    void AttackTab()
    {
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(attackAnimations);
        EditorGUILayout.PropertyField(attackAnimsTSpeed);
        EditorGUILayout.PropertyField(randomizeAttackAnim);
    }

    // render the traversal tab contents
    void TraversalTab(FreeflowCombat script)
    {
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(traversalTime);
        EditorGUILayout.PropertyField(useTraversalAnimations);
        EditorGUILayout.Space(3);
        
        if (script.useTraversalAnimations) {
            EditorGUILayout.PropertyField(traversalAnimations);
            EditorGUILayout.PropertyField(traversalAnimsTSpeed);
            
            EditorGUILayout.Space(3);
            
            EditorGUILayout.PropertyField(applyTraversalAnimDistance);
            EditorGUILayout.PropertyField(randomizeTraversalAnim);
            EditorGUILayout.PropertyField(maintainYPosition);
        }
    }

    // style the toolbar
    GUIStyle ToolbarStyle()
    {
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;
        style.hover.textColor = new Color(1f, 0.6537189f, 0.04245278f, 1);
        style.fixedHeight = 40;
        
        style.active.textColor = new Color(1f, 0.6537189f, 0.04245278f, 1);

        return style;
    }
}
