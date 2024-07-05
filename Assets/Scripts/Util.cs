using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector2 SetX(Vector2 self, float newVal) { return new Vector2(newVal, self.y); }
    public static Vector2 SetY(Vector2 self, float newVal) { return new Vector2(self.x, newVal); }

    public static Vector3 SetX(Vector3 self, float newVal) { return new Vector3(newVal, self.y, self.z); }
    public static Vector3 SetY(Vector3 self, float newVal) { return new Vector3(self.x, newVal, self.z); }
    public static Vector3 SetZ(Vector3 self, float newVal) { return new Vector3(self.x, self.y, newVal); }

}

public static class Extensions
{
    public static Vector2 SetX(ref this Vector2 self, float newVal) { return self = Util.SetX(self, newVal); }

    public static Vector2 SetY(ref this Vector2 self, float newVal) { return self = Util.SetY(self, newVal); }

    public static Vector3 SetX(ref this Vector3 self, float newVal) { return self = Util.SetX(self, newVal); }

    public static Vector3 SetY(ref this Vector3 self, float newVal) { return self = Util.SetY(self, newVal); }

    public static Vector3 SetZ(ref this Vector3 self, float newVal) { return self = Util.SetZ(self, newVal); }
}
