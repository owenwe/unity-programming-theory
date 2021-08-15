using System;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private readonly Enum _scene = Scenes.Main;
    
    void Start()
    {
        // pos: x=0, y=-1.6, z=-12.75
        // rot: x=0, y=90, z=0
        // scale: .25
        //print($"Title Main Manager [IsChanging] = {TitleMainManager.Instance.IsChanging}");
        Vector3 pos = new Vector3(-0.6f, 1f, -26f);
        Gun g = Instantiate(Resources.Load<Gun>(TitleMainManager.Instance.Selected), pos, Quaternion.AngleAxis(90, Vector3.up));
        g.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
    }
}
