using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObjScript : MonoBehaviour
{
    [SerializeField] private Sprite baseImage;
    [SerializeField] private float animationDelay;
    [SerializeField] private float hitImageSize;

    private SpriteRenderer hitImage;

    private WaitForSecondsRealtime waitAnimationDelay;

    private void Awake()
    {
        hitImage = GetComponent<SpriteRenderer>();
        waitAnimationDelay = new WaitForSecondsRealtime(animationDelay);
    }

    public void Show(Vector3 movePos)
    {
        transform.position = movePos;
        StartCoroutine(HitAnimation());
    }

    public void ChangeImage()
    {
        hitImage.sprite = baseImage;
    }

    public void ChangeImage(Sprite sprite)
    {
        hitImage.sprite = sprite;
    }

    public GameObject ChangeInfo(float radius)
    {
        radius *= hitImageSize;
        transform.localScale = new Vector3(radius, radius, radius);
        return gameObject;
    }
    
    public GameObject ChangeInfo(float radius, float animationDelay)
    {
        radius *= hitImageSize;
        transform.localScale = new Vector3(radius, radius, radius);
        this.animationDelay = animationDelay;
        return gameObject;
    }

    IEnumerator HitAnimation()
    {
        yield return waitAnimationDelay;
        gameObject.SetActive(false);
    }
}
