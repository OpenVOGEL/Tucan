using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DotNumerics_Samples.Harness;
using DotNumerics_Samples.Runner;
using DotNumerics_Samples.Utils;

namespace DotNumerics_Samples
{
    public class SampleLoader
    {
        private List<string> _sourceDirectories = new List<string>();

        public SampleLoader()
        {
        }

        public void AddSourceDirectory(string dir)
        {
            _sourceDirectories.Add(dir);
        }

        public SampleGroup Load(SampleSuite suite)
        {
            Type suiteType = suite.GetType();

            // get attributes attached to suite
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(suiteType, typeof(DescriptionAttribute));
            TitleAttribute titleAttribute = (TitleAttribute)Attribute.GetCustomAttribute(suiteType, typeof(TitleAttribute));
            PrefixAttribute prefixAttribute = (PrefixAttribute)Attribute.GetCustomAttribute(suiteType, typeof(PrefixAttribute));

            if (prefixAttribute == null)
                throw new InvalidOperationException("[Prefix] attribute not specified on " + suiteType.Name);
            string prefix = prefixAttribute.Prefix;

            SampleGroup suiteGroup = new SampleGroup(suite, (titleAttribute != null) ? titleAttribute.Title : "", (descriptionAttribute != null) ? descriptionAttribute.Description : "");
            string sourceFile = FindSourceFile(suiteType.Name + ".cs");
            string sourceCode = (sourceFile != null) ? File.ReadAllText(sourceFile) : "";

            MethodInfo[] categoriesObj = suiteType.GetMethods();
            List<string> categories=new List<string>();
            foreach (MethodInfo c in categoriesObj)
            {
                if (c.Name.StartsWith(prefix) == true)
                {
                    CategoryAttribute[] attributes=(CategoryAttribute[])c.GetCustomAttributes(typeof(CategoryAttribute), false);
                    if (attributes.Length > 0)
                    {
                        if (categories.Contains(attributes[0].Category) == false) categories.Add(attributes[0].Category);                        
                    }
                }

            }

            foreach (string cat in categories)
            {
                suiteGroup.Children.Add(LoadCategory(suite, cat, prefix, sourceCode));
            }

            return suiteGroup;
        }

        private SampleGroup LoadCategory(SampleSuite suite, string category, string prefix, string fileSourceCode)
        {
            SampleGroup categoryGroup = new SampleGroup(suite, category, null);
            Type suiteType = suite.GetType();

            MethodInfo[] methodsInCategoryArray =suiteType.GetMethods();
            List<MethodInfo> methodsInCategory = new List<MethodInfo>();

            foreach (MethodInfo c in methodsInCategoryArray)
            {
                if (c.Name.StartsWith(prefix) == true)
                {
                    CategoryAttribute[] attributes = (CategoryAttribute[])c.GetCustomAttributes(typeof(CategoryAttribute), false);
                    if (attributes.Length > 0 && attributes[0].Category == category) methodsInCategory.Add(c);
                }
            }

            foreach (MethodInfo mi in methodsInCategory)
            {
                categoryGroup.Children.Add(LoadSample(suite, mi, fileSourceCode));
            }
            return categoryGroup;
        }

        private Sample LoadSample(SampleSuite suite, MethodInfo method, string fileSourceCode)
        {
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute));
            TitleAttribute titleAttribute = (TitleAttribute)Attribute.GetCustomAttribute(method, typeof(TitleAttribute));

            return new Sample(suite, method, (titleAttribute != null) ? titleAttribute.Title : "", (descriptionAttribute != null) ? descriptionAttribute.Description : "", CodeExtractor.GetCodeBlock(fileSourceCode, "void " + method.Name));
        }

        private string FindSourceFile(string fileName)
        {
            foreach (string dir in _sourceDirectories)
            {
                string fullName = Path.Combine(dir, fileName);
                if (File.Exists(fullName))
                    return fullName;
            }

            return null;
        }
    }
}