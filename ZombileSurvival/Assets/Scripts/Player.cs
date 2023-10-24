using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dotomchi
{
    public enum AniStateType
    {
        idle,
        move,
        attack,
        death,
        max,
    }

    public class Player : MonoBehaviour
    {

        private int point = 0;


        public Manager manager = null;
        public Renderer render = null;

        public Animator ani = null;
        public Rigidbody rigid = null;
        public Collider col = null;
        public NavMeshAgent agent = null;
        public GameObject muzzleEffect = null;

        public AniStateType aniStateType = AniStateType.idle;
        public AnimatorStateInfo stateInfo;

        public bool isShooting = false;
        public bool isAlive = true;
        public bool isReload = false;

        public int level;
        public int hp, maxHp;
        public int attackPoint;
        public int ammo, maxAmmo;
        public float reloadTime = 1.0f;
        public float grabSoulRange = 1.0f;


        private Coroutine muzzleCoroutine = null;
        private Coroutine damageCoroutine = null;
        private Coroutine reloadCoroutine = null;


        public int GetValue()
        {
            return point;
        }
        public void SetValue(int _value)
        {
            point = _value;
        }


        public int Point
        {
            get
            {
                return point;
            }
            set
            {
                point = value;
            }
        }


        // Start is called before the first frame update
        void Start()
        {

            ResetParam();

            ani = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            agent = GetComponent<NavMeshAgent>();

            if (muzzleEffect)
                muzzleEffect.SetActive(false);

            StartCoroutine(ShootingEvent());
        }

        public void ResetParam()
        {
            level = 1;
            attackPoint = 1;
            isAlive = true;
            hp = maxHp = 20;
            ammo = maxAmmo = 30;
        }

        // Update is called once per frame
        void Update()
        {

            if (manager && manager.gameState != GameState.playing)
                return;

            Collider[] colList = Physics.OverlapSphere(transform.position, grabSoulRange, LayerMask.GetMask("Soul"));
            for (int i = 0; i < colList.Length; i++)
            {
                Soul soul = colList[i].GetComponent<Soul>();
                if (soul)
                {
                    soul.target = gameObject;
                }
            }



            if (ani)
            {
                //���� ���ϸ����Ϳ��� �������� �ִ� ������Ʈ ���� ���ؿͼ� ��� ������ �ִϻ��¸� ����.
                //�̷��� �ϴ� ������ �ߺ��ؼ� �ִϰ� ������� �ʰ� üũ�ϱ� ���� �뵵
                stateInfo = ani.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("attack")) //���� �ִϰ� �����̸�
                    aniStateType = AniStateType.attack; //�ִϻ��¸� �������� ����
                else if (stateInfo.IsName("move"))
                    aniStateType = AniStateType.move; //��� ���� ����
                else
                    aniStateType = AniStateType.idle; //�ƹ��͵� �ƴϸ� ���̵����
            }

            //�̵�
            float moveZ = Input.GetAxis("Vertical");
            float moveX = Input.GetAxis("Horizontal");

            Vector3 angle = new Vector3(moveX, 0, -moveZ);
            if (angle != Vector3.zero)
            {
                Quaternion r = Quaternion.LookRotation(angle) * Quaternion.Euler(0, -45, 0);


                Vector3 movePos = GetDegreesDistVector3(transform.position, r.eulerAngles.y, 3.0f);

                
                transform.position  = Vector3.MoveTowards(transform.position, movePos, Time.deltaTime * 3.0f);
                if (ani)
                    ani.SetFloat("moveSpeed", 3.0f);
                //angle�� 0�� �ƴ϶�� �޸��� ��� ���
            }
            else
            {
                if (ani)
                    ani.SetFloat("moveSpeed", 0.0f);
            }

            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 viewPos = hit.point - transform.position;

                viewPos.y = transform.position.y;

                Quaternion rot = Quaternion.LookRotation(viewPos, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 20.0f);
            }


            //���ݿ� ���õ� �κ�
            if (Input.GetMouseButton(0) && isReload == false)
            {


                isShooting = true;

                if (muzzleCoroutine != null)
                    StopCoroutine(muzzleCoroutine);

                muzzleCoroutine = StartCoroutine(MuzzleEvent());


                if (ani && ani.GetFloat("moveSpeed") < 0.05f)
                    ani.Play("attack");

                
            }
        }

        public void SetDamage(int damage, Transform form)
        {
            if (isAlive)
            {
                if (manager)
                    manager.ShowDamagePrefeb(gameObject, damage, Color.red);

                if (form != null)
                {
                    Vector3 look = form.position - transform.position;
                    look.y = transform.position.y;
                    transform.rotation = Quaternion.LookRotation(look);
                }

                if (damageCoroutine != null)
                    StopCoroutine(damageCoroutine);

                damageCoroutine = StartCoroutine(DamageEvent());


                hp -= damage;

                if (hp <= 0)
                    isAlive = false;

            }
        }

        //static���� ������� ������ �������� ȣ�� ������ �Լ�.
        //������ �������� 0~360���� �ش��ϴ� ���� ��ġ�� ����
        static public Vector3 GetDegreesDistVector3(Vector3 oriPos, float degreeVal, float distVal)
        {
            Vector3 resultVec = Vector3.zero;

            float degrees = degreeVal;

            float radian = degrees * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float y = Mathf.Sin(radian);
            resultVec = oriPos + (new Vector3(x, 0, y) * distVal);

            return resultVec;
        }

        IEnumerator MuzzleEvent()
        {
                if (muzzleEffect)
                    muzzleEffect.SetActive(true);

                yield return new WaitForSeconds(0.2f);
                if (muzzleEffect)
                    muzzleEffect.SetActive(false);
            
        }

        IEnumerator ShootingEvent()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (isShooting)
                {
                    GameObject bullet = ObjectPoolManager.instance.GetObject("Prefabs/Bullet"); //Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));

                    if (bullet)
                    {
                        bullet.transform.position = muzzleEffect.transform.position;
                        Bullet bulletScript = bullet.GetComponent<Bullet>();
                        if (bulletScript)
                            bulletScript.MoveStart(transform, attackPoint);
                        ammo--;
                        if (ammo <= 0)
                        {
                            isReload = true;

                            if (reloadCoroutine != null)
                                StopCoroutine(reloadCoroutine);

                            reloadCoroutine = StartCoroutine(ReloadEvent());

                        }
                    }
                    isShooting = false; 
                }
            }
        }

        IEnumerator DamageEvent()
        {
            if (render)
                render.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);
            if (render)
                render.material.color = Color.white;
        }

        IEnumerator ReloadEvent()
        {
            reloadTime = 0.0f;
            while (reloadTime < 1.0f && manager)
            {
                yield return new WaitForSeconds(0.01f);
                reloadTime += 0.005f + (manager.GetUpgradeLevel(UpgradeItemType.fastReload) * 0.005f);
            }
            ammo = maxAmmo;
            isReload = false;
        }
    }
}
