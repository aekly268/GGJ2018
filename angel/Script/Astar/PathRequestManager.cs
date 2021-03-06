﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> PathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance; // for static method
    PathFinding pathfinding;

    bool isProcessingPath;

    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<PathFinding>();
    }
    public static void RequsetPath(Vector3 pathStart,Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        //使用了new仍然是分配在Stack上
        instance.PathRequestQueue.Enqueue(newRequest); //Enqueue = add
        instance.TryProcessNext();
    }

    void TryProcessNext() {
        if (!isProcessingPath && PathRequestQueue.Count > 0) {
            currentPathRequest = PathRequestQueue.Dequeue();  //Dequeue 提取
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart ,currentPathRequest.pathEnd);
        }
    }

    public void FinshedProcessingPath(Vector3[] path,bool isSuccess) {
        currentPathRequest.callback(path, isSuccess);
        isProcessingPath = false;
        instance.TryProcessNext(); 
    }
    
    struct PathRequest {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;
        

        public PathRequest(Vector3 _start,Vector3 _end,Action<Vector3[],bool> _action) {
            pathStart = _start;
            pathEnd = _end;
            callback = _action;
        }
    }
}
