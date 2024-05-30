using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RandomColorChanger : MonoBehaviour
{
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Change()
    {
        _renderer.material.color =
            Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }
}