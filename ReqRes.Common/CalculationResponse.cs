﻿namespace ReqRes.Common;
public class CalculationResponse
{
    public int Result { get; set; }

    public override string ToString()
    {
        return Result.ToString();
    }
}