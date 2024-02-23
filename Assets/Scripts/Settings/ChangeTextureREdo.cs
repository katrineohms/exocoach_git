using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureREdo : MonoBehaviour
{
    public Material material;
    public Texture defaultTexture;

    public string PlayerPrefClass; //I.e. "CurrentSkinTexture"
    public string ResourcesDirectory; //I.e. "ArmTexture/"
    Renderer rend;

    private void Start()
    {

        string savedTextureName = PlayerPrefs.GetString(PlayerPrefClass, defaultTexture.name);
        Texture savedTexture = Resources.Load<Texture>(ResourcesDirectory + savedTextureName);
        ApplyTexture(savedTexture);
        Debug.Log("Start");

    }

    public void AChangeTexture(string playerPrefKey)
    {
        Texture newTexture = Resources.Load<Texture>(ResourcesDirectory + playerPrefKey);
        ApplyTexture(newTexture);

        PlayerPrefs.SetString(PlayerPrefClass, playerPrefKey);
        PlayerPrefs.Save();
    }


    private void ApplyTexture(Texture texture)
    {
        material.mainTexture = texture;
        Debug.Log("Applying texture: " + texture + "to material: " + material);
    }


}