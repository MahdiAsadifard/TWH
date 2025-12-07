namespace Services.ServiceProcessing
{
    /// <summary>
    /// Specifies the processing operation names used by service components.
    /// </summary>
    /// <remarks>Use this enumeration to identify distinct service processing actions, such as health checks
    /// or other controller operations. The values can be used for logging, monitoring, or conditional logic within
    /// service workflows.</remarks>
    public enum ServiceProcessingName
    {
        HealthControllerCheck
    }
}
