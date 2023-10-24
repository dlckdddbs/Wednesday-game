using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{

    public class FollowObject : MonoBehaviour
    {
        public GameObject target = null;

        public float AddX = -8.4f;
        public float AddZ = 5.6f;
        public float Height = 14.0f;
        public float Speed = 5.0f;
        Vector3 Pos;

        // Start is called before the first frame update
        void Start()
        {
            transform.rotation = new Quaternion(0.4f, -0.4f, 0.2f, 0.8f);
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
                return;

            Pos = new Vector3(target.transform.position.x - AddX, target.transform.position.y + Height, target.transform.position.z - AddZ);
            //Pos = target.transform.position + new Vector3(-AddX, Height, -AddZ);

            transform.position = Vector3.Lerp(transform.position, Pos, Speed * Time.deltaTime);
        }
    }
}
