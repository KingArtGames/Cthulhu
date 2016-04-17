using UnityEngine;
using System.Collections;

public class CardHandComponent : MonoBehaviour {

    public GameObject CardPrefab;

    public float radius = 0.1f;
    public float offset = -0.34f;
    public Vector2 center = new Vector2(0, 0);
    public int cardCount = 5;

    public float cardSpace = 10;

    // Use this for initialization
    void Start () {
        SortCards();
    }
	
	// Update is called once per frame
	void Update () {
        //SortCards();

    }


    private void SortCards()
    {
        /*foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }*/

        

       

    }
}
