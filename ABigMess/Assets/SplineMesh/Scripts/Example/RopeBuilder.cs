﻿using SplineMesh;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SplineMesh {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Spline))]
    public class RopeBuilder : MonoBehaviour {
        private bool toUpdate = false;
        private GameObject generated;
        private GameObject Generated {
            get {
                if (generated == null) {
                    string generatedName = "generated by " + GetType().Name;
                    var generatedTranform = transform.Find(generatedName);
                    generated = generatedTranform != null ? generatedTranform.gameObject : UOUtility.Create(generatedName, gameObject);
                }
                return generated;
            }
        }

        private Spline spline;
        private GameObject firstSegment;

        [SerializeField]
        public List<GameObject> wayPoints = new List<GameObject>();

        public GameObject segmentPrefab;
        public int segmentCount;
        public float segmentSpacing;

        public bool isPositionFreeze = true;

        [SerializeField]
        Transform handPosition;
        public Transform HandPosition
        {
            get=> handPosition;
        }

        private Vector3 handOriginPosition;
        public Vector3 HandOriginPosition
        {
            get => handOriginPosition;
        }


        private void OnEnable() {
            spline = GetComponent<Spline>();
            toUpdate = true;
        }

        private void OnValidate() {
            toUpdate = true;
        }

        private void Update() {
            if (toUpdate) {
                toUpdate = false;
                Generate();
                UpdateSpline();
            }

            // balancing
            /*
            if (Application.isPlaying) {
                firstSegment.transform.localPosition = new Vector3(Mathf.Sin(Time.time) * 3, 0, 0);
            }
            */
        }

        private void FixedUpdate()
        {
            UpdateNodes();
        }

        private void UpdateNodes() {
            int i = 0;
            foreach (GameObject wayPoint in wayPoints) {
                var node = spline.nodes[i++];
                //if (Vector3.Distance(node.Position, transform.InverseTransformPoint(wayPoint.transform.position)) > 0.001f) 
                if (Vector3.Distance(node.Position, transform.InverseTransformPoint(wayPoint.transform.position)) > 0.000001f)
                {
                    node.Position = transform.InverseTransformPoint(wayPoint.transform.position);
                    node.Up = wayPoint.transform.up;
                }
            }
        }

        private void UpdateSpline() {
            foreach (var penisNode in wayPoints.ToList()) {
                if (penisNode == null) wayPoints.Remove(penisNode);
            }
            int nodeCount = wayPoints.Count;
            // adjust the number of nodes in the spline.
            while (spline.nodes.Count < nodeCount) {
                spline.AddNode(new SplineNode(Vector3.zero, Vector3.zero));
            }
            while (spline.nodes.Count > nodeCount && spline.nodes.Count > 2) {
                spline.RemoveNode(spline.nodes.Last());
            }
        }

        private void Generate() {
            UOUtility.DestroyChildren(Generated);
            wayPoints.Clear();
            float localSpacing = 0;
            Joint joint = null;
            for (int i = 0; i < segmentCount; i++) {
                var seg = UOUtility.Instantiate(segmentPrefab, Generated.transform);
                seg.transform.Translate(0, 0, localSpacing);

                var segRB = seg.GetComponent<Rigidbody>();
                // we fix the first segment so that the rope won't fall
                if (i == 0 && isPositionFreeze) {
                    firstSegment = seg;
                    segRB.constraints = RigidbodyConstraints.FreezePosition;
                }
                else if(i==segmentCount-1)
                {
                    handPosition = seg.transform;
                    seg.transform.GetChild(0).transform.GetChild(0).GetComponent<CapsuleCollider>().isTrigger = true;
                  //  Destroy(seg.transform.GetChild(0).transform.GetChild(0).GetComponent<CapsuleCollider>());
                }

                // we attach the rigidbody to the joint of the previous segment
                if (joint != null) {
                    joint.connectedBody = segRB;
                }
                joint = seg.GetComponent<Joint>();

                // we save segments as way points for the spline deformation.
                wayPoints.Add(seg);
                localSpacing += segmentSpacing;
            }
            UOUtility.Destroy(joint);
        }
    }


}
