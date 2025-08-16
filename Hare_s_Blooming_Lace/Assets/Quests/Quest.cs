using UnityEngine;

[System.Serializable]
public class Quest
{
    public int id;
    public string title;
    [TextArea(3, 10)]
    public string description;

    public bool isActive = false; // Инициализируем в false по умолчанию
    public bool isCompleted = false; // Инициализируем в false по умолчанию

    public void Activate()
    {
        isActive = true;
    }

    public void Complete()
    {
        isCompleted = true;
        isActive = false;
    }
}