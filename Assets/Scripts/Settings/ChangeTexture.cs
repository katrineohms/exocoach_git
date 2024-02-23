using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
    public Material material;
    public Texture defaultTexture;
    Renderer rend;

    [Header("ChangingObject can be <skybox> or <skin>")]
    public string ChangingObject;

    private void Start()
    {
        if (ChangingObject == "skin")
        {
            string savedTextureName = PlayerPrefs.GetString("CurrentSkinTexture", defaultTexture.name);
            Texture savedTexture = Resources.Load<Texture>("ArmTexture/" + savedTextureName);
            ApplyTexture(savedTexture);
            Debug.Log("Start, skin");
        }

        if (ChangingObject == "skybox")
        {
            string savedTextureName = PlayerPrefs.GetString("CurrentSkybox", defaultTexture.name);
            Debug.Log("Texture name is: " + savedTextureName);

            Texture savedTexture = Resources.Load<Texture>("Skybox/" + savedTextureName);

            Debug.Log("Texture is: " + savedTexture);
             ApplyTexture(savedTexture);
            Debug.Log("Start, skybox");
        }
    }

    public void ChangeToDefaultTexture()
    {
        if (ChangingObject == "skin")
        {
            Texture defaultTex = Resources.Load<Texture>("ArmTexture/" + defaultTexture.name);
            ApplyTexture(defaultTex);
            Debug.Log("Skin is default");
        }

        if (ChangingObject == "skybox")
        {
            Texture defaultTex = Resources.Load<Texture>("Skybox/" + defaultTexture.name);
            ApplyTexture(defaultTex);
            Debug.Log("Skybox is default");
        }
    }

    public void ChangingTexture(string textureName)
    {
        if (ChangingObject == "skin")
        {
            Texture newTexture = Resources.Load<Texture>("ArmTexture/" + textureName);
            ApplyTexture(newTexture);

            PlayerPrefs.SetString("CurrentSkinTexture", textureName);
            PlayerPrefs.Save();
        }

        if (ChangingObject == "skybox")
        {
            Texture newTexture = Resources.Load<Texture>("Skybox/" + textureName);
            if (newTexture != null)
            {
                ApplyTexture(newTexture);
                PlayerPrefs.SetString("CurrentSkybox", textureName);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.LogError("Texture not found: " + textureName);
                // You might want to handle this error case in some other way
            }
        }
    }


    private void ApplyTexture(Texture texture)
    {
        if (ChangingObject == "skybox")
        {
            RenderSettings.skybox.SetTexture("_MainTex", texture);
            //rend.material.SetTexture("_Tex", texture);
            Debug.Log("Skybox texture is: " + texture);
        }

        material.mainTexture = texture;
        Debug.Log("Applying texture: " + texture + "to material: " + material);
    }


}