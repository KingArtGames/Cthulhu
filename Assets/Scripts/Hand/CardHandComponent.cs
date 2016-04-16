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

        

        offset = -0.055f * cardCount;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardGO = Instantiate<GameObject>(CardPrefab);
            cardGO.transform.parent = transform;
            float posX = center.x + radius * Mathf.Sin(offset + (i * cardSpace) * Mathf.Deg2Rad);
            float posY = center.y + radius * Mathf.Cos(offset + (i * cardSpace) * Mathf.Deg2Rad);

            cardGO.transform.localPosition = new Vector3(posX, (i*0.1f), posY);

            Vector2 direction = new Vector2(center.x - posX, center.y - posY);

            cardGO.transform.Rotate(Vector3.up, -direction.x, Space.Self);
        }

    }
}
