namespace Core.Exceptions
{
    public enum ApiExceptionCode
    {
        /// <summary>
        /// 500
        /// </summary>
        UnknownError,
        /// <summary>
        /// 400
        /// </summary>
        BadRequest,
        /// <summary>
        /// 401
        /// </summary>
        Unauthorized,
        /// <summary>
        /// 403
        /// </summary>
        Forbidden,
        /// <summary>
        /// 404
        /// </summary>
        NotFound,
        /// <summary>
        /// 408
        /// </summary>
        RequestTimeout,
        /// <summary>
        /// 409
        /// </summary>
        Conflict,
        /// <summary>
        /// 412
        /// </summary>
        PreconditionFailed,
        /// <summary>
        /// 500
        /// </summary>
        InternalServerError,
        /// <summary>
        /// 507
        /// </summary>
        StorageError,
    }
}
