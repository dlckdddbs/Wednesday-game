using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dotomchi
{

    public class TitleScene : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ChangeScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
        }

    }
}
