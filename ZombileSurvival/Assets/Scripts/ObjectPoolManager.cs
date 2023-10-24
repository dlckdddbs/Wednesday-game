using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private static ObjectPoolManager m_Instance;
        public static ObjectPoolManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject newObject = new GameObject("_ObjectPoolManager");

                    m_Instance = newObject.AddComponent<ObjectPoolManager>();

                }
                return m_Instance;
            }
        }




        protected Dictionary<string, List<GameObject>> m_objectPools = new Dictionary<string, List<GameObject>>();
        public int overCreateSize = 10;

        private GameObject m_uiObjectPool = null;

        private void Awake()
        {
            Debug.Log("[ObjectPoolManager::Awake]");

            if(m_Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {                
                InitUIObjectPool();
            }
        }

        private void InitUIObjectPool()
        {
            if (m_uiObjectPool != null)
            {
                return;
            }



            GameObject uiCanvasObject = Instantiate(Resources.Load("Prefabs/CanvasWorldPositionUI")) as GameObject;

            m_uiObjectPool = new GameObject("_UIObjectPool");
            m_uiObjectPool.transform.parent = uiCanvasObject.transform;
            m_uiObjectPool.transform.localPosition = Vector3.zero;
            m_uiObjectPool.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public void ReservePool(string typeName, GameObject prefab, int size, bool isUIType = false)
        {
            if (m_objectPools.ContainsKey(typeName) == true)
            {
                return;
            }

            List<GameObject> objectList = new List<GameObject>();
            m_objectPools.Add(typeName, objectList);

            for (int i = 0; i < size; i++)
            {
                GameObject instance = (GameObject)Instantiate(prefab);
                instance.name = typeName + "_" + (i + 1);

                Transform parent = null;
                if(isUIType == true)
                {
                    if(m_uiObjectPool == null)
                    {
                        InitUIObjectPool();                    
                    }

                    parent = m_uiObjectPool.transform;
                }
                else
                {
                    parent = transform;
                }
                instance.transform.SetParent(parent);
                if(isUIType)
                    instance.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                m_objectPools[typeName].Add(instance);
            }
        }

        public void ReservePool(string path, int size, bool isUIType = false)
        {
            GameObject prefab = Instantiate(Resources.Load(path)) as GameObject;
            prefab.SetActive(false);
            ReservePool(path, prefab, size, isUIType);
            Destroy(prefab);
        }

        public GameObject GetObject(string typeName, bool autoActive = true, bool isUIType = false)
        {
            if (m_objectPools.ContainsKey(typeName) == false)
            {
                ReservePool(typeName, 1, isUIType);
            }
            //if (m_objectPools.ContainsKey(typeName) == true)
            //{


            List<GameObject> objectList = m_objectPools[typeName];
            
            objectList.RemoveAll(obj => (obj == null));

            // 리스트중에 비활성화 오브젝트 찾아서 넘겨줌
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i] == null)
                {
                    Debug.LogError("Object pool error :" + typeName);
                }
                else if (objectList[i].activeSelf == false)
                {
                    GameObject findObject = objectList[i];

                    if (isUIType)
                        findObject.transform.position = new Vector3(-100, -100, 0);

                    findObject.SetActive(autoActive);
                    return findObject;
                }
            }
            
            // 없으면 추가로 생성
            GameObject returnObject = null;
            GameObject prefab = objectList[0];
            int listCount = objectList.Count;
            for (int i = 0; i < overCreateSize; i++)
            {
                GameObject instance = (GameObject)Instantiate(prefab);
                instance.SetActive(false);
                instance.name = typeName + "_" + (listCount + i + 1);

                Transform parent = isUIType ? m_uiObjectPool.transform : transform;
                instance.transform.SetParent(parent);


                if(isUIType)
                    instance.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                objectList.Add(instance);

                // 첫번째껄 넘겨줌
                if (i == 0)
                {
                    instance.SetActive(autoActive);
                    returnObject = instance;
                }
            }
            return returnObject;
            //}

            //return null;
        }

        public void EnableAll(string typeName, bool isEnable)
        {
            if (m_objectPools.ContainsKey(typeName) == true)
            {
                List<GameObject> objectList = m_objectPools[typeName];

                for (int i = 0; i < objectList.Count; i++)
                {
                    objectList[i].SetActive(isEnable);
                }
            }
        }

        public void EnableAllType(bool isEnable)
        {
            foreach (KeyValuePair<string, List<GameObject>> child in m_objectPools)
            {
                EnableAll(child.Key, isEnable);
            }
        }

        public void DestroyAll()
        {
            foreach (KeyValuePair<string, List<GameObject>> child in m_objectPools)
            {
                foreach (GameObject child2 in child.Value)
                {
                    Destroy(child2);
                }
                child.Value.Clear();
            }
            m_objectPools.Clear();
        }

        public void DestroyTypeObject(string typeName)
        {
            if (m_objectPools.ContainsKey(typeName) == true)
            {
                List<GameObject> objectList = m_objectPools[typeName];

                for (int i = 0; i < objectList.Count; i++)
                {
                    Destroy(objectList[i]);
                }
                objectList.Clear();


                m_objectPools.Remove(typeName);
            }
        }
        public int GetCountObjectType(string typeName)
        {
            if (m_objectPools.ContainsKey(typeName) == true)
            {
                List<GameObject> objectList = m_objectPools[typeName];
                int count = 0;
                for (int i = 0; i < objectList.Count; i++)
                {
                    if (objectList[i].activeSelf)
                        count++;
                }
                return count;
            }
            return -1;
        }
    }
}



