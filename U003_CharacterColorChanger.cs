using UnityEngine;

public class U003_CharacterColorChanger : MonoBehaviour
{
    public enum CharacterType { _2D, _3D } // Enum để chọn kiểu nhân vật

    [Header("Character Settings")]
    [SerializeField] private CharacterType characterType = CharacterType._3D;
    [SerializeField] private Color newColor = Color.white; // Màu mặc định

    private void Start()
    {
        ChangeColor(newColor);
    }

    public void ChangeColor(Color color)
    {
        if (characterType == CharacterType._3D)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
        else if (characterType == CharacterType._2D)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = color;
            }
        }
    }
}
