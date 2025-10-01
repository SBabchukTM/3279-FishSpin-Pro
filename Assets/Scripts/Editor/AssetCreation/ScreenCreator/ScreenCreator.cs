using System.Collections.Generic;
using UI;
using UI.Screens;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Editor.AssetCreation.ScreenCreator
{
    public class ScreenCreator : EditorWindow
    {
        public static ScreenCreator Instance;

        private const string ScreenSavePath = "Assets\\Prefabs\\UI\\Screens";
        private const string ClassSavePath = "Assets\\Scripts\\UI\\Screens";
        private const string StateControllerSavePath = "Assets\\Scripts\\States\\GameStates";
        private const string ScreenNamespace = "UI.Screens";

        [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
        [SerializeField] private GameObject ScreenPrefab;
        [SerializeField] private TextAsset _scriptTemplate;
        [SerializeField] private TextAsset _stateControllerTemplate;
        [FormerlySerializedAs("_gameUIContainer")] [SerializeField] private UIPrefabFactory _uiPrefabFactory;

        public UIPrefabFactory UIPrefabFactory => _uiPrefabFactory;

        public List<string> CachedAssetPaths = new ();

        private ListView _namesListView;
        private ListView _prefabsListView;
        private Toggle _createControllersToggle;

        private bool _createdNewScreen = false;

        private List<GameObject> _prefabs = new();

        private List<string> _screenNames = new();
        [SerializeField] private List<string> _validNames = new ();

        [MenuItem("Tools/Screen Creator")]
        public static void ShowExample()
        {
            ScreenCreator wnd = GetWindow<ScreenCreator>();
            wnd.titleContent = new GUIContent("Screen Creator");
        }

        public void CreateGUI()
        {
            _createdNewScreen = false;

            rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
            FindFields();
        }

        private void OnEnable()
        {
            if(Instance == null)
                Instance = this;
            AssemblyReloadEvents.afterAssemblyReload += ProcessAssemblyReload;
        }


        private void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= ProcessAssemblyReload;
        }

        private void FindFields()
        {
            _namesListView = rootVisualElement.Q<ListView>("ScreenNamesList");
            _prefabsListView = rootVisualElement.Q<ListView>("PrefabsList");
            _createControllersToggle = rootVisualElement.Q<Toggle>("CreateStateControllers");

            _prefabs = new PrefabFinder<BaseScreen>().FindAll();

            _prefabsListView.makeItem = () => new ObjectField();
            _prefabsListView.bindItem = BindGOItem;
            _prefabsListView.itemsSource = _prefabs;

            _screenNames = new();
            _screenNames.Add("");

            _namesListView.itemsSource = _screenNames;
            _namesListView.makeItem = () => new TextField();
            _namesListView.bindItem = BindNameItem;

            rootVisualElement.Q<UnityEngine.UIElements.Button>("CreateScreensButton").clicked += CreateScreen;
        }

        private void BindNameItem(VisualElement element, int index)
        {
            var textField = (TextField)element;
            textField.value = _screenNames[index];
            textField.RegisterValueChangedCallback(evt =>
            {
                _screenNames[index] = evt.newValue;
            });
        }

        private void BindGOItem(VisualElement element, int index)
        {
            var objectField = (ObjectField)element;
            objectField.value = _prefabs[index];
        }

        private void CreateScreen()
        {
            _validNames = new();

            for (int i = 0; i < _screenNames.Count; i++)
            {
                if (ClassNameValidator.IsValid(_screenNames[i]))
                    _validNames.Add(_screenNames[i]);
            }

            for (int i = 0; i < _validNames.Count; i++)
            {
                CreateScreenClass(_validNames[i]);
            }
        }

        private void CreateScreenClass(string name)
        {
            ScriptGenerator.Generate(name, _scriptTemplate.text, ClassSavePath);
            _createdNewScreen = true;

            if (_createControllersToggle.value)
                ScriptGenerator.Generate(name, _stateControllerTemplate.text, StateControllerSavePath, "StateController");
        }

        private void ProcessAssemblyReload()
        {
            if (!_createdNewScreen)
                return;

            _createdNewScreen = false;

            for(int i = 0; i< _validNames.Count; i++)
            {
                CreateNewPrefab(_validNames[i]);
            }
        }

        private void CreateNewPrefab(string className) => CachedAssetPaths.Add(PrefabCreator.Create(className, ScreenNamespace, ScreenPrefab, ScreenSavePath));
    
        public class ScreenSaverPostProcessor : AssetPostprocessor
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
                            UIRegistrationManager.RegisterScreen(Instance.UIPrefabFactory, go);
                        }
                    }
                }
            }
        }
    }
}