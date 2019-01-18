using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse_Particle : MonoBehaviour {

    public GameObject Pt;
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButton(0))
        {
            Pt.gameObject.GetComponent<ParticleSystem>().Play();
            Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vec.z = 1;
            Pt.transform.localPosition = vec;
        

        }
        else if(Input.GetMouseButtonUp(0))
        {
            Pt.gameObject.GetComponent<ParticleSystem>().Stop();
        }
    }

   
}
