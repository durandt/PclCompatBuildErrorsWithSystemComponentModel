using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PclCompatBuildErrorsWithSystemComponentModel
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "PclCompatBuildErrorsWithSystemComponentModel.LocalizationResources";

        private static readonly Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(
            () => new ResourceManager(ResourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));

        public TranslateExtension()
        {
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = new CultureInfo("en");
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var translation = ResMgr.Value.GetString(Text, ci);

            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException($"Key '{Text}' was not found in resources '{ResourceId}' for culture '{ci.Name}'.", nameof(Text));
#else
                translation = Text; // returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }

}
