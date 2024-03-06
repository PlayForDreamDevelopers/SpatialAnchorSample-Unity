using UnityEditor;

namespace YVR.Core
{
    [CustomEditor(typeof(YVRCompositeLayer))]
    public class YVRCompositeLayerEditor : UnityEditor.Editor
    {
        private SerializedProperty m_Script;
        private SerializedProperty m_Shape;
        private SerializedProperty m_CircleSegments;
        private SerializedProperty m_CylinderAngle;
        private SerializedProperty m_Equirect2Angle;
        private SerializedProperty m_Radius;
        private SerializedProperty useFixedResolution;
        private SerializedProperty fixedResolutionWidth;
        private SerializedProperty fixedResolutionHeight;

        private void OnEnable()
        {
            m_Script = serializedObject.FindProperty("m_Script");
            m_Shape = serializedObject.FindProperty("m_Shape");
            m_CircleSegments = serializedObject.FindProperty("m_CircleSegments");
            m_CylinderAngle = serializedObject.FindProperty("m_CylinderAngle");
            m_Equirect2Angle = serializedObject.FindProperty("m_Equirect2Angle");
            m_Radius = serializedObject.FindProperty("m_Radius");
            useFixedResolution = serializedObject.FindProperty("useFixedResolution");
            fixedResolutionWidth = serializedObject.FindProperty("fixedResolutionWidth");
            fixedResolutionHeight = serializedObject.FindProperty("fixedResolutionHeight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Shape);
            YVRRenderLayerType layerShape = (YVRRenderLayerType)m_Shape.intValue;
            if (layerShape == YVRRenderLayerType.Cylinder)
            {
                EditorGUILayout.PropertyField(m_CylinderAngle);
                EditorGUILayout.PropertyField(m_CircleSegments);
            }
            else if (layerShape == YVRRenderLayerType.Equirect)
            {
                EditorGUILayout.PropertyField(m_Radius);
            }
            else if (layerShape == YVRRenderLayerType.Equirect2)
            {
                EditorGUILayout.PropertyField(m_Radius);
                EditorGUILayout.PropertyField(m_Equirect2Angle);
            }

            DrawPropertiesExcluding(serializedObject, m_Script.propertyPath, m_Shape.propertyPath, m_CircleSegments.propertyPath, m_CylinderAngle.propertyPath,
                m_Equirect2Angle.propertyPath, m_Radius.propertyPath, useFixedResolution.propertyPath, fixedResolutionWidth.propertyPath, fixedResolutionHeight.propertyPath);

            EditorGUILayout.PropertyField(useFixedResolution);
            if (useFixedResolution.boolValue)
            {
                EditorGUILayout.PropertyField(fixedResolutionWidth);
                EditorGUILayout.PropertyField(fixedResolutionHeight);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
