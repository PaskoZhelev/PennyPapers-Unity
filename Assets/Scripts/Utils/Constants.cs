﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public static int NUM_DICE = 3;
    public static int DICE_SIDES = 5;
    public static int MAX_TREASURES = 5;
    public static int MAX_ISLAND_SPACES_BOARD_TORTUGA = 30;

    public static float ANIMATION_SPEED = 5500f;
    public static float NEW_TURN_DELAY = 1.0f;
    public static float DICE_ROLLING_COMPENSATION_TIME = 0.4f;
    public static float TIMER = 3f;

    public static Color32 GREEN1 = new Color32(64, 164, 71, 255);
    public static Color32 GREEN2 = new Color32(97, 117, 73, 255);
    public static Color32 PINK = new Color32(240, 107, 165, 255);
    public static Color32 PINK2 = new Color32(196, 8, 98, 255);
    public static Color32 BLUE1 = new Color32(33, 156, 216, 255);
    public static Color32 BLUE2 = new Color32(34, 107, 214, 255);
    public static Color32 GRAY = new Color32(130, 135, 145, 255);
    public static Color32 ORANGE1 = new Color32(241, 145, 32, 255);
    public static Color32 ORANGE2 = new Color32(175, 95, 14, 255);
    public static Color32 PURPLE1 = new Color32(126, 75, 152, 255);
    public static Color32 PURPLE2 = new Color32(95, 71, 150, 255);
    public static Color32 MARINE1 = new Color32(71, 139, 150, 255);
    public static Color32 MARINE2 = new Color32(8, 104, 119, 255);
    public static Color32 RED1 = new Color32(216, 61, 61, 255);
    public static Color32 RED2 = new Color32(170, 68, 68, 255);

    public static List<Color32> NUMBER_COLORS = new List<Color32>() { 
        ORANGE1, GRAY, MARINE1, GREEN1, BLUE1,
        PINK, MARINE2, PURPLE1, RED1, GREEN2,
        PINK2, BLUE2, ORANGE2, PURPLE2, RED2
    };

}