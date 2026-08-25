namespace Assets.Code.Common
{
    public class GameConstants
    {
        public const int TICK_RATE = 60;
        public const float FIXED_DELTA_TIME = 1f / TICK_RATE;
        public const int MAX_CATCHUP_TICKS = 5;

        public const double GAME_SCENE_QUADRANT_SIZE = 50_000;
        public const double GAME_SCENE_HALF_QUADRANT_SIZE = 25_000;

        public const float DISTANCE_UI_TO_REAL = 0.1f;
        public const float DISTANCE_REAL_TO_UI = 10f;

        public const float SKYBOX_OBJECTS_POSITION_MODIFIER = 0.0001f;
    }
}
