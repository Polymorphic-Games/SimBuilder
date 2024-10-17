using geniikw.DataRenderer2D;
using UnityEngine;

public class UICurveController : MonoBehaviour {

    [SerializeField] RectTransform startTransform;
    [SerializeField] RectTransform midTransform;
    [SerializeField] RectTransform endTransform;

    RectTransform canvasRect;
    float canvasLocalScale;

    [SerializeField] UILine startLine;
    [SerializeField] UILine endLine;

    [SerializeField] float lineWidth = 5f;

    Vector3 startPos;
    Vector3 midPos;
    Vector3 endPos;

    float startHandleDistance;
    float endHandleDistance;

    Vector3 handle0;
    Vector3 handle1;
    Vector3 midOffset;
    Vector3 handle2;
    Vector3 handle3;

    void Start() {
        canvasRect = transform.root.GetComponent<RectTransform>();
    }

    void Update() {
        canvasLocalScale = canvasRect.localScale.x;
        startPos = startTransform.position;
        midPos = midTransform.position;
        endPos = endTransform.position;

        startHandleDistance = (midPos - startPos).magnitude / 3f;
        endHandleDistance = (endPos - midPos).magnitude / 3f;

        handle0 = startPos + transform.right * startHandleDistance;
        handle3 = endPos - transform.right * endHandleDistance;

        midOffset = (handle0 - handle3).normalized;

        startLine.line.EditPoint(0, startPos, Vector3.right * startHandleDistance / canvasLocalScale, Vector3.zero, lineWidth);
        startLine.line.EditPoint(1, midPos, Vector3.zero, midOffset * startHandleDistance / canvasLocalScale, lineWidth);
        endLine.line.EditPoint(0, endPos, -Vector3.right * endHandleDistance / canvasLocalScale, Vector3.zero, lineWidth);
        endLine.line.EditPoint(1, midPos, Vector3.zero, -midOffset * endHandleDistance / canvasLocalScale, lineWidth);



        //debug
        /*
        handle1 = midPos + midOffset * startHandleDistance;
        handle2 = midPos - midOffset * endHandleDistance;
        Debug.DrawLine(startPos, handle0, Color.red);
        Debug.DrawLine(handle0, handle1, Color.yellow);
        Debug.DrawLine(handle1, midPos, Color.green);
        Debug.DrawLine(midPos, handle2, Color.cyan);
        Debug.DrawLine(handle2, handle3, Color.blue);
        Debug.DrawLine(handle3, endPos, Color.magenta);
        */
    }
}