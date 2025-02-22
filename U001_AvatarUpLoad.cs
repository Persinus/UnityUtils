using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Collections;

public class U001_AvatarUpLoad : MonoBehaviour
{
    public enum DownloadMethod
    {
        Coroutine,
        UniTask
    }

    [Header("UI Components")]
    [SerializeField] private Image imageComponent;
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private DownloadMethod downloadMethod = DownloadMethod.Coroutine;

    public void LoadAvatarFromURL()
    {
        string imageUrl = textComponent.text.Trim();

        if (string.IsNullOrEmpty(imageUrl))
        {
            Debug.LogWarning("⚠️ Image URL is empty. Please provide a valid URL.");
            return;
        }

        switch (downloadMethod)
        {
            case DownloadMethod.Coroutine:
                StartCoroutine(DownloadImage_Coroutine(imageUrl));
                break;

            case DownloadMethod.UniTask:
                DownloadImage_UniTask(imageUrl).Forget();
                break;
        }
    }

    /// <summary>
    /// Download image using Coroutine
    /// </summary>
    private IEnumerator DownloadImage_Coroutine(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApplyImage(((DownloadHandlerTexture)request.downloadHandler).texture);
                Debug.Log($"✅ Image loaded successfully (Coroutine) from: {url}");
            }
            else
            {
                Debug.LogError($"❌ Failed to load image (Coroutine). Error: {request.error}");
            }
        }
    }

    /// <summary>
    /// Download image using UniTask
    /// </summary>
    private async UniTaskVoid DownloadImage_UniTask(string url)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ApplyImage(DownloadHandlerTexture.GetContent(request));
                Debug.Log($"✅ Image loaded successfully (UniTask) from: {url}");
            }
            else
            {
                Debug.LogError($"❌ Failed to load image (UniTask). Error: {request.error}");
            }
        }
    }

    /// <summary>
    /// Convert Texture2D to Sprite and apply it to the Image UI.
    /// </summary>
    private void ApplyImage(Texture2D texture)
    {
        imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }
}
