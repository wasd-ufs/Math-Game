using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using UnityEditor;

/// <summary>
/// Controla triggers para gerenciar ações da camera, como panning e troca de cameras, quando o jogador entra ou sai da área.
/// </summary>
public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;
    private Collider2D _coll;

    // Inicializa o componente Collider2D.
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    // Aciona o panning da camera quando o jogador entra no gatilho.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && customInspectorObjects.panCameraOnContact)
        {
            CameraManager.Instance.PanCameraOnContact(
                customInspectorObjects.panDistance,
                customInspectorObjects.panTime,
                (CameraManager.PanDirection)customInspectorObjects.panDirection,
                false
            );
        }
    }

    // Gerencia troca de cameras e panning ao sair do gatilho, com base na direção de saída.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if (customInspectorObjects.swapCameras &&
                customInspectorObjects.cameraOnLeft != null &&
                customInspectorObjects.cameraOnRight != null)
            {
                CameraManager.Instance.SwapCamera(
                    customInspectorObjects.cameraOnLeft,
                    customInspectorObjects.cameraOnRight,
                    exitDirection
                );
            }

            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.Instance.PanCameraOnContact(
                    customInspectorObjects.panDistance,
                    customInspectorObjects.panTime,
                    (CameraManager.PanDirection)customInspectorObjects.panDirection,
                    true
                );
            }
        }
    }
}

[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineCamera cameraOnLeft;
    [HideInInspector] public CinemachineCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection { Up, Down, Left, Right }

[CustomEditor(typeof(CameraControlTrigger))]
public class MyScriptEditor : Editor
{
    CameraControlTrigger cameraControlTrigger;

    // Inicializa a referência ao componente CameraControlTrigger.
    private void OnEnable()
    {
        cameraControlTrigger = (CameraControlTrigger)target;
    }

    // Personaliza a interface do Inspector para exibir campos condicionalmente.
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (cameraControlTrigger.customInspectorObjects.swapCameras)
        {
            cameraControlTrigger.customInspectorObjects.cameraOnLeft =
                EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObjects.cameraOnLeft, typeof(CinemachineCamera), true) as CinemachineCamera;

            cameraControlTrigger.customInspectorObjects.cameraOnRight =
                EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObjects.cameraOnRight, typeof(CinemachineCamera), true) as CinemachineCamera;
        }

        if (cameraControlTrigger.customInspectorObjects.panCameraOnContact)
        {
            cameraControlTrigger.customInspectorObjects.panDirection =
                (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", cameraControlTrigger.customInspectorObjects.panDirection);

            cameraControlTrigger.customInspectorObjects.panDistance =
                EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
            cameraControlTrigger.customInspectorObjects.panTime =
                EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObjects.panTime);
        }

        if (GUI.changed) EditorUtility.SetDirty(cameraControlTrigger);
    }
}