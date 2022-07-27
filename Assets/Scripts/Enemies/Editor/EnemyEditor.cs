using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyOverworldStateMachine))]
public class EnemyEditor : Editor
{
    private void OnSceneGUI() {
        EnemyOverworldStateMachine t = (EnemyOverworldStateMachine) target;
        Handles.color = Color.white;
        Handles.DrawWireArc(t.transform.position, Vector3.up, Vector3.forward, 360, t.Radius);

        Vector3 viewAngle01 = DirectionFromAngle(t.transform.eulerAngles.y, -t.Angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(t.transform.eulerAngles.y, t.Angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(t.transform.position, t.transform.position + viewAngle01 * t.Radius);
        Handles.DrawLine(t.transform.position, t.transform.position + viewAngle02 * t.Radius);

        if(t.PlayerDetected) {
            Handles.color = Color.green;
            Handles.DrawLine(t.transform.position, t.PlayerRef.transform.position);
        }

        Vector3 pos = t.transform.position;

        Vector3[] verts = new Vector3[]
        {
            new Vector3(pos.x - t.XLimit, pos.y, pos.z - t.ZLimit),
            new Vector3(pos.x - t.XLimit, pos.y, pos.z + t.ZLimit),
            new Vector3(pos.x + t.XLimit, pos.y, pos.z + t.ZLimit),
            new Vector3(pos.x + t.XLimit, pos.y, pos.z - t.ZLimit)
        };

        Handles.DrawSolidRectangleWithOutline(verts, new Color(0.5f, 0.5f, 0.5f, 0.1f), new Color(0, 0, 0, 1));
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}