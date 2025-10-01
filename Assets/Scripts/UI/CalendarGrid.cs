using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class CalendarGrid : LayoutGroup
    {
        public int columns = 7; // Default for a calendar week

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            int childCount = rectChildren.Count;

            float totalWidth = rectTransform.rect.width;
            float cellSize = totalWidth / columns;

            for (int i = 0; i < childCount; i++)
            {
                int row = i / columns;
                int col = i % columns;

                float x = cellSize * col;
                float y = (cellSize * row); // Negative to go downward

                SetChildAlongAxis(rectChildren[i], 0, x, cellSize);
                SetChildAlongAxis(rectChildren[i], 1, y, cellSize);
            }

            // Set layout height to fit all rows
            int rowCount = Mathf.CeilToInt((float)childCount / columns);
            float totalHeight = cellSize * rowCount;
            SetLayoutInputForAxis(totalHeight, totalHeight, -1, 1);
        }

        public override void CalculateLayoutInputVertical()
        {
            // Required override; handled in CalculateLayoutInputHorizontal
        }

        public override void SetLayoutHorizontal()
        {
            CalculateLayoutInputHorizontal();
        }

        public override void SetLayoutVertical()
        {
            CalculateLayoutInputHorizontal();
        }
    }
}
