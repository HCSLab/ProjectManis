using UnityEngine;
using UnityEngine.SceneManagement;

namespace DungeonKIT
{
    public class SaveManager : MonoBehaviour
    {
        public static void Save()
        {
            PlayerPrefs.SetInt("Saved_HP", PlayerStats.Instance.HP.max);

            PlayerPrefs.SetInt("Saved_Money", PlayerStats.Instance.money);
            PlayerPrefs.SetInt("Saved_Bottles", PlayerStats.Instance.bottles);

            PlayerPrefs.SetString("Saved_Level", SceneManager.GetActiveScene().name);
        }

        public static void Load()
        {
            PlayerStats.Instance.HP = new DoubleInt(PlayerPrefs.GetInt("Saved_HP"), PlayerPrefs.GetInt("Saved_HP"));

            PlayerStats.Instance.money = PlayerPrefs.GetInt("Saved_Money");
            PlayerStats.Instance.bottles = PlayerPrefs.GetInt("Saved_Bottles");
        }


    }
}