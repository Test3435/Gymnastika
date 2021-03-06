﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.IO;
using System.Windows;
using Gymnastika.Modules.Sports.Facilities;

namespace Gymnastika.Modules.Sports.Converters
{
    //[ValueConversion(typeof(string), typeof(FlowDocument))]
    public class RtfUriToDocumentConverter : IValueConverter
    {
        public object Convert(object path, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var doc = new FlowDocument();
            string docPath = (path as string);
            if (!string.IsNullOrEmpty(docPath))
            {
                docPath = PathFacility.GetDataAbsolutePath(docPath);
                try
                {
                    using (Stream stream = File.Open(docPath, FileMode.Open))
                    {
                        TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                        range.Load(stream, DataFormats.Rtf);
                    }
                }
                catch (Exception)
                {
                }
            }
            return doc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
