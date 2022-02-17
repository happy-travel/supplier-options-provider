using System;
using Microsoft.Extensions.Logging;

namespace HappyTravel.SupplierOptionsProvider.Logger;

public static partial class LoggerExtensions
{
    [LoggerMessage(6001, LogLevel.Debug, "Suppliers storage was refreshed with {Count} suppliers")]
    static partial void SuppliersStorageRefreshed(ILogger logger, int Count);
    
    [LoggerMessage(6002, LogLevel.Error, "Supplier storage update failed with error {Error}")]
    static partial void SupplierStorageUpdateFailed(ILogger logger, System.Exception exception, string Error);
    
    
    
    public static void LogSuppliersStorageRefreshed(this ILogger logger, int Count)
        => SuppliersStorageRefreshed(logger, Count);
    
    public static void LogSupplierStorageUpdateFailed(this ILogger logger, System.Exception exception, string Error)
        => SupplierStorageUpdateFailed(logger, exception, Error);
}