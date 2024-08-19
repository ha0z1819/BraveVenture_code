using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraControl : MonoBehaviour
{
   private CinemachineConfiner2D confiner2D;
   public CinemachineImpulseSource impulseSource;
    public VoidEnentSO cameraShakeEvent;
   public void Awake()
   {
        confiner2D=GetComponent<CinemachineConfiner2D>();
   }

    public void OnEnable()
    {
        cameraShakeEvent.OnEnentRaised += OncameraShakeEvent;
    }
    private void OnDisable()
    {
        cameraShakeEvent.OnEnentRaised -= OncameraShakeEvent;
    }
    private void OncameraShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }


    // TODO :场景切换后更改
   private void Start()
   {
        GetNewCameraBounds();
   }

   private void GetNewCameraBounds()
   {
        GameObject[] obj=GameObject.FindGameObjectsWithTag("Bounds");
        if (obj == null)
            return;
            
        confiner2D.m_BoundingShape2D=obj[0].GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
   }
}
