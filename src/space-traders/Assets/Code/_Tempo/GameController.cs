using Assets.Code._Tempo;
using SQLite;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [Header("DataBase")]
    [SerializeField] private string _saveFileName = "save_data.db";
    [SerializeField] private int _saveSlot = 0;

    [Space]
    [SerializeField] private Button _buttonIncrease;
    [SerializeField] private Button _buttonDiscrease;
    [SerializeField] private TextMeshProUGUI _text;

    [Space]
    [SerializeField] private int _currentPoints;

    [SerializeField] private GameData _gameData = null;

    private void OnEnable()
    {
        _gameData = Load(_saveSlot);
        _currentPoints = _gameData.Points;
        PointsChanged();

        _buttonIncrease.onClick.AddListener(Increase);
        _buttonDiscrease.onClick.AddListener(Discrease);
    }

    private void OnDisable()
    {
        _buttonIncrease.onClick.RemoveListener(Increase);
        _buttonDiscrease.onClick.RemoveListener(Discrease);
    }

    private void Increase()
    {
        _currentPoints++;
        PointsChanged();
    }

    private void Discrease()
    {
        _currentPoints--;
        PointsChanged();
    }

    private void PointsChanged()
    {
        _text.text = _currentPoints.ToString();

        _gameData.Points = _currentPoints;
        Save(_gameData);
    }

    private GameData Load(int slotIndex)
    {
        var dbPath = Path.Combine(Application.persistentDataPath, _saveFileName);
        var dbConnection = new SQLiteConnection(dbPath);

        dbConnection.CreateTable<GameData>();
        var gameData = dbConnection.Find<GameData>(_saveSlot);
        if (gameData == null)
        {
            gameData = new();
            gameData.SaveSlotId = slotIndex;

            dbConnection.Insert(gameData);
        }

        dbConnection.Dispose();

        return gameData;
    }

    private void Save(GameData data)
    {
        var dbPath = Path.Combine(Application.persistentDataPath, _saveFileName);
        var dbConnection = new SQLiteConnection(dbPath);

        dbConnection.Update(_gameData);
        dbConnection.Dispose();
    }
}
