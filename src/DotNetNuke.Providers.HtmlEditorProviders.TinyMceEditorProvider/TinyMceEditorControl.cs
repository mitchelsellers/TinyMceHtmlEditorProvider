using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;

namespace DotNetNuke.Providers.HtmlEditorProviders.TinyMceEditorProvider
{
    public class TinyMceEditorControl : WebControl, IPostBackDataHandler
    {
        /// <summary>
        /// The provider type.
        /// </summary>
        private const string ProviderType = "htmlEditor";

        /// <summary>
        /// The portal settings.
        /// </summary>
        private readonly PortalSettings _portalSettings = (PortalSettings)HttpContext.Current.Items["PortalSettings"];


        public TinyMceEditorControl()
        {

        }

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            try
            {
                string currentValue = Value;
                string postedValue = postCollection[postDataKey];

                if (currentValue == null | !postedValue.Equals(currentValue))
                {
                    if (currentSettings.InjectSyntaxJs)
                    {
                        if (postedValue.Contains("<pre class=\"brush:") && !postedValue.Contains("shCore.js"))
                        {
                            // Add Syntax Highlighter Plugin
                            postedValue =
                                string.Format(
                                    "<!-- Injected Syntax Highlight Code --><script type=\"text/javascript\" src=\"{0}plugins/syntaxhighlight/scripts/shCore.js\"></script><link type=\"text/css\" rel=\"stylesheet\" href=\"{0}plugins/syntaxhighlight/styles/shCore.css\"/><script type=\"text/javascript\">SyntaxHighlighter.all();</script>{1}",
                                    Globals.ResolveUrl("~/Providers/HtmlEditorProviders/DNNConnect.CKE/CKEditor/"),
                                    postedValue);
                        }

                        if (postedValue.Contains("<span class=\"math-tex\">") && !postedValue.Contains("MathJax.js"))
                        {
                            // Add MathJax Plugin
                            postedValue =
                                string.Format(
                                    "<!-- Injected MathJax Code --><script type=\"text/javascript\" src=\"//cdn.mathjax.org/mathjax/2.2-latest/MathJax.js?config=TeX-AMS_HTML\"></script>{0}",
                                    postedValue);
                        }
                    }

                    Value = postedValue;

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            // Do nothing
        }
    }
}