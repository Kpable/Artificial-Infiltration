using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;


public enum Achievements
{
    Complete_All_Levels,
    Complete_Mission_1,
    Complete_Mission_2,
    Complete_Mission_3,
    Complete_Mission_4,
    Complete_Mission_5,
    Complete_Mission_1_In_X_Amount_Of_Time,
    Complete_Mission_2_In_X_Amount_Of_Time,
    Complete_Mission_3_In_X_Amount_Of_Time,
    Complete_Mission_4_In_X_Amount_Of_Time,
    Complete_Mission_5_In_X_Amount_Of_Time
}

public class SteamAchievements : MonoBehaviour {

    // UserStatsStored
    protected Callback<UserStatsStored_t> userStatsStoredCallback;
    // UserStatsReceived
    protected Callback<UserStatsReceived_t> userStatsReceivedCallback;
    // AchievementStored
    protected Callback<UserAchievementStored_t> userAchievementStoredCallback;

    private AppId_t gameId;

    void OnEnable() {
        if (SteamManager.Initialized)
        {
            gameId = SteamUtils.GetAppID();
            CSteamID currentUserID = SteamUser.GetSteamID();
            Debug.Log(name + " Current User ID:" + currentUserID.ToString());

            userStatsStoredCallback = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            userStatsReceivedCallback = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            userAchievementStoredCallback = Callback<UserAchievementStored_t>.Create(OnUserAchievementStored);



            SteamUserStats.RequestUserStats(currentUserID);

        }
    }

    bool SetAchievment(Achievements achievement)
    {
        if (SteamManager.Initialized)
        {
            // Set the given acheivement 
            SteamUserStats.SetAchievement(achievement.ToString());
            // Attempt to store the new stats. 
            return SteamUserStats.StoreStats();
        }

        return false;
    }

    void OnUserStatsStored(UserStatsStored_t callback)
    {
        // Verify the callback is coming from our game
        if(callback.m_nGameID == gameId.m_AppId)
        {
            // Verify Successful result
            if(callback.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log("Stored stats");

            }
            else if(callback.m_eResult == EResult.k_EResultInvalidParam)
            {
                // Some stats failed we should reretrieve stats to stay in sync. 
                Debug.Log("We broke some stat contraint, resyncing stats");
                UserStatsReceived_t cb = new UserStatsReceived_t();
                cb.m_eResult = EResult.k_EResultOK;
                cb.m_nGameID = gameId.m_AppId;
                OnUserStatsReceived(cb);
            }
            else
            {
                Debug.LogError("Error Storing stats: " + callback.m_eResult);
            }
        }
    }

    void OnUserStatsReceived(UserStatsReceived_t callback)
    {
        // Verify the callback is coming from our game
        if (callback.m_nGameID == gameId.m_AppId)
        {
            // Verify Successful result
            if (callback.m_eResult == EResult.k_EResultOK)
            {
                Debug.Log("Received stats for user: " + callback.m_steamIDUser.ToString());   
                // Load achievements
                           
            }
            else
            {
                Debug.LogError("Error recieving Stats: " + callback.m_eResult);
            }
        }
    }
    void OnUserAchievementStored(UserAchievementStored_t callback)
    {
        // Verify the callback is coming from our game
        if (callback.m_nGameID == gameId.m_AppId)
        {
            // Verify Successful result
            if (callback.m_nMaxProgress == 0)
            {
                Debug.Log("Achievement " + callback.m_rgchAchievementName + " Unlocked");

            }
            else
            {
                Debug.Log("Achievement: " + callback.m_rgchAchievementName + " callback " + 
                    "(" + callback.m_nCurProgress + "/" + callback.m_nMaxProgress + ") progress made.");
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
