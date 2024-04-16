using System;
using MathNet.Numerics.Distributions;
using System.Collections.Generic;
using System.Linq;

public static class Analyzer
{
    public static double CalculateBlackScholes(double currentPrice, List<double> closingPrices, double strikePrice, double riskFreeRate, double timeToExpiration, bool isPutOption = false)
    {
        double s = currentPrice; // Underlying stock price
        double k = strikePrice; // Strike price
        double r = riskFreeRate; // Risk-free rate
        double t = timeToExpiration; // Time to expiration in years
        double v = StandardDeviation(closingPrices); // Volatility of the underlying stock

        if (v == 0 || t == 0)
            return 0;

        double d1 = (Math.Log(s / k) + (r + 0.5 * Math.Pow(v, 2)) * t) / (v * Math.Sqrt(t));
        double d2 = d1 - v * Math.Sqrt(t);

        if (isPutOption)
        {
            return (k * Math.Exp(-r * t) * Normal.CDF(0, 1, -d2) - s * Normal.CDF(0, 1, -d1));
        }
        else
        {
            return (s * Normal.CDF(0, 1, d1) - k * Math.Exp(-r * t) * Normal.CDF(0, 1, d2));
        }
    }

    public static List<double> CalculatePercentageChange(List<double> closingPrices)
    {
        double actualPctChange = 100 * (closingPrices.Last() - closingPrices.First()) / closingPrices.First();
        return Enumerable.Repeat(actualPctChange, closingPrices.Count).ToList();
    }

    public static List<double> CalculatePredictedPctChange(List<double> closingPrices, int periodLength)
    {
        List<double> dailyPctChange = closingPrices.Select((x, i) => i > 0 ? (closingPrices[i] - closingPrices[i - 1]) / closingPrices[i - 1] : 0).Skip(1).ToList();
        double avgDailyPctChange = dailyPctChange.Average();
        double predictedPctChange = avgDailyPctChange * periodLength * 100;
        return Enumerable.Repeat(predictedPctChange, periodLength).ToList();
    }

    public static double CalculateRiskFreeRate(double closingPrice)
    {
        return closingPrice / 100;
    }

    public static List<double> CalculateSimulatedPrice(List<double> closingPrices, int nsim)
    {
        List<double> dailyReturns = closingPrices.Select((x, i) => i > 0 ? Math.Log(closingPrices[i] / closingPrices[i - 1]) : 0).Skip(1).ToList();
        double mu = dailyReturns.Average();
        double sigma = StandardDeviation(dailyReturns);

        double S0 = closingPrices.Last();
        double T = nsim / 252.0; // Convert the simulation period to years assuming 252 trading days in a year

        List<double> simulatedPrices = new List<double>();
        Normal normalDist = new Normal(0, 1);
        for (int i = 0; i < nsim; i++)
        {
            double brownian = normalDist.Sample() * sigma * Math.Sqrt(T) + mu * T;
            simulatedPrices.Add(S0 * Math.Exp(brownian));
        }
        return simulatedPrices;
    }

    private static double StandardDeviation(List<double> values)
    {
        double avg = values.Average();
        return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
    }
}
