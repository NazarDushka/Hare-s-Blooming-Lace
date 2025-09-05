using System.Collections.Generic;
using UnityEngine;

public class SceneDataCarrier : MonoBehaviour
{
    // Статический экземпляр, доступный из любого места
    public static SceneDataCarrier Instance;

    // Переменная, которую мы хотим передать
    public bool isWhiteIn = false;
    public bool isPlayedBeforeSovidze = false;
    public bool isPlayedBeforeEjidze = false;

    // Сцены, в которых уже была загружена анимация "OncePlayed"
    public HashSet<string> scenesWithOncePlayedLoading = new HashSet<string>();


    private void Awake()
    {
        // Если экземпляр еще не существует, мы назначаем себя
        if (Instance == null)
        {
            Instance = this;
            // Сохраняем этот объект при загрузке новой сцены
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Если экземпляр уже есть, уничтожаем этот дубликат
            Destroy(gameObject);
        }
    }
}