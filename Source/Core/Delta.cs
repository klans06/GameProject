using System.Diagnostics;

namespace Overtile.Source.Core;

public static class Delta {
    // private static long _lastTimestamp = Stopwatch.GetTimestamp();
    // private static double _delta = 0;
    // // 5 FPS minimum.
    // private static double _maxDelta = 1.0 / 7.0;
    //
    // public static void CalculateDelta() {
    //     long currentTimestamp = Stopwatch.GetTimestamp();
    //     TimeSpan elapsed = Stopwatch.GetElapsedTime(_lastTimestamp, currentTimestamp);
    //
    //     _delta = elapsed.TotalSeconds;
    //
    //     // Delta limiter.
    //     if (_delta > _maxDelta) {
    //         _delta = _maxDelta;
    //     }
    //
    //     _lastTimestamp = currentTimestamp;
    // }
    //
    // public static float Get() {
    //     return (float)_delta;
    // }
}