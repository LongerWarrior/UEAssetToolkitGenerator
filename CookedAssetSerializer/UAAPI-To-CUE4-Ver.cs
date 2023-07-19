using CUE4Parse.UE4.Versions;

namespace CookedAssetSerializer;

public static class UAAPI_To_CUE4_Ver
{
    public static EGame UToC(this UE4Version a)
    {
        switch (a)
        {
            case UE4Version.VER_UE4_0: return EGame.GAME_UE4_0;
            case UE4Version.VER_UE4_1: return EGame.GAME_UE4_1;
            case UE4Version.VER_UE4_2: return EGame.GAME_UE4_2;
            case UE4Version.VER_UE4_3: return EGame.GAME_UE4_3;
            case UE4Version.VER_UE4_4: return EGame.GAME_UE4_4;
            case UE4Version.VER_UE4_5: return EGame.GAME_UE4_5;
            case UE4Version.VER_UE4_6: return EGame.GAME_UE4_6;
            case UE4Version.VER_UE4_7: return EGame.GAME_UE4_7;
            case UE4Version.VER_UE4_8: return EGame.GAME_UE4_8;
            case UE4Version.VER_UE4_9 or UE4Version.VER_UE4_10: return EGame.GAME_UE4_9;
            case UE4Version.VER_UE4_11: return EGame.GAME_UE4_11;
            case UE4Version.VER_UE4_12: return EGame.GAME_UE4_12;
            case UE4Version.VER_UE4_13: return EGame.GAME_UE4_13;
            case UE4Version.VER_UE4_14: return EGame.GAME_UE4_14;
            case UE4Version.VER_UE4_15: return EGame.GAME_UE4_15;
            case UE4Version.VER_UE4_16 or UE4Version.VER_UE4_17: return EGame.GAME_UE4_16;
            case UE4Version.VER_UE4_18: return EGame.GAME_UE4_18;
            case UE4Version.VER_UE4_19 or UE4Version.VER_UE4_20: return EGame.GAME_UE4_19;
            case UE4Version.VER_UE4_21 or UE4Version.VER_UE4_22: return EGame.GAME_UE4_21;
            case UE4Version.VER_UE4_23 or UE4Version.VER_UE4_24: return EGame.GAME_UE4_23;
            case UE4Version.VER_UE4_25 or UE4Version.VER_UE4_26: return EGame.GAME_UE4_25;
            case UE4Version.VER_UE4_27: return EGame.GAME_UE4_27;
            default: return EGame.GAME_UE4_27;
        }
    }
}