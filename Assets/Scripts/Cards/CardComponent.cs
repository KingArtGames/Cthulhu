using UnityEngine;
using System.Collections;
using Assets;
using Zenject;

public class CardComponent : MonoBehaviour, IClickable
{

    public Animator animator;

    [Inject]
    public Field field;

    private bool _selected;
    public bool Selected
    {
        get { return _selected; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        if (!Selected)
        {
            animator.SetTrigger("Select");
            _selected = true;
        }
        else
        {
            animator.SetTrigger("Deselect");
            _selected = false;
        }
        

    }
}
