using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Leaderboards : MonoBehaviour {
    //public static Leaderboards S;                           // Making a singleton to make scores accessible 
    
    //public List<MissionData> missionBestTimes;          // List of all the missions' best time

    //void Awake()
    //{
    //    if(S == null)
    //    {
    //        S = this;
    //    }

    //    LoadBestTimes();
    //}
	
    //public void SaveBestTimes()
    //{
    //    GameData.SaveData("BestTimes.score", missionBestTimes);         // Save the best mission times
    //}

    //public void LoadBestTimes()
    //{
    //    Debug.Log("Loading Best Times");
    //    missionBestTimes = (List<MissionData>) GameData.LoadData("BestTimes.score");    // Load the saved best mission times. 
    //}



    //public void HandleScore(string levelName, DateTime score)
    //{
    //    //Debug.Log("Handling Score: " + score.Second);

    //    if (missionBestTimes.Count > 0)                     // Check if we have scores
    //    {
    //        missionBestTimes.ForEach(bestTime =>            // Search them all for current level
    //        {
    //            //Debug.Log("Level: " + bestTime.levelName);

    //            if (bestTime.levelName.Equals(levelName))   // Check if have a match
    //            {
    //                //Debug.Log("level found: " + levelName);

    //                if (score < bestTime.levelTime)         // Check to see if the best time is greater than the current score
    //                {
    //                    //Debug.Log("New best time");
    //                    bestTime.levelTime = score;             // Save the time. 
    //                    bestTime.recievedDate = DateTime.Now;   // Save the date recieved
    //                    SaveBestTimes();                        // Save new score to file
    //                }
    //            }
    //        });
    //    }
    //    else
    //    {
    //        //Debug.Log("Empty List");
    //        // There is no current mission data. Create it. 

    //        MissionData newBestTime = new MissionData();    
    //        newBestTime.levelName = levelName;
    //        newBestTime.levelTime = score;
    //        newBestTime.recievedDate = DateTime.Now;

    //        missionBestTimes.Add(newBestTime);      // Add the new data

    //        SaveBestTimes();                        // Save new score to file
    //    }
    //}

}

#region Future Code ?

//public Dictionary< string, List<DateTime> > Leaderboard;

//private int maxNumberOfScoresPerLevel = 10;

//public Dictionary<string, List<DateTime>> LoadLeaderboardData()
//{
//    Dictionary<string, List<DateTime>> data = null;
//    return data;
//}

//public void SaveLeaderboardData(Dictionary<string, List<DateTime>> data)
//{

//}

//public void HandleScore(string levelName, DateTime score)
//{
//    if ( Leaderboard.ContainsKey(levelName))                             // If this level is in the leaderboards
//    {

//        if( Leaderboard[levelName].Count < maxNumberOfScoresPerLevel)                          // if we have less than 10 scores for this level
//        {
//            Leaderboard[levelName].Add(score);                          // Add the score to the list
//            Leaderboard[levelName].OrderBy(time => time.Second);        // Then sort it. Default is ascending.
//        }
//        else    // Else, we have 10. This else always keeps it at 10 scores. 
//        {
//            Leaderboard[levelName].Remove( 
//                Leaderboard[levelName].Max() );                         // Remove the highest number of seconds. HighestTime=LowestScore
//            Leaderboard[levelName].Add(score);                          // Add the score to the list
//            Leaderboard[levelName].OrderBy(time => time.Second);        // Then sort it. Default is ascending.
//        }
//    }
//    else        //This level is not in the leaderboards, lets add it, then add the score. 
//    {
//        List<DateTime> scores = new List<DateTime>();   // Create new list of scores
//        scores.Add(score);                              // Add the first score
//        Leaderboard[levelName] = scores;                // Set this level's score list. 
//    }
//}
#endregion
