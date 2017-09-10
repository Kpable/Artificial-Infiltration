using UnityEngine;
using System.Collections;
using System;

public enum EdgeOfCube { Bottom, Right, Top, Left, BottomRightCorner, BottomLeftCorner, TopRightCorner, TopLeftCorner, Lost }

// Probably not the best place to put this global enum. 
public enum Direction { Up, Left, Down, Right, Forward, Back }

// Probably not the best place for this static class. 
public static class EnumUtils
{

    #region Enum Direction
    //Extension method to get the Vector3 based on the direction. Not really necessary but 
    // thought it would be easier to select from a drop down rather than inputting a Vector3
    // in the editor. 
    public static Vector3 Vec(this Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return Vector3.up;
            case Direction.Left:
                return Vector3.left;
            case Direction.Down:
                return Vector3.down;
            case Direction.Right:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }

    //Extension method to get the Vector3 based on the Transform's direction. 
    public static Vector3 Trans(this Direction dir, Transform t)
    {
        switch (dir)
        {
            case Direction.Up:
                return t.up;
            case Direction.Left:
                return t.right * -1;
            case Direction.Down:
                return t.up * -1;
            case Direction.Right:
                return t.right;
            default:
                return t.up;
        }
    }

    #endregion Enum Direction End

    #region Enum EdgeOfCube
    
    // Returns the angle of edge on X-Z axis
    public static float EdgeToAngle(this EdgeOfCube edge)
    {
        float angle = 0;

        switch (edge)
        {
            case EdgeOfCube.Bottom:
                angle = 270f;
                break;
            case EdgeOfCube.Right:
                angle = 0f;
                break;
            case EdgeOfCube.Top:
                angle = 90f;
                break;
            case EdgeOfCube.Left:
                angle = 180f;
                break;
            case EdgeOfCube.BottomRightCorner:
                angle = 315f;
                break;
            case EdgeOfCube.BottomLeftCorner:
                angle = 225f;
                break;
            case EdgeOfCube.TopRightCorner:
                angle = 45f;
                break;
            case EdgeOfCube.TopLeftCorner:
                angle = 135f;
                break;
            case EdgeOfCube.Lost:
                angle = 0f;
                break;
            default:
                break;
        }

        return angle;
    }

    // Returns the rotation along the y axis of object's self based on the edge
    public static float EdgeToRotation(this EdgeOfCube edge)
    {

        float rotation = 0;

        switch (edge)
        {
            case EdgeOfCube.Bottom:
                rotation = 0;
                break;
            case EdgeOfCube.Right:
                rotation = 270f;
                break;
            case EdgeOfCube.Top:
                rotation = 180;
                break;
            case EdgeOfCube.Left:
                rotation = 90;
                break;

            // we dont want to return a diagonal rotation. 
            case EdgeOfCube.BottomRightCorner:
            case EdgeOfCube.BottomLeftCorner:
            case EdgeOfCube.TopRightCorner:
            case EdgeOfCube.TopLeftCorner:
            case EdgeOfCube.Lost:
            default:
                break;
        }

        return rotation;
    }

    //Return the position of the edge, ignoring Y and Variable axes
    public static Vector3 Pos (this EdgeOfCube edge, CubeSpace cube)
    {
        Vector3 pos = cube.origin;

        //float offset = t.localScale.x / 2;
        //TODO fixed values often cause issues. 
        float offset = 0.5f;
        float edgePos = (cube.cubeSize / 2) - offset;
        Vector3 cubeOffset = cube.transform.position;


        switch (edge)
        {
            case EdgeOfCube.Bottom:
                pos.z = -edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.Right:
                pos.x = edgePos + cubeOffset.x;
                break;
            case EdgeOfCube.Top:
                pos.z = edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.Left:
                pos.x = -edgePos + cubeOffset.x;
                break;
            case EdgeOfCube.BottomRightCorner:
                pos.x = edgePos + cubeOffset.x;
                pos.z = -edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.BottomLeftCorner:
                pos.x = -edgePos + cubeOffset.x;
                pos.z = -edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.TopRightCorner:
                pos.x = edgePos + cubeOffset.x;
                pos.z = edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.TopLeftCorner:
                pos.x = -edgePos + cubeOffset.x;
                pos.z = edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.Lost:
            default:
                break;
        }

        return pos;
    }

    //Return the position of the edge, retaining passed in position axes. 
    public static Vector3 Pos(this EdgeOfCube edge, Transform target)
    {
        Vector3 pos = target.position;
        CubeSpace cube = CubeEdges.DetectNearestCube(target);

        float targetOffset = target.localScale.x / 2;
        float edgePos = (cube.cubeSize / 2) - targetOffset;
        Vector3 cubeOffset = cube.transform.position; 


        switch (edge)
        {
            case EdgeOfCube.Bottom:
                pos.z = -edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.Right:
                pos.x = edgePos + cubeOffset.x;
                break;
            case EdgeOfCube.Top:
                pos.z = edgePos + cubeOffset.z;
                break;
            case EdgeOfCube.Left:
                pos.x = -edgePos + cubeOffset.x;
                break;
            case EdgeOfCube.BottomRightCorner:
                pos.x = edgePos + cubeOffset.x; 
                pos.z = -edgePos + cubeOffset.z; 
                break;
            case EdgeOfCube.BottomLeftCorner:
                pos.x = -edgePos + cubeOffset.x; 
                pos.z = -edgePos + cubeOffset.z; 
                break;
            case EdgeOfCube.TopRightCorner:
                pos.x = edgePos + cubeOffset.x; 
                pos.z = edgePos + cubeOffset.z; 
                break;
            case EdgeOfCube.TopLeftCorner:
                pos.x = -edgePos + cubeOffset.x; 
                pos.z = edgePos + cubeOffset.z; 
                break;
            case EdgeOfCube.Lost:
            default:
                break;
        }

        return pos;
    }

    public static float LeftRightAxis(this EdgeOfCube edge, Vector3 vector)
    {
        float value = 0;

        switch (edge)
        {
            case EdgeOfCube.Bottom:
                value = vector.x;
                break;
            case EdgeOfCube.Top:
                value = -vector.x;
                break;
            case EdgeOfCube.Right:
                value = vector.z;
                break;
            case EdgeOfCube.Left:
                value = -vector.z;
                break;
            case EdgeOfCube.BottomRightCorner:
            case EdgeOfCube.BottomLeftCorner:
            case EdgeOfCube.TopRightCorner:
            case EdgeOfCube.TopLeftCorner:
            case EdgeOfCube.Lost:
            default:
                break;
        }

        return value;
    }

    #endregion Enum EdgeOfCube End

    #region DateTime Extension

    public static string PrettyTime(this DateTime time)
    {
        return time.Minute.ToString("00") + ":" + time.Second.ToString("00") + "." + time.Millisecond.ToString("000");
    }

    #endregion
}

public static class CubeEdges
{

    // Returns the Cube info of the nearest Cube
    public static CubeSpace DetectNearestCube(Transform t)
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        CubeSpace closestCube = null;

        float minDistance = 0;
        float currentDistance = 0;


        for (int i = 0; i < cubes.Length; i++)
        {
            CubeSpace currentCube = cubes[i].GetComponent<CubeSpace>();
            if (currentCube == null)
            {
                Debug.LogError("Cube " + cubes[i].name + " Does not have a CubeSize script on it but it is tagged as a Cube");
                // I suppose i could add it here but i dont know how its values will behave. Maybe they'll just work.
                break;
            }

            currentDistance = Vector3.Distance(t.position, currentCube.origin);

            if (i == 0 || minDistance > currentDistance )
            {
                minDistance = currentDistance;
                closestCube = currentCube;
            }
            
        }

        //Debug.Log("DetectNearestCube: Closest Cube-" + closestCube.transform.name + " at " + closestCube.origin);

        return closestCube;
    }

    // Returns the angle of edge on X-Z axis
    public static EdgeOfCube AngleToEdge(float angle)
    {
        EdgeOfCube edge = EdgeOfCube.Lost;

        // Angle may not be perfect
        float roundedAngle = Mathf.Round(angle / 45) * 45;

        //// Lets stick with small numbers -360 to 360 degrees
        if (roundedAngle >= 360) roundedAngle -= 360;
        if (roundedAngle <= -360) roundedAngle += 360;

        // Bottom
        if (roundedAngle == 0 || roundedAngle == 360 || roundedAngle == -360) edge = EdgeOfCube.Right;
        // Left
        else if (roundedAngle == 90 || roundedAngle == -270) edge = EdgeOfCube.Top;
        // Top
        else if (roundedAngle == 180 || roundedAngle == -180) edge = EdgeOfCube.Left;
        // Right
        else if (roundedAngle == 270 || roundedAngle == -90) edge = EdgeOfCube.Bottom;
        // BottomRight
        else if (roundedAngle == 315 || roundedAngle == -45) edge = EdgeOfCube.BottomRightCorner;
        // BottomLeft
        else if (roundedAngle == 225 || roundedAngle == -270) edge = EdgeOfCube.BottomLeftCorner;
        // TopRight
        else if (roundedAngle == 45 || roundedAngle == -180) edge = EdgeOfCube.TopRightCorner;
        // TopLeft
        else if (roundedAngle == 135 || roundedAngle == -90) edge = EdgeOfCube.TopLeftCorner;
        // Lost        
        else edge = EdgeOfCube.Lost;


        return edge;
    }

    // Returns the edge based on the rotation along the y axis of Object's self
    public static EdgeOfCube RotationToEdge(float angle)
    {
        EdgeOfCube edge = EdgeOfCube.Lost;

        // Angle may not be perfect based on initial position
        float roundedAngle = Mathf.Round(angle / 90) * 90;

        //// Lets stick with small numbers -360 to 360 degrees
        if (roundedAngle >= 360) roundedAngle -= 360;
        if (roundedAngle <= -360) roundedAngle += 360;

        // Bottom
        // Left and right is the X axis: - 0 +
        if (roundedAngle == 0 || roundedAngle == 360 || roundedAngle == -360)
        {
            edge = EdgeOfCube.Bottom;
        }
        // Left
        // Left and right is the Z axis reversed: + 0 -
        else if (roundedAngle == 90 || roundedAngle == -270)
        {
            edge = EdgeOfCube.Left;
        }
        // Top
        // Left and right is the X axis reversed: + 0 -
        else if (roundedAngle == 180 || roundedAngle == -180)
        {
            edge = EdgeOfCube.Top;
        }
        // Right
        // Left and right is the X axis: - 0 +
        else if (roundedAngle == -90 || roundedAngle == 270)
        {
            edge = EdgeOfCube.Right;
        }

        return edge;
    }

    //Clamp position to Cube Edges
    public static Vector3 Clamp(Transform t)
    {
        CubeSpace nearestCube = DetectNearestCube(t);

        float offset = t.localScale.x / 2;
        float edgeDistance = (nearestCube.cubeSize / 2) - offset;
        //Debug.Log("edgeDistance = " + edgeDistance.ToString());
        Vector3 pos = t.position;

        EdgeOfCube edge = RotationToEdge(t.rotation.eulerAngles.y);
        //Debug.Log("edge = " + edge);
        Vector3 cubePos = nearestCube.origin;
        //Debug.Log("cubepose = " + cubePos.ToString());

        switch (edge)
        {
            // Left and right is the X axis: - 0 +
            case EdgeOfCube.Bottom:
                pos.x = Mathf.Clamp(pos.x, -edgeDistance + cubePos.x, edgeDistance + cubePos.x);
                pos.z = Mathf.Clamp(pos.z, -edgeDistance + cubePos.z, -edgeDistance + cubePos.z);
                break;
            // Left and right is the X axis: - 0 +
            case EdgeOfCube.Right:
                pos.x = Mathf.Clamp(pos.x, edgeDistance + cubePos.x, edgeDistance + cubePos.x);
                pos.z = Mathf.Clamp(pos.z, -edgeDistance + cubePos.z, edgeDistance + cubePos.z);
                break;
            // Left and right is the X axis reversed: + 0 -
            case EdgeOfCube.Top:
                pos.x = Mathf.Clamp(pos.x, -edgeDistance + cubePos.x, edgeDistance + cubePos.x);
                pos.z = Mathf.Clamp(pos.z, edgeDistance + cubePos.z, edgeDistance + cubePos.z);
                break;
            // Left and right is the Z axis reversed: + 0 -
            case EdgeOfCube.Left:
                pos.x = Mathf.Clamp(pos.x, -edgeDistance + cubePos.x, -edgeDistance + cubePos.x);
                pos.z = Mathf.Clamp(pos.z, -edgeDistance + cubePos.z, edgeDistance + cubePos.z);
                break;

            // No need to handle these cases
            case EdgeOfCube.BottomRightCorner:
            case EdgeOfCube.BottomLeftCorner:
            case EdgeOfCube.TopRightCorner:
            case EdgeOfCube.TopLeftCorner:
            case EdgeOfCube.Lost:
            default:
                break;
        }
        return pos;
    }

    //Finds what edge the object is on. 
    // Should not be called on Awake because Detect Closest requires Pos which requires CubeSize.Awake to complete
    public static EdgeOfCube DetectEdge(Transform t)
    {
        EdgeOfCube edge = EdgeOfCube.Lost;
        CubeSpace nearestCube = DetectNearestCube(t);
        
        float targetOffset = t.localScale.x / 2;

        float edgeDistance = (nearestCube.cubeSize / 2) - targetOffset;

        Vector3 cubePos = nearestCube.origin;

        //Debug.Log("DetectEdge: edgePos=" + edgePos);

        // Corners

        // TopRightCorner
        if (t.position.x == edgeDistance + cubePos.x && t.position.z == edgeDistance + cubePos.z)
            edge = EdgeOfCube.TopRightCorner;
        // TopLeftCorner
        else if (t.position.x == -edgeDistance + cubePos.x && t.position.z == edgeDistance + cubePos.z)
            edge = EdgeOfCube.TopLeftCorner;
        // BottomRightCorner
        if (t.position.x == edgeDistance + cubePos.x && t.position.z == -edgeDistance + cubePos.z)
            edge = EdgeOfCube.TopRightCorner;
        // BottomLeftCorner
        else if (t.position.x == -edgeDistance + cubePos.x && t.position.z == -edgeDistance + cubePos.z)
            edge = EdgeOfCube.TopLeftCorner;

        // Sides

        // Bottom - fixed negative z pos, variable x pos
        else if (t.position.z == -edgeDistance + cubePos.z && t.position.x > -edgeDistance + cubePos.x && t.position.x < edgeDistance + cubePos.x)
            edge = EdgeOfCube.Bottom;
        // Top -    fixed positive z pos, variable x pos
        else if (t.position.z == edgeDistance + cubePos.z && t.position.x > -edgeDistance + cubePos.x && t.position.x < edgeDistance + cubePos.x)
            edge = EdgeOfCube.Top;
        // Left -   fixed negative x pos, variable z pos
        else if (t.position.x == -edgeDistance + cubePos.x && t.position.z > -edgeDistance + cubePos.z && t.position.z < edgeDistance + cubePos.z)
            edge = EdgeOfCube.Left;
        // Right -  fixed positive x pos, variable z pos
        else if (t.position.x == edgeDistance + cubePos.x && t.position.z > -edgeDistance + cubePos.z && t.position.z < edgeDistance + cubePos.z)
            edge = EdgeOfCube.Right;

        // If all else failed, you are lost.. I'm sorry but i cannot help you, good luck out there beyond the edges.
        else
        {
            edge = EdgeOfCube.Lost;
            //Debug.Log(t.gameObject.name + ": Is lost beyond the edges. Position: " + t.position.ToString());
        }

        if (edge == EdgeOfCube.Lost)
            edge = DetectClosestEdge(t);

        return edge;
    }

    //Finds what edge the object is closest to. 
    public static EdgeOfCube DetectClosestEdge(Transform t, bool includeCorners = false)
    {
        //float offset = t.localScale.x / 2;

        CubeSpace nearestCube = DetectNearestCube(t);

        //float edgePos = (nearestCube.cubeSize / 2) - offset;

        // We want to ignore the height of object
        Vector3 objectPos = new Vector3(t.position.x, 0, t.position.z);

        EdgeOfCube closestEdge = EdgeOfCube.Lost;
        float minDistance = 0;
        float currentDistance = 0;

        //Debug.Log("DetectClosestEdge: edgePos=" + edgePos);

        // Only Search first four edges by default
        int maxLength = (int)EdgeOfCube.BottomRightCorner;

        // Include corners if desired. 
        if (includeCorners)
            maxLength = (int)EdgeOfCube.Lost;

        for (int i = 0; i < maxLength; i++)
        {
            currentDistance = Vector3.Distance(objectPos, ((EdgeOfCube)i).Pos(nearestCube));

            //Debug.Log("Detect Closest Edge: Checking " + t.name + " with edge " + (EdgeOfCube)i + " min distance " + minDistance + " current distance " + currentDistance +
            //    " where object pos is " + objectPos.ToString() + " and edge pos is " + ((EdgeOfCube)i).Pos());

            if (i == 0)
            {
                minDistance = currentDistance;
                closestEdge = (EdgeOfCube)i;
            }
            else
            {
                if (minDistance > currentDistance)
                {
                    minDistance = currentDistance;
                    closestEdge = (EdgeOfCube)i;
                }
            }
        }

        //Debug.Log("DetectClosestEdge: Closest Edge-" + closestEdge);


        return closestEdge;
    }

}

public static class Utils {
    public const string PlayerSaveFilename = "Player_Save_Data.datacore";
    public const string GameSettingsFilename = "Settings.datacore";
    public const float DesiredAspectRatio = 16f/9f;

    public static string GetPlayerSaveFilePath()
    {
        return Application.persistentDataPath + "/" + PlayerSaveFilename;
    }

    public static string GetSettingsFilePath()
    {
        return Application.persistentDataPath + "/" + GameSettingsFilename;
    }

    public static IEnumerator WaitForRealSeconds(float duration)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + duration)
        {
            yield return null;
        }
    }

    public static void StopTime()
    {
        Time.timeScale = 0;
    }

    public static void StartTime()
    {
        Time.timeScale = 1;
    }
}
