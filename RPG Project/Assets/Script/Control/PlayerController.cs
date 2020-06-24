﻿using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using System.Net;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        
        Mover mover;
        Health health;
       
        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings;
        [SerializeField] CursorMapping defaultMapping;
        [SerializeField] float maxNavSampleDistance = 1f;
        [SerializeField] float maxNavPathLength = 40f;




        void Awake()
        {
            mover = GetComponent<Mover>();
            health = GetComponent<Health>();
            defaultMapping = cursorMappings[0];
        }

        void Update()
        {
            if (InteractWithUI()) 
            {
                SetCursor(CursorType.UI);
                return; 
            };
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit == false) return false;

            if (Input.GetMouseButton(0))
            {
                mover.StartMoveAction(target, 1f);
            }
            SetCursor(CursorType.Movement);
            return true;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            Physics.Raycast(GetMouseRay(), out hit);
            NavMeshHit navHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navHit, maxNavSampleDistance, NavMesh.AllAreas);
            if(!hasCastToNavMesh) return false;
            
            target = navHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            Vector3[] waypoints = path.corners;
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                float distance = Vector3.Distance(waypoints[i], waypoints[i + 1]);
                total += distance;
            }
            return total; 
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            mapping.hotspot = new Vector2(mapping.texture.width / 2, mapping.texture.height / 2); 
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(type == mapping.type)
                {
                    return mapping;
                }
            }
            return defaultMapping;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
