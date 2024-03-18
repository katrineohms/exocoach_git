using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSpawner : MonoBehaviour
{
    public GameObject hitbox;
    private GameObject currentHitbox;
    public Transform gameView;

    public float heightOffset = 1;
    public float sideOffset = 1;

    public float sizeOffset = 0.5f;
    private int hitBoxNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHitbox == null)
        {
            spawnHitBox();
        }


    }
    void spawnHitBox()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;

        float leftmostPoint = transform.position.x - sideOffset;
        float rightmostPoint = transform.position.x + sideOffset;

        float narrowestScale = transform.localScale.x - sizeOffset;
        float widestScale = transform.localScale.x + sizeOffset;
        float shortestScale = transform.localScale.y - sizeOffset;
        float tallestScale = transform.localScale.y + sizeOffset;

        currentHitbox = Instantiate(hitbox, new Vector3(Random.Range(leftmostPoint, rightmostPoint), Random.Range(lowestPoint, highestPoint), transform.position.z), transform.rotation);
        currentHitbox.transform.localScale = new Vector3(Random.Range(narrowestScale, widestScale), Random.Range(shortestScale, tallestScale), transform.localScale.z/20);
        currentHitbox.transform.parent = gameView;

        hitBoxNumber++;
        currentHitbox.name = "Hitbox" + hitBoxNumber;
    }


}
