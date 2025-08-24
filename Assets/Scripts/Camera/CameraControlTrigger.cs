using UnityEngine;
using Unity.Cinemachine;
using UnityEditor;

/// <summary>
/// Controla triggers gerais para gerenciar ações da camera
/// Pode realizar troca de cameras, camera panoramica e AutoScroll. Ao player entrar ou sair do trigger. 
/// <summary>
public class CameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;
    private Collider2D _coll;

    // Inicializa o componente Collider2D.
    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    // Aciona ações da câmera quando o jogador entra no gatilho.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Ação de Panning
        if (customInspectorObjects.panCameraOnContact)
        {
            CameraManager.Instance.PanCameraOnContact(
                customInspectorObjects.panDistance,
                customInspectorObjects.panTime,
                (CameraManager.PanDirection)customInspectorObjects.panDirection,
                false
            );
        }

        // Ação de Auto-Scroll
        if (customInspectorObjects.autoScrollAction)
        {
            if (customInspectorObjects.scrollActionType == AutoScrollAction.Start)
            {
                CameraManager.Instance.StartAutoScroll(
                    collision.transform.position,
                    customInspectorObjects.scrollDirection,
                    customInspectorObjects.scrollSpeed
                );
            }
            else if (customInspectorObjects.scrollActionType == AutoScrollAction.Stop)
            {
                CameraManager.Instance.StopAutoScroll();
            }
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
/// <summary>
/// Declaração de uma classe para armazenar variáveis que serão exibidas no Inspector.
/// <summary>
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;
    public bool autoScrollAction = false;

    [HideInInspector] public AutoScrollAction scrollActionType;
    [HideInInspector] public Vector2 scrollDirection = Vector2.right;
    [HideInInspector] public float scrollSpeed = 3f;

    [HideInInspector] public CinemachineCamera cameraOnLeft;
    [HideInInspector] public CinemachineCamera cameraOnRight;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

public enum PanDirection { Up, Down, Left, Right }

public enum AutoScrollAction { Start, Stop }

[CustomEditor(typeof(CameraControlTrigger))]

/// <summary>
/// Editor personalizado para CameraControlTrigger.
/// <summary>
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

        if (cameraControlTrigger.customInspectorObjects.autoScrollAction)
        {
            cameraControlTrigger.customInspectorObjects.scrollActionType =
                (AutoScrollAction)EditorGUILayout.EnumPopup("Scroll Action", cameraControlTrigger.customInspectorObjects.scrollActionType);
        }

        if (cameraControlTrigger.customInspectorObjects.scrollActionType == AutoScrollAction.Start)
        {
            cameraControlTrigger.customInspectorObjects.scrollDirection =
                EditorGUILayout.Vector2Field("Scroll Direction", cameraControlTrigger.customInspectorObjects.scrollDirection);

            cameraControlTrigger.customInspectorObjects.scrollSpeed =
                EditorGUILayout.FloatField("Scroll Speed", cameraControlTrigger.customInspectorObjects.scrollSpeed);
        }

        if (GUI.changed) EditorUtility.SetDirty(cameraControlTrigger);
    }
}