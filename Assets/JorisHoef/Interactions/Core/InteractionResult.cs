using UnityEngine;

namespace JorisHoef.Interactions.Core
{
    public readonly struct InteractionResult
    {
#region Constructors and Destructors
        private InteractionResult(bool success,
                                  string message,
                                  Object contextObject,
                                  string originTypeName,
                                  string originStack)
        {
            Success = success;
            Message = message;
            ContextObject = contextObject;
            OriginTypeName = originTypeName;
            OriginStack = originStack;
        }
#endregion

#region Public Properties
        public bool Success { get; }
        public string Message { get; }
        public Object ContextObject { get; }
        public string OriginTypeName { get; }
        public string OriginStack { get; }
#endregion

#region Public Methods
        public static InteractionResult SuccessFrom(Object originContext, string message = null) =>
                new InteractionResult(true, message, originContext, SafeTypeName(originContext),
                                      StackTraceUtility.ExtractStackTrace());

        public static InteractionResult FailureFrom(Object originContext, string message = null) =>
                new InteractionResult(false, message, originContext, SafeTypeName(originContext),
                                      StackTraceUtility.ExtractStackTrace());

        public static InteractionResult FromPlain(object origin, bool success, string message = null) =>
                new InteractionResult(success, message, null, origin?.GetType().Name,
                                      StackTraceUtility.ExtractStackTrace());
#endregion

#region Private Methods
        private static string SafeTypeName(Object o) => o ? o.GetType().Name : null;
#endregion
    }
}