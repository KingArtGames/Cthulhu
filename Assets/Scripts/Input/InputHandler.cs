using UnityEngine;
using System.Collections;
using Assets;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))

                if (hit.transform != null)
                {
                    Component clickable = hit.transform.gameObject.GetComponent(typeof(IClickable));
                    if (clickable != null)
                    {
                        (clickable as IClickable).OnClick();
                    }
                }
        }
    }
}
