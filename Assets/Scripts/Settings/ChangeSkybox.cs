using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Material skybox;

    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = skybox;
    }

    // Update is called once per frame
    public void ChangeSkyboxTexture(Cubemap skyboxTexture)
    {
        skybox.SetTexture("_Tex", skyboxTexture);
    }
}
