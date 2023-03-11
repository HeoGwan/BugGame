using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CESCO
{
    public enum OBJ_TYPE
    {
        HP_GAUGE_IMAGE,
        HP_GAUGE_BG_IMAGE,
        TOOL_GAUGE_IMAGE,
        TOOL_GAUGE_BG_IMAGE,
        SCORE,
        SHOW_PLAYER_HAS,
        PLAYER_HAS,
    }

    public enum HIT_OBJ_TYPE
    {
        SHOW_HIT,
        CHECK_HIT,
    }

    public enum GAME_STATE
    {
        START,
        RUNNING,
        PAUSE,
        PAY,
    }

    public enum SCREEN
    {
        START = 0,
        INGAME,
        PAUSE,
        SETTING,
        PAY,
        REINFORCE,
        SHOP,
        GAMEOVER,
        GAMERULE,
    }

    public enum TOUCH_SCREEN
    {
        LEFT = 0,
        RIGHT = 1,
    }

    public enum SOUND
    {
        BGM,
        EFFECT,
        UI_EFFECT,
        BUG_EFFECT,
    }

    public enum UI_SOUND
    {
        START,
        PAUSE,
        APPLY,
        CANCEL,
        BUY,
    }

    public enum BUG_TYPE
    {
        MOSQUITO,
        FLY,
        CICADA,
        COCKROACH,
        NONE,
    }

    public enum TOOL
    {
        HAND,
        FLY_SWATTER,
        INSECTICIDE,
        TRAP,
        ELECTRIC_FLY_SWATTER,
        A,
        NONE,
    }

    public enum TOOL_RATE
    {
        SUPER_SLOW,     // 매우 느림
        SLOW,           //   느림
        NORMAL,         //   보통
        FAST,           //   빠름
        SUPER_FAST      // 매우 빠름
    }

    public enum TOOL_RADIUS
    {
        SMALL,          // 작음
        MEDIUM,         // 보통
        LARGE,          //  큼
    }
        
    public enum TOOL_SPEED
    {
        SUPER_SLOW,     // 매우 느림
        SLOW,           //   느림
        NORMAL,         //   보통
        FAST,           //   빠름
        SUPER_FAST      // 매우 빠름
    }

    public enum TOOL_DAMAGE
    {
        VERY_WEAK,
        WEAK,
        NORMAL,
        STRONG,
        VERY_STRONG,
    }
}
