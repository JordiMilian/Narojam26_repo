using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] AudioSource intro, loop;
    [SerializeField] AudioClip clipIntro;


    private void Start()
    {
        StartCoroutine(waitForLoop());
    }
    IEnumerator waitForLoop()
    {
        yield return new WaitForSeconds(clipIntro.length);
        loop.Play();
    }
}
