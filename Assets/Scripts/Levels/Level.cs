using UnityEngine;

public class Level {
    public Vector3 Spawn { get; private set; }
    public Vector3 Finish { get; private set; }
    public Vector3[] Cubes { get; private set; }

    public Level(Vector3 spawn, Vector3 finish, Vector3[] cubes) {
        Spawn = spawn;
        Finish = finish;
        Cubes = cubes;
    }
}
