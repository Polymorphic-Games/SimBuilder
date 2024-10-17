using geniikw.DataRenderer2D;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SceneSandBox
{
    public class RateLineScript : MediatorComponent, IUpdatePositionHandler
    {
        GameObject FromObject;
        GameObject ToObject;

        const int numPoints = 100;
        GameObject[] lineSegments = new GameObject[numPoints];

        //GameObject[] controlPoints = new GameObject[4];

        public Vector2 P0 { get; private set; } = Vector2.negativeInfinity;
        public Vector2 P1 { get; private set; }
        public Vector2 P2 { get; private set; }
        public Vector2 P3 { get; private set; } = Vector2.negativeInfinity;

        float scale;

        public void OnDestroy()
        {
            if (FromObject != null)
            {
                FromObject.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this, false);
            }
            if (ToObject != null)
            {
                ToObject.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this, false);
            }
            DestroyLineSegments();
        }

        public void InitializeComponent(MediatorComponent mediator, GameObject fromObject)
        {
            base.InitializeComponent(mediator);
            FromObject = fromObject;
            FromObject.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);
            P0 = fromObject.transform.position;
        }

        public void InitializeComponent(MediatorComponent mediator, GameObject fromObject, GameObject toObject)
        {
            base.InitializeComponent(mediator);
            FromObject = fromObject;
            ToObject = toObject;
            FromObject.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);
            ToObject.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);
            P0 = FromObject.transform.position;
            P3 = ToObject.transform.position;
            DrawCurve();
        }

        public void DestroyLineSegments()
        {
            for (int i = 0; i < numPoints; ++i)
            {
                Destroy(lineSegments[i]);
            }
        }

        public void Awake()
        {

            for (int i = 0; i < numPoints; ++i)
            {
                lineSegments[i] = new GameObject();
                lineSegments[i].transform.parent = transform;
                lineSegments[i].AddComponent<Image>();
                lineSegments[i].GetComponent<Image>().color = new Color32(128, 128, 128, 128);
                lineSegments[i].GetComponent<Image>().raycastTarget = false;
            }



            //for (int i = 0; i < 4; ++i)
            //{
            //    controlPoints[i] = new GameObject();
            //    controlPoints[i].transform.parent = transform;
            //    controlPoints[i].AddComponent<Image>();
            //    controlPoints[i].GetComponent<RectTransform>().sizeDelta = new Vector2(10,10);
            //    controlPoints[i].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            //}
        }



        Vector2 CubicBezier(float t)
        {
            Vector2 P =
                Mathf.Pow(1 - t, 3) * P0 +
                t * P1 * (3 * Mathf.Pow(1 - t, 2)) +
                P2 * (3 * (1 - t) * Mathf.Pow(t, 2)) +
                P3 * Mathf.Pow(t, 3);
               
            return P;
        }

        public void DrawSCurve()
        {
            float curveLength = Mathf.Abs(P0.x - P3.x);
            float halfHeightDiff = (P0.y - P3.y) / 2;
            P1 = new Vector2(P0.x + 0.75f * curveLength, P0.y);
            P2 = new Vector2(P0.x + 0.25f * curveLength, P3.y);

            //controlPoints[0].GetComponent<RectTransform>().position = P0;
            //controlPoints[1].GetComponent<RectTransform>().position = P1;
            //controlPoints[2].GetComponent<RectTransform>().position = P2;
            //controlPoints[3].GetComponent<RectTransform>().position = P3;


        }

        public void DrawCircleCurve()
        {
            P1 = new Vector2(P0.x + 100, P0.y + 100);
            P2 = new Vector2(P3.x - 100, P0.y + 100);
        }
        public void DrawCurve()
        {
            float t = 1 / (float)numPoints;
            if (ToObject == null)
            {
                DrawSCurve();
            }
            else if (FromObject.transform.parent != ToObject.transform.parent)
            {
                DrawSCurve();
            } else
            {
                DrawCircleCurve();
            }


            Vector2 lineSegmentA = CubicBezier(0);
            Vector2 lineSegmentB;
            RectTransform rect;

            for (int i = 0; i < numPoints; i++)
            {
                lineSegmentB = CubicBezier((i + 1) * t);
                rect = lineSegments[i].GetComponent<RectTransform>();
                DrawLine(rect, lineSegmentA, lineSegmentB);
                lineSegmentA = lineSegmentB;
            }

            NotifyPositionHandler();
        }

        public void DrawCurve(Vector2 p0, Vector2 p3)
        {
            P0 = p0;
            P3 = p3;
            DrawCurve();
        }

        public void DrawLine(RectTransform rect, Vector2 a, Vector2 b)
        {
            int lineWidth = 8;
            rect.position = (a + b) / 2;
            Vector3 dif = a - b;
            rect.sizeDelta = new Vector3(dif.magnitude, lineWidth);
            rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }

        public void UpdateStartPosition(Vector2 startPosition)
        {
            P0 = startPosition;
            //returns false when P3 is infinite
            if (P3 == P3)
            {
                DrawCurve();
            }
        }

        public void UpdateEndPosition(Vector2 endPosition)
        {
            P3 = endPosition;
            //returns false when P0 is inifinite
            if (P0 == P0)
            {
                DrawCurve();
            }
        }

        public void NotifyPositionHandler()
        {
            GetComponent<PositionHandlerScript>().UpdatePosition(CubicBezier(0.5f));
        }

        public void UpdatePositionCallback(object sender, Vector2 position)
        {
            GameObject senderGameObject = ((PositionHandlerScript) sender).gameObject;
            
            //get origin script of sender gameobject
            if (senderGameObject.GetComponent<NodeOutScript>() != null)
            {
                UpdateStartPosition(position);
                return;
            }

            if (senderGameObject.GetComponent<NodeInScript>() != null)
            {
                UpdateEndPosition(position);
                return;
            }
        }

    }

}