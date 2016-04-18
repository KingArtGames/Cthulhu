using UnityEngine;
using System.Collections;
using Assets;

public class InputHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform != null)
            {
                Component clickable = hit.transform.gameObject.GetComponent(typeof(IClickable));
                if (clickable != null)
                {
                    if (Input.GetMouseButtonDown(0))
                        (clickable as IClickable).OnLeftClick();
                    else
                        (clickable as IClickable).OnRightClick();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    CardPreview.SetEnabled(false);
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CardPreview.SetEnabled(false);
            }
        }
    }
}
