using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainEntry : MonoBehaviour
{
    // Start is called before the first frame update
    UIControl uiControl;

    void Start()
    {
        ReflectionManager.Instance.CreateAllMethodPackages();
        uiControl = GameObject.FindObjectOfType<UIControl>();
        uiControl.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        uiControl.FlowUpdate(dt);

    }
}
