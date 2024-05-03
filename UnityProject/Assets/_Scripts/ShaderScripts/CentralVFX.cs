using ChristinaCreatesGames.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CentralVFX : MonoBehaviour
{
    [SerializeField] private Material _material;
    [SerializeField] private List<MeshRenderer> _meshRenderers;
    [SerializeField] private List<VisualEffect> _dissolveEffects;
    [SerializeField] private VisualEffect _smoke1, _smoke2, _smokeDissapear, _spawnCloud;
    [SerializeField] private GameObject _eolicCentral, _nuclearCentral;
    [SerializeField] private float _transitionDuration;
    public static int PosID = Shader.PropertyToID("_MaskPos");
    [SerializeField] private SquashAndStretch Stretch;

    public IEnumerator Coroutine_DissolveCentral()
    {
        if(_smoke1 != null) _smoke1.Stop();
        if(_smoke2 != null) _smoke2.Stop();
        if (_dissolveEffects != null) Invoke(nameof(PlayDissolve), 0.3f);
        if (_meshRenderers != null)
        {
            foreach(MeshRenderer mr in _meshRenderers)
            {
                mr.material = _material;
            }
        }
        float lerp = 0;
        while (lerp < 1)
        {
            _material.SetVector("_MaskPos", Vector3.Lerp( new Vector3 (0, 0, 0), new Vector3 (0, 0, .25f), lerp ));
            lerp += Time.deltaTime * _transitionDuration;
            yield return null;
        }
        if (lerp >= 1)
        { 
            _smokeDissapear.Play();
            yield return new WaitForSeconds(0.3f);
            _nuclearCentral.SetActive(false);
            _eolicCentral.SetActive(true);
            Stretch.PlaySquashAndStretch();
        }
    }

    public void PlayDissolve()
    {
        foreach (VisualEffect disolver in _dissolveEffects)
        { 
            disolver.Play(); 
        }
    }

    public void CallCoroutine()
    {
        StartCoroutine(nameof(Coroutine_DissolveCentral));
    }
    private void Start()
    {
        _material.SetVector("_MaskPos", new Vector3(0, 0, 0));
        _eolicCentral.SetActive(false);
        _nuclearCentral.SetActive(true);
        _smoke1.Play();
        _smoke2.Play();
    }

    public void SpawnCloudVFX()
    { 
        _spawnCloud.Play();
    }
}
