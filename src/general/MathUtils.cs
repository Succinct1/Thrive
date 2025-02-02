﻿using System;
using System.Collections.Generic;
using Godot;

/// <summary>
///   Math related utility functions for Thrive
/// </summary>
public static class MathUtils
{
    public const float EPSILON = 0.00000001f;
    public const float DEGREES_TO_RADIANS = Mathf.Pi / 180;
    public const double FULL_CIRCLE = Math.PI * 2;

    public static T Clamp<T>(this T val, T min, T max)
        where T : IComparable<T>
    {
        if (val.CompareTo(min) < 0)
        {
            return min;
        }

        if (val.CompareTo(max) > 0)
        {
            return max;
        }

        return val;
    }

    public static double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }

    /// <summary>
    ///   Creates a rotation for an organelle. This is used by the editor, but PlacedOrganelle uses RotateY as this
    ///   didn't work there for some reason.
    /// </summary>
    /// <remarks>
    ///   <para>
    ///     Rotation is now the number of 60 degree rotations
    ///   </para>
    /// </remarks>
    public static Quat CreateRotationForOrganelle(float rotation)
    {
        return new Quat(new Vector3(0, -1, 0), rotation * 60 * DEGREES_TO_RADIANS);
    }

    /// <summary>
    ///   This still takes the angle in degrees as this is used from
    ///   places that calculate the angle in degrees.
    /// </summary>
    public static Quat CreateRotationForExternal(float angle)
    {
        return new Quat(new Vector3(0, 1, 0), 180 * DEGREES_TO_RADIANS) *
            new Quat(new Vector3(0, 1, 0), angle * DEGREES_TO_RADIANS);
    }

    /// <summary>
    ///   Rotation for the pilus physics cone
    /// </summary>
    public static Quat CreateRotationForPhysicsOrganelle(float angle)
    {
        return new Quat(new Vector3(-1, 0, 0), 90 * DEGREES_TO_RADIANS) *
            new Quat(new Vector3(0, 0, -1), (180 - angle) * DEGREES_TO_RADIANS);
    }

    /// <summary>
    ///   Returns a Lerped value, and snaps to the target value if current and target
    ///   value is approximately equal by the specified tolerance value.
    /// </summary>
    public static float Lerp(float from, float to, float weight, float tolerance = Mathf.Epsilon)
    {
        if (Mathf.IsEqualApprox(from, to, tolerance))
            return to;

        return Mathf.Lerp(from, to, weight);
    }

    /// <summary>
    ///   Standard modulo for negative values in C# produces negative results.
    ///   This function returns modulo values between 0 and mod-1.
    /// </summary>
    /// <returns>The positive modulo</returns>
    public static int PositiveModulo(this int val, int mod)
    {
        int result = val % mod;
        return (result < 0) ? result + mod : result;
    }

    public static (double Average, double StandardDeviation) CalculateAverageAndStandardDeviation(
        this IEnumerable<int> enumerable)
    {
        int count = 0;
        double sum = 0;
        double sumOfSquares = 0;

        foreach (var value in enumerable)
        {
            ++count;
            sum += value;
            sumOfSquares += value * value;
        }

        if (count == 0)
            throw new InvalidOperationException("Sequence contains no elements");

        double average = sum / count;
        double standardDeviation = Math.Sqrt(sumOfSquares / count - average * average);
        return (average, standardDeviation);
    }

    public static (double Average, double StandardDeviation) CalculateAverageAndStandardDeviation(
        this IEnumerable<double> enumerable)
    {
        int count = 0;
        double sum = 0;
        double sumOfSquares = 0;

        foreach (var value in enumerable)
        {
            ++count;
            sum += value;
            sumOfSquares += value * value;
        }

        if (count == 0)
            throw new InvalidOperationException("Sequence contains no elements");

        double average = sum / count;
        double standardDeviation = Math.Sqrt(sumOfSquares / count - average * average);
        return (average, standardDeviation);
    }
}
