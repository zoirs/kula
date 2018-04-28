using System;
using UnityEngine;

public enum GravityDirection {
    DOWN,
    LEFT,
    UP,
    RIGHT
}

static class GravityDirectionExtention {
    public static GravityDirection Rigth(this GravityDirection garavityDirection, Direction moveDirection) {
        switch (garavityDirection) {
            case GravityDirection.DOWN:
                switch (moveDirection) {
                    case Direction.UP:
                        return GravityDirection.RIGHT;
                    case Direction.FORWARD:
                        return GravityDirection.DOWN;
                    case Direction.DOWN:
                        return GravityDirection.LEFT;
                    default:
                        throw new ArgumentOutOfRangeException("moveDirection", moveDirection, null);
                }
            case GravityDirection.LEFT:
                switch (moveDirection) {
                    case Direction.UP:
                        return GravityDirection.DOWN;
                    case Direction.FORWARD:
                        return GravityDirection.LEFT;
                    case Direction.DOWN:
                        return GravityDirection.UP;
                    default:
                        throw new ArgumentOutOfRangeException("moveDirection", moveDirection, null);
                }
            case GravityDirection.UP:
                switch (moveDirection) {
                    case Direction.UP:
                        return GravityDirection.LEFT;
                    case Direction.FORWARD:
                        return GravityDirection.UP;
                    case Direction.DOWN:
                        return GravityDirection.RIGHT;
                    default:
                        throw new ArgumentOutOfRangeException("moveDirection", moveDirection, null);
                }
            case GravityDirection.RIGHT:
                switch (moveDirection) {
                    case Direction.UP:
                        return GravityDirection.UP;
                    case Direction.FORWARD:
                        return GravityDirection.RIGHT;
                    case Direction.DOWN:
                        return GravityDirection.DOWN;
                    default:
                        throw new ArgumentOutOfRangeException("moveDirection", moveDirection, null);
                }
            default:
                throw new ArgumentOutOfRangeException("garavityDirection", garavityDirection, null);
        }
    }

    public static Vector3 GetOppositeVector(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return Vector3.up;
            case GravityDirection.LEFT:
                return Vector3.right;
            case GravityDirection.UP:
                return Vector3.down;
            case GravityDirection.RIGHT:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    public static Vector3 GetVector(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return Vector3.down;
            case GravityDirection.LEFT:
                return Vector3.left;
            case GravityDirection.UP:
                return Vector3.up;
            case GravityDirection.RIGHT:
                return Vector3.right;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }
    
    public static Vector3 GetRigthVector(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return Vector3.right;
            case GravityDirection.LEFT:
                return Vector3.down;
            case GravityDirection.UP:
                return Vector3.left;
            case GravityDirection.RIGHT:
                return Vector3.up;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    public static Vector3 GetUpVector(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return Vector3.up;
            case GravityDirection.LEFT:
                return Vector3.right;
            case GravityDirection.UP:
                return Vector3.down;
            case GravityDirection.RIGHT:
                return Vector3.left;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }
    }

    public static Vector3 GetDownVector(this GravityDirection direction) {
        return GetVector(direction);
    }

    public static float GetRotationAngel(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return 0f;
            case GravityDirection.LEFT:
                return 90f;
            case GravityDirection.UP:
                return 180f;
            case GravityDirection.RIGHT:
                return 270f;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum Direction {
    UP,
    FORWARD,
    DOWN
}