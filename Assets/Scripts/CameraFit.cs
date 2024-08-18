using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFit : MonoBehaviour
{

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Vector2 fieldSize = LevelManager.instanse.fieldSize;
        // + 2 бо поле
        float fieldHeight = fieldSize.y + 2.0f;
        float fieldWidth = fieldSize.x + 2.0f;
        // довжина проекції на камеру
        // бок кут нахилу 30*
        float cameraAngle = 30.0f;
        float projectionHeight = fieldHeight * Mathf.Cos(Mathf.Abs(cameraAngle) * Mathf.Deg2Rad);

        float aspectRatio = (float)Screen.width / (float)Screen.height;
        float orthographicSize;
       
        if (aspectRatio > 1)
        {
            orthographicSize = projectionHeight / 2f;
        }
        else
        { 
            orthographicSize = fieldWidth / 2f / aspectRatio; 
        }

        camera.orthographicSize = orthographicSize;
    }


}
