using Dotomchi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiHpbar1 : MonoBehaviour
{
    public GameObject showObj = null;
    public Image hpGauge = null;
    public Text hpValue = null, distValue = null;
    public Enemy Enemy = null;
    private float height = 220.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (showObj)
            showObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraDist = Camera.main.transform.position - Enemy.transform.position;
        height = 220 - cameraDist.sqrMagnitude * 0.05f;
        if (height < 50)
            height = 50;

        else
        {
            if (distValue)
                distValue.gameObject.SetActive(false);
        }

        if (hpValue)
            hpValue.text = string.Format("{0}/{1}", Enemy.hp, Enemy.maxHp);
        if (hpGauge)
            hpGauge.fillAmount = (float)Enemy.hp / (float)Enemy.maxHp;

        if (Camera.main && Enemy)
        {

            transform.rotation = Camera.main.transform.rotation;
        }

        //Vector3 v = Camera.main.transform.position;
        //transform.LookAt(new Vector3(transform.position.x, v.y, v.z));
    }
}
