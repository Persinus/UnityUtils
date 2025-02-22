using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class U004_DelayedHealthBar : MonoBehaviour
{
    public Image healthBar;         // Thanh máu chính
    public Image delayedHealthBar;  // Thanh máu "ảo" (hiệu ứng vàng/trắng)
    public Button damageButton;     // Nút bấm để gây sát thương

    private float currentHealth = 1f; // Máu hiện tại (1f = 100%)
    private float maxHealth = 1f;    // Máu tối đa
    private float delayTime = 0.5f;    // Thời gian trễ trước khi giảm máu ảo
    private float shrinkSpeed = 0.5f; // Tốc độ giảm của thanh màu vàng/trắng

    private void Start()
    {
        //damageButton.onClick.AddListener(() => TakeDamage(0.2f)); // Nhấn nút để mất 20% máu
    }

    public void TakeDamage(float damagePercent)
    {
        if (currentHealth <= 0) return; // Nếu hết máu thì không giảm nữa

        currentHealth -= damagePercent;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.fillAmount = currentHealth;

        // Bắt đầu hiệu ứng tụt dần của thanh máu "ảo"
        StopAllCoroutines();
        StartCoroutine(DelayedHealthDrop());
    }

    private IEnumerator DelayedHealthDrop()
    {
        yield return new WaitForSeconds(delayTime); // Chờ 2 giây trước khi giảm máu ảo

        while (delayedHealthBar.fillAmount > currentHealth)
        {
            delayedHealthBar.fillAmount -= shrinkSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
