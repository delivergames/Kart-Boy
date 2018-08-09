using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension {

    public static Vector3 NoY(this Vector3 v) {
        return new Vector3(v.x, 0f, v.z);
    }

}
