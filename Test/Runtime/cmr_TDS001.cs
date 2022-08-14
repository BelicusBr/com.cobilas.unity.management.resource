using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cobilas.Unity.Management.Resources;

public class cmr_TDS001 : MonoBehaviour
{
    private Texture2D texture1;
    private Sprite texture2;
    private Texture2D texture3;
    private Sprite texture4;
    // Start is called before the first frame update
    void Start()
    {
        texture1 = CobilasResources.GetTexture<Texture2D>("blue-document-icon");
        texture2 = CobilasResources.GetSprite("blue-folder-icon");
        texture3 = CobilasResources.GetTexture<Texture2D>("Seta_Aberto");
        texture4 = CobilasResources.GetSprite("Seta_Fechado");
    }

    private void OnGUI()
    {
        Rect rect = new Rect(0f, 150f, 50f, 50f);
        GUI.DrawTexture(rect, texture1);
        rect.y += 50f;
        GUI.DrawTexture(rect, texture2.texture);
        rect.y += 50f;
        GUI.DrawTexture(rect, texture3);
        rect.y += 50f;
        GUI.DrawTexture(rect, texture4.texture);
    }
}
