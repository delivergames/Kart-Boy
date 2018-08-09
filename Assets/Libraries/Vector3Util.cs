using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Util {

	public static Vector3 GetUphillFromNormal(Vector3 normal) {
        Vector3 right = Quaternion.AngleAxis(90f, normal) * normal.NoY();
        return Vector3.Cross(normal, right);
    }
}
