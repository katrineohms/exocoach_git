using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JigsawManager : MonoBehaviour
{
    [Header("UI elemenents")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Texture2D texture in imageTextures)
        {
            Image image = Instantiate(levelSelectPrefab, levelSelectPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
