using System;
using UnityEngine;

public enum Direction {
    DOWN,
    LEFT,
    UP,
    RIGHT
}

static class GravityDirectionExtention {
   
    public static Vector3 GetOppositeVector(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Vector3.up;
            case Direction.LEFT:
                return Vector3.right;
            case Direction.UP:
                return Vector3.down;
            case Direction.RIGHT:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    public static Vector3 GetVector(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Vector3.down;
            case Direction.LEFT:
                return Vector3.left;
            case Direction.UP:
                return Vector3.up;
            case Direction.RIGHT:
                return Vector3.right;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Vector3 GetRigthVector(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Vector3.right;
            case Direction.LEFT:
                return Vector3.down;
            case Direction.UP:
                return Vector3.left;
            case Direction.RIGHT:
                return Vector3.up;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Vector3 GetLeftVector(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Vector3.left;
            case Direction.LEFT:
                return Vector3.up;
            case Direction.UP:
                return Vector3.right;
            case Direction.RIGHT:
                return Vector3.down;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Direction  GetNextClockwise(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Direction.LEFT;
            case Direction.LEFT:
                return Direction.UP;
            case Direction.UP:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.DOWN;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Direction GetCounterClockwise(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.UP;
            case Direction.UP:
                return Direction.LEFT;
            case Direction.LEFT:
                return Direction.DOWN;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Vector3 GetDirection(this Direction direction, Direction moveDir) {
        switch (moveDir) {
            case Direction.LEFT:
                return GetLeftVector(direction);
            case Direction.RIGHT:
                return GetRigthVector(direction);
            default:
                throw new ArgumentOutOfRangeException("moveDir", moveDir, null);
        }
    }

    public static Vector3 GetUpVector(this Direction direction) {
        switch (direction) {
            case Direction.DOWN:
                return Vector3.up;
            case Direction.LEFT:
                return Vector3.right;
            case Direction.UP:
                return Vector3.down;
            case Direction.RIGHT:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    public static Vector3 GetDownVector(this Direction direction) {
        return GetVector(direction);
    }
}
