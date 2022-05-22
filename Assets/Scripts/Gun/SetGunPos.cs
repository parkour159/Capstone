using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGunPos : MonoBehaviour
{
    public GameObject ak74;
    public GameObject m1911;
    public Transform ak74SetPos;
    public Transform m1911SetPos;
    public bool isM1911Grabbed = false;
    public bool isAK74Grabbed = false;

    void Update()
    {
        if (isAK74Grabbed == false)
        {
            ak74.transform.position = ak74SetPos.transform.position;
            ak74.transform.rotation = ak74SetPos.transform.rotation;
        }
        if (isM1911Grabbed == false)
        {
            m1911.transform.position = m1911SetPos.transform.position;
            m1911.transform.rotation = m1911SetPos.transform.rotation;
        }
    }
}
