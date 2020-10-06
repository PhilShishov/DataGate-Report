﻿namespace ReportProcessor.DataProcessor
{
    using System;
    using ReportProcessor.Common;
    using ReportProcessor.Dtos;

    public static class Controller
    {
        public static bool SecurityCheck(ShareClassDto shareClass, string isin, string currency)
        {
            bool hasPassedCheck = true;

            // First security check: ISIN and currency in report are existing and the same as in internal DB
            if (shareClass == null)
            {
                Console.WriteLine(string.Format(ErrorMessages.InvalidIsin, isin, currency));
                hasPassedCheck = false;
            }

            return hasPassedCheck;
        }
    }
}
