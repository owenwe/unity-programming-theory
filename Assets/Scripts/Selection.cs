using System;
using UnityEngine;

public class Selection : MonoBehaviour
{
    private MeshRenderer _mr;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private float alpha = 1.0f;
    [SerializeField] private bool isOpaque;
    
    // TODO implement a better way to fade the selections in and out
    private Material _opaqueMaterial;
    private Material _transparentMaterial;

    public string GetWeaponPrefab => gameObject.transform.GetChild(0).GetComponent<ProjectileWeapon>().PrefabPath;

    private void Start()
    {
        _mr = GetBaseGameObject().GetComponentInChildren<MeshRenderer>();
        _opaqueMaterial = Instantiate(Resources.Load<Material>("Materials/Opaque"));
        _transparentMaterial = Instantiate(Resources.Load<Material>("Materials/Transparent"));
    }
    
    private void Update()
    {
        if (!isMoving) return;
        
        var currentAlpha = Mathf.Round(_mr.material.color.a);
        var finalAlpha = Mathf.Round(alpha);

        if (Mathf.Approximately(currentAlpha, finalAlpha))
        {
            isMoving = false;
            if (Mathf.Approximately(alpha, 1.0f))
            {
                _mr.material = _opaqueMaterial;
                return;
            }
            var c = _mr.material.color;
            c.a = alpha;
            _mr.material.SetColor("_Color", c);
        }
        else
        {
            var c = _mr.material.color;
            c.a = isOpaque ? c.a + TitleMainManager.Instance.alphaChangeStep : c.a - TitleMainManager.Instance.alphaChangeStep;
            _mr.material.SetColor("_Color", c);
        }
    }

    public GameObject GetBaseGameObject()
    {
        return gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
    }

    public void UpdateTransparency(float newAlpha, Boolean newOpaque)
    {
        if (_mr != null)
        {
            _mr.material = _transparentMaterial;
        }
        isMoving = true;
        isOpaque = newOpaque;
        alpha = newAlpha;
    }
}
