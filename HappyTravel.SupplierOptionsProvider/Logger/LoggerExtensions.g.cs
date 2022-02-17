using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.SupplierOptionsProvider.Logger
{
    public static class LoggerExtensions
    {
        static LoggerExtensions()
        {
            SuppliersStorageRefreshed = LoggerMessage.Define<int>(LogLevel.Debug,
                new EventId(6001, "SuppliersStorageRefreshed"),
                "Suppliers storage was refreshed with {Count} suppliers");
            
            SupplierStorageUpdateFailed = LoggerMessage.Define<string>(LogLevel.Error,
                new EventId(6002, "SupplierStorageUpdateFailed"),
                "Supplier storage update failed with error {Error}");
            
        }
    
                
         public static void LogSuppliersStorageRefreshed(this ILogger logger, int Count, Exception exception = null)
            => SuppliersStorageRefreshed(logger, Count, exception);
                
         public static void LogSupplierStorageUpdateFailed(this ILogger logger, string Error, Exception exception = null)
            => SupplierStorageUpdateFailed(logger, Error, exception);
    
    
        
        private static readonly Action<ILogger, int, Exception> SuppliersStorageRefreshed;
        
        private static readonly Action<ILogger, string, Exception> SupplierStorageUpdateFailed;
    }
}