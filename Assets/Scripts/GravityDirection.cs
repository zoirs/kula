using System;

public enum GravityDirection {
    DOWN,
    LEFT,
    UP,
    RIGHT
    

}

    
static class GravityDirectionExtention{
    public static GravityDirection Rigth(this GravityDirection direction) {
        switch (direction) {
            case GravityDirection.DOWN:
                return GravityDirection.LEFT;
            case GravityDirection.LEFT:
                return GravityDirection.UP;
            case GravityDirection.UP:
                return GravityDirection.RIGHT;
            case GravityDirection.RIGHT:
                return GravityDirection.DOWN;
            default:
                throw new ArgumentOutOfRangeException("direction", direction, null);
        }

    }

}
    