using UI;
using UI.Popups;
using UI.Screens;
using UnityEditor;
using UnityEngine;

namespace Editor.AssetCreation
{
    public class UIRegistrationManager
    {
        public static void RegisterScreen(UIPrefabFactory uiPrefabFactory, GameObject prefabAsset)
        {
            SerializedObject serializedUiServiceViewContainer = new SerializedObject(uiPrefabFactory);

            BaseScreen uiScreenComponent = prefabAsset.GetComponent<BaseScreen>();

            SerializedProperty screensPrefabProperty = serializedUiServiceViewContainer.FindProperty("_screens");

            screensPrefabProperty.arraySize++;
            screensPrefabProperty.GetArrayElementAtIndex(screensPrefabProperty.arraySize - 1).objectReferenceValue = uiScreenComponent;

            serializedUiServiceViewContainer.ApplyModifiedProperties();
        }
        
        public static void RegisterPopup(UIPrefabFactory uiPrefabFactory, GameObject prefabAsset)
        {
            SerializedObject serializedUiServiceViewContainer = new SerializedObject(uiPrefabFactory);

            BasePopup basePopupComponent = prefabAsset.GetComponent<BasePopup>();

            SerializedProperty popupsPrefabProperty = serializedUiServiceViewContainer.FindProperty("_popups");

            popupsPrefabProperty.arraySize++;
            popupsPrefabProperty.GetArrayElementAtIndex(popupsPrefabProperty.arraySize - 1).objectReferenceValue = basePopupComponent;

            serializedUiServiceViewContainer.ApplyModifiedProperties();
        }
    }
}