using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScrollview : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private RectTransform content;
    [SerializeField] private GridLayoutGroup layoutGroup;

    [SerializeField] private float columnCount = 2;

    private Vector2 lastVelocity = Vector2.zero;
    private bool velocityUpdated;
    public List<RectTransform> buttons;

    void Start()
    {
        int count = Mathf.CeilToInt(viewPort.rect.height / (layoutGroup.cellSize.y + layoutGroup.spacing.y))* (int)columnCount;

        for (int i = 0; i < count; i++)
        {
            var rect = Instantiate(buttons[i % buttons.Count], content);
            rect.SetAsLastSibling();
        }

        for (int i = 0; i < count; i++)
        {
            var index = (buttons.Count-1) - (i% buttons.Count);
            if(index < 0)
            {
                index = buttons.Count-1;
            }
            var rect = Instantiate(buttons[index], content);
            rect.SetAsFirstSibling();
        }
        var y = (layoutGroup.cellSize.y + layoutGroup.spacing.y)* (float)count / columnCount;
        content.localPosition = new Vector3(content.localPosition.x, y, content.localPosition.z);   
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        if (velocityUpdated)
        {
            velocityUpdated = false;
            scrollRect.velocity = lastVelocity;
        }
        if(content.localPosition.y <= 0.1f)
        {
            velocityUpdated = true;
            Canvas.ForceUpdateCanvases();
            lastVelocity = scrollRect.velocity;
            var y = (buttons.Count/ columnCount) * (layoutGroup.cellSize.y + layoutGroup.spacing.y);
            content.localPosition += Vector3.up * y;
        }

        var canvasH = canvas.pixelRect.height * (1 / canvas.scaleFactor);
        if (content.localPosition.y >= content.rect.height - canvasH - 0.5f)
        {
            velocityUpdated = true;
            Canvas.ForceUpdateCanvases();
            lastVelocity = scrollRect.velocity;
            var y = (buttons.Count / columnCount) * (layoutGroup.cellSize.y + layoutGroup.spacing.y);
            content.localPosition -= Vector3.up * y;
        }
    }
}
