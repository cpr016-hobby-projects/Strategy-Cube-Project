using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticVariables
{
    private static bool canvasBool = false;
    private static bool menuBool = false;

    public static bool getCanvasBool() {
        return canvasBool;
    }

    public static bool getMenuBool() {
        return menuBool;
    }

    public static void setCanvasBool(bool canvasBool) {
        StaticVariables.canvasBool = canvasBool;
    }

    public static void setMenuBool(bool menuBool) {
        StaticVariables.menuBool = menuBool;
    }

}
