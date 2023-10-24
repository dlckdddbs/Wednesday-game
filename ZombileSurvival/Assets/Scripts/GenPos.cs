using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Dotomchi
{
    public enum GizmoType
    {
        enemy,
        player,
    }

    public class GenPos : MonoBehaviour
    {
        public GizmoType type = GizmoType.enemy;

        private void OnDrawGizmos()
        {
            Gizmos.color = (type == GizmoType.enemy) ? Color.red : Color.blue;
            Gizmos.DrawSphere(transform.position, 1);
        }


    }
}
