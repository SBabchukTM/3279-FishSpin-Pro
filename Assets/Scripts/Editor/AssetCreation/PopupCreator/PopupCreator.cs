using System.Collections.Generic;
using UI;
using UI.Popups;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Editor.AssetCreation.PopupCreator
{
    public class PopupCreator : EditorWindow
    {
        public static PopupCreator Instance;

        private const string PopupSavePath = "Assets\\Prefabs\\UI\\Popups";
        private const string PopupNamespace = "UI.Popups";
        private const string ClassSavePath = "Assets\\Scripts\\UI\\Popups";

        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField] private GameObject PopupPrefab;
        [SerializeField] private TextAsset _scriptTemplate;
        [FormerlySerializedAs("_gameUiContainer")] [SerializeField] private UIPrefabFactory _uiPrefabFactory;
        public UIPrefabFactory UIPrefabFactory => _uiPrefabFactory;
    
        private List<PopupData> _popupsDataList = new();

        private ListView _popupDataListView;
        private ListView _prefabsListView;

        private List<GameObject> _prefabs = new();

        private bool _createdNewPopup = false;

        [SerializeField] private List<PopupData> _validPopups = new();

        public List<string> CachedAssetPaths;

        [MenuItem("Tools/Popup Creator")]
        public static void ShowExample()
        {
            PopupCreator wnd = GetWindow<PopupCreator>();
            wnd.titleContent = new GUIContent("Popup Creator");
        }

        public void CreateGUI()
        {
            _createdNewPopup = false;

            rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
            FindFields();
        }

        private void OnEnable()
        {
            Instance = this;
            AssemblyReloadEvents.afterAssemblyReload += ProcessAssemblyReload;
        }

        private void OnDisable()
        {
            Instance = null;
            AssemblyReloadEvents.afterAssemblyReload -= ProcessAssemblyReload;
        }
    
        private void FindFields()
        {
            _popupsDataList = new();
            _popupsDataList.Add(new());

            _popupDataListView = rootVisualElement.Q<ListView>("PopupNamesList");
            _prefabsListView = rootVisualElement.Q<ListView>("PrefabsList");

            _prefabs = new PrefabFinder<BasePopup>().FindAll();

            _prefabsListView.makeItem = () => new ObjectField();
            _prefabsListView.bindItem = BindGOItem;
            _prefabsListView.itemsSource = _prefabs;

            _popupDataListView.itemsSource = _popupsDataList;
            _popupDataListView.makeItem = () => new CustomPopupDataElement();
            _popupDataListView.bindItem = BindPopupDataElement;

            rootVisualElement.Q<UnityEngine.UIElements.Button>("CreatePopupsButton").clicked += CreatePopup;
        }
    
        private void BindPopupDataElement(VisualElement visualElement, int index)
        {
            var customElement = (CustomPopupDataElement)visualElement;
        
            if(_popupsDataList[index] == null)
                _popupsDataList[index] = new();
        
            customElement.TextField.value = _popupsDataList[index].Name;
            customElement.TextField.RegisterValueChangedCallback(evt =>
            {
                _popupsDataList[index].Name = evt.newValue;
            });
        }

        private void BindGOItem(VisualElement element, int index)
        {
            var objectField = (ObjectField)element;
            objectField.value = _prefabs[index];
        }

        private void CreatePopup()
        {
            _validPopups = new ();

            for (int i = 0; i < _popupsDataList.Count; i++)
            {
                if (ClassNameValidator.IsValid(_popupsDataList[i].Name))
                    _validPopups.Add(_popupsDataList[i]);
            }

            for(int i = 0; i < _validPopups.Count; i++)
            {
                string className = _validPopups[i].Name;
                CreatePopupClass(className);
            }
        }

        private void CreatePopupClass(string className)
        {
            ScriptGenerator.Generate(className, _scriptTemplate.text, ClassSavePath);
            _createdNewPopup = true;
        }
    
        private void ProcessAssemblyReload()
        {
            if (!_createdNewPopup)
                return;

            _createdNewPopup = false;

            for(int i = 0; i < _validPopups.Count;i++) 
                CreateNewPrefab(_validPopups[i].Name);
        }

        private void CreateNewPrefab(string name) => CachedAssetPaths.Add(PrefabCreator.Create(name, PopupNamespace, PopupPrefab, PopupSavePath));
    
        public class PopupSaverPostProcessor : AssetPostprocessor
        {
            public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                if (Instance == null)
                    return;

                if (Instance.CachedAssetPaths == null)
                    return;

                List<string> cachedPaths = Instance.CachedAssetPaths;
                for (int i = 0; i < cachedPaths.Count; i++)
                {
                    string targetPath = cachedPaths[i].Replace("\\", "/");
                    for (int j = 0; j < importedAssets.Length; j++)
                    {
                        if (importedAssets[j] == targetPath)
                        {
                            GameObject go = AssetDatabase.LoadAssetAtPath(targetPath, typeof(GameObject)) as GameObject;
                            UIRegistrationManager.RegisterPopup(Instance.UIPrefabFactory, go);
                        }
                    }
                }
            }
        }
    }
}
