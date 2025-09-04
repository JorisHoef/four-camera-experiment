using UnityEngine;

namespace JorisHoef.Interactions.UX
{
    /// <summary>
    ///     Static locator with a sensible default: auto-loads a TextTable named "Interactions_Texts" from Resources.
    ///     You can also set a custom provider at runtime via SetProvider.
    /// </summary>
    public static class Texts
    {
#region Constants and Fields
        private const string DEFAULT_RESOURCE_NAME = "Interactions_Texts";
        private static ITextProvider _provider;
#endregion

#region Private Properties
        private static ITextProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    TextTable table = Resources.Load<TextTable>(DEFAULT_RESOURCE_NAME);
                    if (table != null)
                    {
                        _provider = table;
                    }
                }
                return _provider;
            }
        }
#endregion

#region Public Methods
        public static void SetProvider(ITextProvider provider) => _provider = provider;

        public static string Get(string key, string fallback = "")
        {
            if (Provider != null && Provider.TryGet(key, out string v))
            {
                return v;
            }
            return fallback;
        }
#endregion
    }
}