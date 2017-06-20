using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Collections;
using System.Text;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

    public static class HtmlExtentions
    {
        public static MvcHtmlString Print<TModel>(this HtmlHelper<TModel> htmlHelper, string text)
        {
            return MvcHtmlString.Create(text);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, 1);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MultiSelectList selectList)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, 1);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MultiSelectList selectList, int numberOfColumns)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, numberOfColumns);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, int numberOfColumns)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, numberOfColumns);
        }

        //public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<INamedListItems> selectList, IDictionary<string, object> htmlAttributes = null)
        //{
        //    var sb = new StringBuilder();
        //    if (selectList != null)
        //    {
        //        string name = ExpressionHelper.GetExpressionText(expression);
        //        name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

        //        sb.Append("<table border=\"0\"><tr>");
        //        foreach (INamedListItems column in selectList)
        //        {
        //            sb.Append("<td valign=\"top\"><b>");
        //            sb.Append(column.Name);
        //            sb.Append("</b><br/>");
        //            foreach (var option in column.ListItems)
        //            {
        //                var builder = new TagBuilder("input");
        //                if (option.Selected)
        //                    builder.MergeAttribute("checked", "checked");

        //                builder.MergeAttributes<string, object>(htmlAttributes);
        //                builder.MergeAttribute("type", "checkbox");
        //                builder.MergeAttribute("value", option.Value);
        //                builder.MergeAttribute("name", name);

        //                var contenBuilder = new TagBuilder("span");
        //                contenBuilder.MergeAttributes<string, object>(htmlAttributes);
        //                contenBuilder.SetInnerText(option.Text);

        //                builder.InnerHtml = contenBuilder.ToString(TagRenderMode.Normal);
        //                sb.Append(builder.ToString(TagRenderMode.Normal));
        //                sb.Append("<br />");
        //            }
        //            sb.Append("</td>");
        //        }
        //        sb.Append("</tr></table>");
        //    }
        //    return MvcHtmlString.Create(sb.ToString());
        //}
        
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MultiSelectList selectList,
            object htmlAttributes, int numberOfColumns)
        {
            return CheckBoxListFor<TModel, TProperty>(htmlHelper, expression, selectList, ((IDictionary<string, object>)new RouteValueDictionary(htmlAttributes)), numberOfColumns);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, MultiSelectList selectList,
            IDictionary<string, object> htmlAttributes, int numberOfColumns)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            // Get the property (and assume IEnumerable)
            IEnumerable currentValues = htmlHelper.ViewData.Model != null
                                            ? (IEnumerable)expression.Compile().Invoke(htmlHelper.ViewData.Model)
                                            : null;

            int columnCount = 0;
            var sb = new StringBuilder();
            foreach (var option in selectList)
            {
                columnCount++;
                var builder = new TagBuilder("input");
                if (ShouldItemBeSelected(option, currentValues))
                    builder.MergeAttribute("checked", "checked");

                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", option.Value);
                builder.MergeAttribute("name", name);

                var contenBuilder = new TagBuilder("span");
                contenBuilder.MergeAttributes<string, object>(htmlAttributes);
                contenBuilder.SetInnerText(option.Text);

                builder.InnerHtml = contenBuilder.ToString(TagRenderMode.Normal);
                sb.Append(builder.ToString(TagRenderMode.Normal));
                if (columnCount == numberOfColumns)
                {
                    columnCount = 0;
                    sb.Append("<br />");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, htmlAttributes, 1);
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes, int numberOfColumns)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            int columnCount = 0;
            var sb = new StringBuilder();
            foreach (var option in selectList)
            {
                columnCount++;
                var builder = new TagBuilder("input");
                if (option.Selected)
                    builder.MergeAttribute("checked", "checked");

                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "checkbox");
                builder.MergeAttribute("value", option.Value);
                builder.MergeAttribute("name", name);

                var contenBuilder = new TagBuilder("span");
                contenBuilder.MergeAttributes<string, object>(htmlAttributes);
                contenBuilder.SetInnerText(option.Text);

                builder.InnerHtml = contenBuilder.ToString(TagRenderMode.Normal);
                sb.Append(builder.ToString(TagRenderMode.Normal));
                if (columnCount == numberOfColumns)
                {
                    columnCount = 0;
                    sb.Append("<br />");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }

        private static bool ShouldItemBeSelected(SelectListItem item, IEnumerable selectedValues)
        {
            bool selected = false;
            if (null != selectedValues)
            {
                var enumerator = selectedValues.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var currentValueAsString = (string)Convert.ChangeType(enumerator.Current, typeof(string));
                    selected = string.Equals(currentValueAsString, item.Value);
                    if (selected)
                        break;
                }
            }
            return selected;
        }

        #region RadioButtonList
        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return RadioButtonListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, 1);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, int numberOfColumns)
        {
            return RadioButtonListFor<TModel, TProperty>(htmlHelper, expression, selectList, null, numberOfColumns);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes, int numberOfColumns)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

            // Get the property (and assume IEnumerable)
            //TProperty currentValue = htmlHelper.ViewData.Model != null
            //                                ? (TProperty)expression.Compile().Invoke(htmlHelper.ViewData.Model)
            //                                : default(TProperty);

            int columnCount = 0;
            var sb = new StringBuilder();
            foreach (var option in selectList)
            {
                columnCount++;
                var builder = new TagBuilder("input");
                if (option.Selected)
                    builder.MergeAttribute("checked", "checked");

                builder.MergeAttributes<string, object>(htmlAttributes);
                builder.MergeAttribute("type", "radio");
                builder.MergeAttribute("value", option.Value);
                builder.MergeAttribute("name", name);
                builder.InnerHtml = option.Text;
                sb.Append(builder.ToString(TagRenderMode.Normal));
                if (columnCount == numberOfColumns)
                {
                    columnCount = 0;
                    sb.Append("<br />");
                }
                else
                {
                    sb.Append("&nbsp;");
                }
            }
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion

        //public static MvcHtmlString MessagesFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, IEnumerable<TProperty>>> expression) where TProperty : ModelMessage
        //{
        //    string name = ExpressionHelper.GetExpressionText(expression);
        //    name = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);

        //     //Get the property (and assume IEnumerable)
        //    IEnumerable<TProperty> currentValue = htmlHelper.ViewData.Model != null
        //                                    ? (IEnumerable<TProperty>)expression.Compile().Invoke(htmlHelper.ViewData.Model)
        //                                    : default(IEnumerable<TProperty>);

        //    var sb = new StringBuilder();
        //    if (currentValue != null)
        //    {
        //        sb.Append("<a name=\"hkjhkMesStart\" id=\"hkjhkMesStart\" href=\"#mdlMessages\"></a><table style=\"background-color:White;\">");
        //        foreach (var message in currentValue)
        //        {
        //            string styleString = null, iconSrc;
        //            sb.Append("<tr><td valign=\"top\" width=\"14\"><img height=\"14\" src=\""); //First cell
        //            switch (message.MessageType)
        //            {
        //                case FedExIScanWeb.Code.Enumerations.ModelMessageTypeOptions.ERROR:
        //                    styleString = "color: red;";
        //                    iconSrc = ResolveUrl("~/images/icons/error.png");
        //                    break;
        //                case FedExIScanWeb.Code.Enumerations.ModelMessageTypeOptions.SUCCESS:
        //                    styleString = "color: green;";
        //                    iconSrc = ResolveUrl("~/images/icons/sucess.png");
        //                    break;
        //                case FedExIScanWeb.Code.Enumerations.ModelMessageTypeOptions.INFO:
        //                case FedExIScanWeb.Code.Enumerations.ModelMessageTypeOptions.YES_NO:
        //                    iconSrc = ResolveUrl("~/images/icons/warning.png");
        //                    break;
        //                default:
        //                    iconSrc = ResolveUrl("~/images/icons/bullet.png");
        //                    break;
        //            }
        //            sb.Append(iconSrc);
        //            sb.AppendFormat("\"/></td><td style=\"{0}\">", styleString); //Second cell
        //            sb.Append(message.Text);
        //            if (message.MessageType == FedExIScanWeb.Code.Enumerations.ModelMessageTypeOptions.YES_NO)
        //            {
        //                string mesId = message.GetUniqueId();
        //                sb.Append("<input type=\"hidden\" name=\"");
        //                sb.Append(mesId);
        //                sb.Append("\" id=\"");
        //                sb.Append(mesId);
        //                sb.Append("\" />");
        //                sb.Append("&nbsp;&nbsp;&nbsp;<input type=\"submit\" value=\"Yes\" class=\"confirm_mess_btn confirm_mess_btn_yes\" onclick=\"document.getElementById('");
        //                sb.Append(mesId);
        //                sb.Append("').value='yes';\" />");
        //                sb.Append("&nbsp;<input type=\"submit\" value=\"No\" class=\"confirm_mess_btn confirm_mess_btn_no\" onclick=\"document.getElementById('");
        //                sb.Append(mesId);
        //                sb.Append("').value='no';\" />");
        //            }
        //            sb.Append("</td></tr>");
        //        }
        //        sb.AppendLine("</table><script>try{$(\"#link\").focus();}catch(e){}</script>");
        //    }
        //    return MvcHtmlString.Create(sb.ToString());
        //}

        public static string ResolveUrl(string relativeUrl)
        {
            if (HttpContext.Current != null)
            {
                string root = string.Format("{0}://{1}{2}",
                    HttpContext.Current.Request.Url.Scheme,
                    HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                    (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? string.Empty : HttpContext.Current.Request.ApplicationPath
                    );

                return relativeUrl.Replace("~", root);
            }
            else
                return relativeUrl;
        }

        public static MvcHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, object routeValues, System.Web.Mvc.Ajax.AjaxOptions ajaxOptions, object htmlAtributes)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions, htmlAtributes);
            return MvcHtmlString.Create(
                link.ToHtmlString().Replace("[replaceme]",
                   string.Concat("<imgLabel>", altText, "</imgLabel>", builder.ToString(TagRenderMode.SelfClosing))
                )
            );
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imageUrl, string altText, string actionName, string controllerName, object routeValues, object htmlAtributes)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            var link = helper.ActionLink("[replaceme]", actionName, controllerName, routeValues, htmlAtributes);
            return MvcHtmlString.Create(
                link.ToHtmlString().Replace("[replaceme]",
                   string.Concat("<imgLabel>", altText, "</imgLabel>", builder.ToString(TagRenderMode.SelfClosing))
                )
            );
        }

        #region Password

        public static MvcHtmlString PasswordBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return PasswordBoxFor<TModel, TProperty>(htmlHelper, expression, ((IDictionary<string, object>)null));
        }

        public static MvcHtmlString PasswordBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            return PasswordBoxHelper(htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText((LambdaExpression)expression), null, htmlAttributes);
        }

        public static MvcHtmlString PasswordBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return PasswordBoxFor<TModel, TProperty>(htmlHelper, expression, ((IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));
        }


        private static MvcHtmlString PasswordBoxHelper(this HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            return InputBoxHelper(htmlHelper, InputType.Password, metadata, name, value, false, false, true, true, null, htmlAttributes);
        }

        static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState))
            {
                if (modelState.Value != null)
                {
                    return modelState.Value.ConvertTo(destinationType, null /* culture */);
                }
            }
            return null;
        }

        private static MvcHtmlString InputBoxHelper(this HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, string format, IDictionary<string, object> htmlAttributes)
        {
            ModelState state;
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("name");
            }
            TagBuilder tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder.MergeAttribute("name", fullHtmlFieldName, true);
            string b = htmlHelper.FormatValue(value, format);
            bool flag = false;
            switch (inputType)
            {
                case InputType.CheckBox:
                    {
                        tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
                        bool? modelStateValue = GetModelStateValue(htmlHelper, fullHtmlFieldName, typeof(bool)) as bool?;
                        if (modelStateValue.HasValue)
                        {
                            isChecked = modelStateValue.Value;
                            flag = true;
                        }
                        break;
                    }
                case InputType.Password:
                    if (HttpContext.Current.Request.Browser.Type.StartsWith("Chrome"))
                    {
                        tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Text));
                        if (value != null)
                        {
                            tagBuilder.MergeAttribute("value", b, isExplicitValue);
                        }
                        //tagBuilder.MergeAttribute("readonly", "readonly");
                        //tagBuilder.MergeAttribute("onfocus", "this.removeAttribute('readonly');");
                        tagBuilder.MergeAttribute("autocomplete", "off");
                        tagBuilder.MergeAttribute("autocorrect", "off");
                        tagBuilder.MergeAttribute("autocapitalize", "off");
                        var styleAttr = tagBuilder.Attributes.FirstOrDefault(a => a.Key == "style");
                        if (string.IsNullOrWhiteSpace(styleAttr.Value))
                            tagBuilder.MergeAttribute("style", "-webkit-text-security: disc !important; -moz-text-security: disc !important; text-security: disc !important; autocomplete:off !important; autocorrect:off !important; autocapitalize:off !important;");
                        else
                        {
                            tagBuilder.Attributes.Remove(styleAttr);
                            tagBuilder.MergeAttribute("style", string.Join("; ", "-webkit-text-security: disc !important; -moz-text-security: disc !important; text-security: disc !important; autocomplete:off !important; autocorrect:off !important; autocapitalize:off !important;", styleAttr.Value));
                        }
                    }
                    else
                    {
                        //throw new FedExIScanWeb.Code.Exceptions.BrowserNotSupportedException(FedExIScanWeb.ErrorMessages.FireFoxIsEval, null);
                        tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
                        if (value != null)
                        {
                            tagBuilder.MergeAttribute("value", b, isExplicitValue);
                        }
                    }
                    goto Label_016C;
                case InputType.Radio:
                    tagBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
                    break;

                default:
                    {
                        string str4 = (string)GetModelStateValue(htmlHelper, fullHtmlFieldName, typeof(string));
                        tagBuilder.MergeAttribute("value", str4 ?? (useViewData ? EvalString(htmlHelper, fullHtmlFieldName, format) : b), isExplicitValue);
                        goto Label_016C;
                    }
            }
            if (!flag)
            {
                string a = GetModelStateValue(htmlHelper, fullHtmlFieldName, typeof(string)) as string;
                if (a != null)
                {
                    isChecked = string.Equals(a, b, StringComparison.Ordinal);
                    flag = true;
                }
            }
            if (!flag && useViewData)
            {
                isChecked = htmlHelper.EvalBoolean(fullHtmlFieldName);
            }
            if (isChecked)
            {
                tagBuilder.MergeAttribute("checked", "checked");
            }
            tagBuilder.MergeAttribute("value", b, isExplicitValue);
        Label_016C:
            if (setId)
            {
                tagBuilder.GenerateId(fullHtmlFieldName);
            }
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state) && (state.Errors.Count > 0))
            {
                tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }
            tagBuilder.MergeAttributes<string, object>(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));
            if (inputType == InputType.CheckBox)
            {
                StringBuilder builder2 = new StringBuilder();
                builder2.Append(tagBuilder.ToString(TagRenderMode.SelfClosing));
                TagBuilder builder3 = new TagBuilder("input");
                builder3.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
                builder3.MergeAttribute("name", fullHtmlFieldName);
                builder3.MergeAttribute("value", "false");
                builder2.Append(builder3.ToString(TagRenderMode.SelfClosing));
                return MvcHtmlString.Create(builder2.ToString());
            }
            return MvcHtmlString.Create( tagBuilder.ToString( TagRenderMode.SelfClosing ));
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), System.Globalization.CultureInfo.CurrentCulture);
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key, string format)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key, format), System.Globalization.CultureInfo.CurrentCulture);
        }

        internal static bool EvalBoolean(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToBoolean(htmlHelper.ViewData.Eval(key), System.Globalization.CultureInfo.InvariantCulture);
        }

        #endregion
    }
