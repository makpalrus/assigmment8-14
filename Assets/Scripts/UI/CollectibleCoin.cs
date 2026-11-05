using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    bool collected;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected || !other.GetComponentInParent<Player>() || !GameUI.Instance || !GameUI.Instance.IsPlaying) return;
        collected = true;
        GameUI.Instance.AddItem();
        gameObject.SetActive(false);
    }
}
