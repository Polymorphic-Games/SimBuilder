using SceneSandBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.UI;

namespace SceneSandbox { 

    public class RateModifierLineScript : MediatorComponent, IUpdatePositionHandler
    {
        const int numPoints = 50;
        GameObject[] lineSegments = new GameObject[numPoints];
        GameObject InstanceRateEquation;
        GameObject InstanceStateNode;

        //GameObject[] controlPoints = new GameObject[4];

        Vector2 P0;
        Vector2 P1;
        Vector2 P2;

        float scale;

        public void OnDestroy()
        {
            if (InstanceRateEquation != null)
            {
                InstanceRateEquation.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this, false);
            }
            if (InstanceStateNode != null)
            {
                InstanceStateNode.GetComponentInChildren<NodeUpAndOutScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this, false);
            }
            destroyLineSegments();
        }

        public void InitializeComponent(MediatorComponent mediator, Vector2 initialPosition)
        {
            base.InitializeComponent(mediator);
            P0 = initialPosition;
        }

        public void InitializeComponent(MediatorComponent mediator, GameObject instanceRateEquation, GameObject instanceStateNode)
        {
            base.InitializeComponent(mediator);
            InstanceRateEquation = instanceRateEquation;
            InstanceStateNode = instanceStateNode;
            
            InstanceRateEquation.GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);
            InstanceStateNode.GetComponentInChildren<NodeUpAndOutScript>().GetComponent<PositionHandlerScript>().UpdatePositionEventListener(this);

            P0 = InstanceStateNode.transform.position;
            P2 = InstanceRateEquation.transform.position;
            drawCurve();
        }

        public void destroyLineSegments()
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
                lineSegments[i].GetComponent<Image>().color = new Color32(128, 128, 128, 32);
                lineSegments[i].GetComponent<Image>().raycastTarget = false;
            }

        }



        Vector2 Bezier(float t)
        {
            Vector2 P =
                Mathf.Pow(1 - t, 2) * P0 +
                2 * (1 - t) * t * P1 +
                Mathf.Pow(t, 2) * P2;

            return P;
        }

        public void drawCurve()
        {
            float t = 1 / (float)numPoints;

            P1.x = (P2.x + P0.x) / 2;

            if (P0.y > P2.y)
            {
                P1.y = P0.y + 200;
            } else
            {
                P1.y = P2.y + 200;
            }

            Vector2 lineSegmentA = Bezier(0);
            Vector2 lineSegmentB;
            RectTransform rect;

            for (int i = 0; i < numPoints; i++)
            {
                lineSegmentB = Bezier((i + 1) * t);
                rect = lineSegments[i].GetComponent<RectTransform>();
                drawLine(rect, lineSegmentA, lineSegmentB);
                lineSegmentA = lineSegmentB;
            }

        }

        public void drawLine(RectTransform rect, Vector2 a, Vector2 b)
        {
            int lineWidth = 2;
            rect.position = (a + b) / 2;
            Vector3 dif = a - b;
            rect.sizeDelta = new Vector3(dif.magnitude*0.75f, lineWidth);
            rect.rotation = Quaternion.Euler(new Vector3(0, 0, 180 * Mathf.Atan(dif.y / dif.x) / Mathf.PI));
        }

        public void updateStartPosition(Vector2 startPosition)
        {
            P0 = startPosition;
            drawCurve();
        }

        public void updateEndPosition(Vector2 endPosition)
        {
            P2 = endPosition;
            drawCurve();
        }


        public void UpdatePositionCallback(object sender, Vector2 position)
        {
            GameObject senderGameObject = ((PositionHandlerScript)sender).gameObject;

            //get origin script of sender gameobject
            if (senderGameObject.GetComponent<NodeUpAndOutScript>() != null)
            {
                updateStartPosition(position);
                return;
            }

            if (senderGameObject.GetComponent<RateEquationScript>() != null)
            {
                updateEndPosition(position);
                return;
            }
        }

    }
}
