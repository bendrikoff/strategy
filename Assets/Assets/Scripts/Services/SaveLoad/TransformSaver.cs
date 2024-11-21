using UnityEngine;

public class TransformSaver : MonoBehaviour, ISaveable
{
    public object CaptureState()
    {
        // Сохраняем позицию, ротацию и масштаб объекта
        return new TransformData
        {
            Position = transform.position,
            Rotation = transform.rotation,
            Scale = transform.localScale
        };
    }

    public void RestoreState(object state)
    {
        var data = (TransformData)state;
        transform.position = data.Position;
        transform.rotation = data.Rotation;
        transform.localScale = data.Scale;
    }

    [System.Serializable]
    public struct TransformData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
    }
}

