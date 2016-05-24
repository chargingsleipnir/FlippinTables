using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {
    public const float DROP_OFF_LIMIT = -20.0f;
    public const float DROP_TIME_MIN = 0.75f;
    public const float EARLY_EXIT_TIME = 1.0f;
    public const float MAX_ADD_FORCE_X = -200.0f;
    public const float RANGE_FORCE_Y = 200.0f;
    public const float RANGE_TORQUE = 50.0f;
    public const float TABLE_LAUNCH_X = -20.0f;
    public const int AD_RESET_COUNTER = 4;

    public const string BOARD_ID_SCORE = "CgkIgpqt_u0CEAIQAQ";
    public const string BOARD_ID_COMBO = "CgkIgpqt_u0CEAIQAg";
    public const string BOARD_ID_FLIPS = "CgkIgpqt_u0CEAIQAw";

    public const string ACH_FOCUSED_FLIPPER = "CgkIgpqt_u0CEAIQBA";
    public const string ACH_GOOD_GRIP = "CgkIgpqt_u0CEAIQBQ";
    public const string ACH_MESSY_MAYHEM = "CgkIgpqt_u0CEAIQBg";
    public const string ACH_SOOTHING_SONG = "CgkIgpqt_u0CEAIQBw";
    public const string ACH_TABLE_TIRADE = "CgkIgpqt_u0CEAIQCA";

    public const int ACH_FLIP_ACC_COUNT = 10;
    public const int ACH_TABLE_TIRADE_SCORE = 1000;
    public const int ACH_TABLE_TIRADE_FLIPS = 200;
    public const int ACH_SOOTHING_SONG_MINS = 5;
}
