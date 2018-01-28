using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Donnina : MonoBehaviour
{
    private Material donninaMat;
    private Material insegnaMat;
    public Texture donnina1;
    public Texture donnina2;

    void Start()
    {
        donninaMat = GetComponent<MeshRenderer>().materials[3];
        insegnaMat = GetComponent<MeshRenderer>().materials[0];
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            donninaMat.mainTexture = donnina1;
            yield return new WaitForSeconds(0.7f);
            donninaMat.mainTexture = donnina2;
            yield return new WaitForSeconds(0.7f);

            var random = Random.Range(0f, 1f);

            if (random > 0.75f)
            {
                insegnaMat.DOFade(0f, 0.1f).SetEase(Ease.InOutSine).OnComplete(() =>
                    {
                        insegnaMat.DOFade(1f, 0.1f).SetEase(Ease.InOutSine);
                    });
            }
        }
    }
}