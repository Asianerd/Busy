﻿using System;
using System.Collections;
using System.Collections.Generic;

public class GameValue
{
    public string Name;
    public double I;
    public double Min;
    public double Max;
    public double Regeneration;
    //  |---------I----------|    |---I----------------|    |----------------I----|
    // Min                  Max  Min                  Max  Min                  Max


    public GameValue(double _min, double _max, double _regeneration, double _iPercent = 100.0)
    {
        Min = _min;
        Max = _max;
        Regeneration = _regeneration;
        I = (_max - _min) * (_iPercent / 100);
    }

    public void Regenerate(double _multiplier = 1)
    {
        I += Regeneration * _multiplier;
        I = Clamp(I, Min, Max);
    }

    public void AffectValue(double _amount, bool _limit = true)
    {
        I += _amount;
        if (_limit)
        {
            I = Clamp(I, Min, Max);
        }
    }

    public void AffectValue(float _percent, bool _limit = true)
    {
        I = (Max - Min) * _percent; // Percent = 0f to 1f
    }

    public double Percent()
    {
        return I / (Max - Min);
    }

    public double PercentToValue(float _percent)
    {
        return (Max - Min) * _percent;
    }


    public void AffectMax(double _amount)
    {
        Max += _amount;
    }

    public void AffectMax(float _percent)
    {
        Max *= _percent; // Percent = 0f to 1f
    }


    public void AffectMin(double _amount)
    {
        Min += _amount;
    }

    public void AffectMin(float _percent)
    {
        Min *= _percent; // Percent = 0f to 1f
    }

    double Clamp(double _value, double _min, double _max)
    {
        if (_value < _min)
        {
            return _min;
        }

        if (_value > _max)
        {
            return _max;
        }

        return _value;
    }
}