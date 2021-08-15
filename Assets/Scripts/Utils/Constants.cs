using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static string SELECTED_MAP = "Maracao";
    public static int SELECTED_MAP_MAX_ISLAND_SPACES = 29;

    public static string DB_NAME = "pennyPapers.sqlite";
    public static string TORTUGA_MAP = "Tortuga";
    public static string MARACAO_MAP = "Maracao";

    public static int NUM_DICE = 3;
    public static int GRID_ROW = 9;
    public static int GRID_COL = 9;
    public static int DICE_SIDES = 6;
    public static int MAX_TREASURES = 5;
    public static int MAX_SKULLS = 5;
    public static int MAX_ISLAND_SPACES_BOARD_TORTUGA = 30;
    public static int MAX_ISLAND_SPACES_BOARD_MARACAO = 29;
    public static int OVERCOME_SKULL_NUMBER = 9;

    public static int PURPLE_DIE_INDEX = 0;
    public static int GREEN_DIE_INDEX = 1;
    public static int RED_DIE_INDEX = 2;

    public static float ANIMATION_SPEED = 3500f;
    public static float NEW_TURN_DELAY = 0.5f;
    public static float STARTING_GAME_DELAY = 1.5f;
    public static float DICE_ROLLING_COMPENSATION_TIME = 0.4f;
    public static float TIMER = 3f;

    // new values can be filled only next to number or ship
    // as it is in the rules of the game
    public static bool ADJACENCY_RULE_ENABLED = true;
    public static bool UNDO_FUNCTIONALITY_ENABLED = false;

    public static Color32 COLOR_1 = new Color32(241, 145, 32, 255); //f09121
    public static Color32 COLOR_2 = new Color32(130, 135, 145, 255);
    public static Color32 COLOR_3 = new Color32(71, 139, 150, 255);
    public static Color32 COLOR_4 = new Color32(64, 164, 71, 255);
    public static Color32 COLOR_5 = new Color32(33, 156, 216, 255);
    public static Color32 COLOR_6 = new Color32(240, 107, 165, 255);
    public static Color32 COLOR_7 = new Color32(5, 127, 140, 255);
    public static Color32 COLOR_8 = new Color32(126, 75, 152, 255);
    public static Color32 COLOR_9 = new Color32(216, 61, 61, 255);
    public static Color32 COLOR_10 = new Color32(88, 129, 60, 255);
    public static Color32 COLOR_11 = new Color32(196, 8, 98, 255);
    public static Color32 COLOR_12 = new Color32(34, 107, 214, 255);
    public static Color32 COLOR_13 = new Color32(170, 127, 44, 255);
    public static Color32 COLOR_14 = new Color32(95, 71, 150, 255);
    public static Color32 COLOR_15 = new Color32(67, 168, 110, 255);

    public static List<Color32> NUMBER_COLORS = new List<Color32>() {
        COLOR_1, COLOR_2, COLOR_3, COLOR_4, COLOR_5, COLOR_6,
        COLOR_7, COLOR_8, COLOR_9, COLOR_10, COLOR_11, COLOR_12,
        COLOR_13, COLOR_14, COLOR_15
    };

    public static string getTextSuccessLevelScore(int score)
    {
        if (score <= 59)
        {
            return "Tourist";
        }
        else if (score >= 60 && score <= 74)
        {
            return "Pathfinder";
        }
        else if (score >= 75 && score <= 89)
        {
            return "Voyager";
        }
        else
        {
            return "Explorer";
        }
    }

}
