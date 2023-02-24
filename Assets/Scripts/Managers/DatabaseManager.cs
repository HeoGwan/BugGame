using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class DatabaseManager : MonoBehaviour
{
    DatabaseReference databaseReference;

    void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void GetData()
    {
        databaseReference.Child("users").OrderByChild("score").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error Database");
                return;
            }

            if (!task.IsCompleted)
            {
                Debug.LogError("Fail Get");
                return;
            }

            DataSnapshot snapshot = task.Result;

            foreach (DataSnapshot child in snapshot.Children)
            {
                GameManager.instance.scoreManager.GetScoreData(
                    child.Child("nickname").Value.ToString(),
                    child.Child("date").Value.ToString(),
                    child.Child("score").Value.ToString());
            }
        });
    }

    public bool WriteData(string nickname, int score)
    {
        try
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            DatabaseReference data = databaseReference.Child("users").Push();

            data.Child("nickname").SetValueAsync(nickname);
            data.Child("date").SetValueAsync(date);
            data.Child("score").SetValueAsync(score);

            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            return false;
        }
    }
}
