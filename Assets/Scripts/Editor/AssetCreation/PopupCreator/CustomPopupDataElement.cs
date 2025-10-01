using UnityEngine.UIElements;

namespace Editor.AssetCreation.PopupCreator
{
    public class CustomPopupDataElement : VisualElement
    {
        public TextField TextField { get; private set; }

        public string PopupName => TextField.text;

        public CustomPopupDataElement()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.FlexStart;

            TextField = new TextField { style = 
                { 
                    width = 295 
                } 
            };
            Add(TextField);
        }
    }
}