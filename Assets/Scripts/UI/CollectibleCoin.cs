using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    Vector3 origin;
    float age;
    bool collected;
    void Awake()
    {
        origin = transform.localPosition;
    }
    void Update()
    {
        age += Time.deltaTime;
        transform.localPosition = origin + Vector3.up * (Mathf.Sin(age * 3) * .08f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.GetComponentInParent<Player>() || !MobileGameUI.Instance || !MobileGameUI.Instance.IsPlaying) return;
        collected = true;
        MobileGameUI.Instance.AddItem();
        gameObject.SetActive(false);
    }
}
