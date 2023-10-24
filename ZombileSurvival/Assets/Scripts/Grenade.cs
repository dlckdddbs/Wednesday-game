using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{

    public class Grenade : MonoBehaviour
    {
        public Rigidbody rigid = null;

        public GameObject bulletObj = null, explosionEff = null;

        // Start is called before the first frame update
        void OnEnable()
        {
            if (bulletObj)
                bulletObj.SetActive(true);
            if (explosionEff)
                explosionEff.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ThrowStart(Transform form)
        {
            transform.rotation  = form.rotation;
            if (rigid)
                rigid.velocity = transform.forward * 8.0f;
            StartCoroutine(Explosion());
        }

        IEnumerator Explosion()
        {
            yield return new WaitForSeconds(3.0f);

            if (bulletObj)
                bulletObj.SetActive(false);
            if (explosionEff)
                explosionEff.SetActive(true);

            Collider[] colList = Physics.OverlapSphere(transform.position, 2.0f, LayerMask.GetMask("Enemy"));
            for (int i = 0; i < colList.Length; i++)
            {
                Enemy enemy = colList[i].GetComponent<Enemy>();
                if (enemy)
                    enemy.SetDamage(5, transform);
            }

            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }

    }
}
