using UnityEditor;
using UnityEngine;
 using UnityEngine.UI;

[CustomEditor(typeof(EnemyOverworldStateMachine))]
public class EnemyEditor : Editor
{
    private RaycastHit _hit;

    private void OnSceneGUI() {
        EnemyOverworldStateMachine t = (EnemyOverworldStateMachine) target;
        Vector3 fovVector;

        Raycast(t.transform, t.transform.position, out fovVector);

        Handles.color = Color.white;
        Handles.DrawWireArc(fovVector, Vector3.up, Vector3.forward, 360, t.Radius);

        Vector3 viewAngle01 = DirectionFromAngle(t.transform.eulerAngles.y, -t.Angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(t.transform.eulerAngles.y, t.Angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fovVector, fovVector + viewAngle01 * t.Radius);
        Handles.DrawLine(fovVector, fovVector + viewAngle02 * t.Radius);

        if(t.PlayerDetected) {
            Handles.color = Color.green;
            Handles.DrawLine(fovVector, t.PlayerRef.transform.position);
        }

        Vector3 pos;

        if(!Application.isPlaying) {
            Raycast(t.transform, t.transform.position, out pos);
        } else {
            Raycast(t.transform, t.StartingPos, out pos);
        }

        float xLimit = Mathf.Clamp(t.XLimit, 3f, Mathf.Infinity);
        float zLimit = Mathf.Clamp(t.ZLimit, 3f, Mathf.Infinity);

        Vector3[] verts = new Vector3[]
        {
            new Vector3(pos.x - xLimit, pos.y, pos.z - zLimit),
            new Vector3(pos.x - xLimit, pos.y, pos.z + zLimit),
            new Vector3(pos.x + xLimit, pos.y, pos.z + zLimit),
            new Vector3(pos.x + xLimit, pos.y, pos.z - zLimit)
        };

        Handles.DrawSolidRectangleWithOutline(verts, new Color(0.5f, 0.5f, 0.5f, 0.1f), new Color(0, 0, 0, 1));
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void Raycast(Transform targetTransform, Vector3 original, out Vector3 newVector) {
        if(Physics.Raycast(targetTransform.position, targetTransform.TransformDirection(Vector3.down), out _hit,
            Mathf.Infinity)) {
            newVector = new Vector3(original.x, _hit.point.y, original.z);
        } else {
            newVector = original;
        }
    }

    public override void OnInspectorGUI() {
        EnemyOverworldStateMachine t = (EnemyOverworldStateMachine) target;

        DrawDefaultInspector();
        
        if (t.FloatingEnemy) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Float", EditorStyles.boldLabel);
            t.FloatSpeed = (float)EditorGUILayout.FloatField("Float Speed", t.FloatSpeed);
            t.FloatStrength = (float)EditorGUILayout.FloatField("Float Strength", t.FloatStrength);
        }
    }
}