public interface ISaveable
{
    object CaptureState(); // Получить состояние объекта
    void RestoreState(object state); // Восстановить состояние объекта
}