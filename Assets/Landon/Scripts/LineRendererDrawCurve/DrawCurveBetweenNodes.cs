using UnityEngine;
using System.Collections.Generic;

public class DrawCurveBetweenNodes : MonoBehaviour {

    [SerializeField]
    RectTransform startRect;
    [SerializeField]
    RectTransform endRect;
    LineRenderer lr;


    public int pointCount = 32;
    public float curveDistance = 100f;

    List<Vector3> points;


    void Start() {
        lr = GetComponent<LineRenderer>();
	}

    void Update() {
        DrawCurveBetweenRects(startRect,endRect);

    }

    void DrawCurveBetweenRects(RectTransform start, RectTransform end) {

        Vector3 startPoint = transform.InverseTransformPoint(start.position);
        Vector3 endPoint = transform.InverseTransformPoint(end.position);
        Vector3 startHandle = startPoint + transform.right * curveDistance;
        Vector3 endHandle = endPoint - transform.right * curveDistance;

        points = new List<Vector3>();
        for (float ratio = 0f; ratio <= 1f; ratio += 1f / pointCount) {
            Vector3 startTangent = Vector3.Lerp(startPoint, startHandle, ratio);
            Vector3 midLine = Vector3.Lerp(startHandle, endHandle, ratio);
            Vector3 endTangent = Vector3.Lerp(endHandle, endPoint, ratio);
            Vector3 startCurve = Vector3.Lerp(startTangent, midLine, ratio);
            Vector3 endCurve = Vector3.Lerp(midLine, endTangent, ratio);
            Vector3 curve = Vector3.Lerp(startCurve, endCurve, ratio);

            points.Add(curve);
        }

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
    }
}





/*
        startPos = transform.InverseTransformPoint(start.position);
        endPos = transform.InverseTransformPoint(end.position);
        Vector3 startHandle = startPos + transform.right * curveDistance + transform.up * (startPos - endPos).y;
        Vector3 endHandle = endPos - transform.right * curveDistance - transform.up * (startPos - endPos).y;
        Vector3 startMid = startHandle - transform.up * (startHandle - endHandle).y / 2;
        Vector3 endMid = endHandle + transform.up * (startHandle - endHandle).y / 2;

        points = new List<Vector3>();


        for (float ratio = 0f; ratio <= 1f; ratio += 1f / pointCount) {

            Vector3 startTangent = Vector3.Lerp(startPos, startHandle, ratio);
            Vector3 midHoz = Vector3.Lerp(startMid, endMid, ratio);
            Vector3 endTangent = Vector3.Lerp(endHandle, endPos, ratio);
            Vector3 startCurve = Vector3.Lerp(startTangent, midHoz, ratio);
            Vector3 endCurve = Vector3.Lerp(midHoz, endTangent, ratio);
            Vector3 curve = Vector3.Lerp(startCurve, endCurve, ratio);

            // Debug.DrawLine(transform.TransformPoint(startPos), transform.TransformPoint(startHandle));
            // Debug.DrawLine(transform.TransformPoint(endHandle), transform.TransformPoint(endPos));
            points.Add(curve);
        }

        lr.positionCount = points.Count;
        lr.SetPositions(points.ToArray());
*/