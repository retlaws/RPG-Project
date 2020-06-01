using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] float waypointGizmoSize = 1f;
        private void OnDrawGizmos()
        {
            for (int currentWaypoint = 0; currentWaypoint < transform.childCount; currentWaypoint++)
            {
                int nextWaypoint = GetNextWaypoint(currentWaypoint);

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(GetWaypoint(currentWaypoint), waypointGizmoSize);
                Gizmos.DrawLine(GetWaypoint(currentWaypoint), GetWaypoint(nextWaypoint));
            }
        }

        public int GetNextWaypoint(int currentWaypoint)
        {
            if(currentWaypoint < transform.childCount -1)
            {
                return currentWaypoint + 1;
            }
            else
            {
                return 0;
            }

        }

        public Vector3 GetWaypoint(int currentWaypoint)
        {
            return transform.GetChild(currentWaypoint).position;
        }
    }
}
