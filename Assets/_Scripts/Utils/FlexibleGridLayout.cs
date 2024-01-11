using UnityEngine;
using UnityEngine.UI;

public class FlexibleGridLayout : LayoutGroup
{
    public int rows;
    public int cols;
    public Vector2 cellSize;

    // more support for spacing and padding
    public Vector2 spacing;


    //  ref: https://www.youtube.com/watch?v=CGsEJToeXmA
    public override void CalculateLayoutInputVertical()
    {
        base.CalculateLayoutInputHorizontal();

        float sqrRt = Mathf.Sqrt(transform.childCount);
        rows = Mathf.CeilToInt(sqrRt);
        cols = Mathf.CeilToInt(sqrRt);

        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;

        // the minus is to reduce our elements with the spacing we want to use, so the 'children will remain within the transform'
        // we also do the same to substract the padding left and right, width uses left and right padding, height uses left ands right padding
        float cellWidth = parentWidth / (float)cols - ((spacing.x / (float)cols) * 2) - (padding.left / (float)cols) - (padding.right / (float)cols);
        float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * 2) - (padding.top / (float)cols) - (padding.bottom / (float)cols);

        cellSize.y = cellHeight;
        cellSize.x = cellWidth;

        int columnCount = 0;
        int rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            columnCount = i % cols;
            rowCount = i / cols;

            var item = rectChildren[i];

            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            var yPos = (cellSize.x * rowCount) + (spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
        }

    }

    public override void SetLayoutHorizontal()
    {
        return;
    }

    public override void SetLayoutVertical()
    {
        return;
    }

}
