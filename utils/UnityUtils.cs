using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnityUtils : MonoBehaviour
{

    public static void SetTexture2DForImage(Image image, Texture2D texture2D)
    {
        // 获取原始Sprite的尺寸
        float originalWidth = image.sprite.bounds.size.x;
        float originalHeight = image.sprite.bounds.size.y;

        // 将Texture2D赋值给Image对象，并根据原始Sprite尺寸进行缩放
        Sprite sprite = Sprite.Create(
            texture2D, 
            new Rect(0, 0, texture2D.width, texture2D.height), 
            new Vector2(0.5f, 0.5f), Mathf.Max(originalWidth / texture2D.width, 
            originalHeight / texture2D.height)
            );
        image.sprite = sprite;
    }

}
