/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.Utilities.Formatting;
using Plexdata.Utilities.Formatting.Interfaces;
using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Plexdata.Utilities.Templates.Tester
{
    class Program
    {
        static void Main(String[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture("en-US");

            Console.OutputEncoding = Encoding.UTF8;

            Program.CommonExamples_SimpleFormatting();

            Console.WriteLine("------------------------------");

            Program.CommonExamples_MixedFormatting();

            Console.WriteLine("------------------------------");

            Program.CommonExamples_ExtendedFormatting();

            Console.WriteLine("------------------------------");

            Program.CommonExamples_AlignmentFormatting();

            Console.WriteLine("------------------------------");

            Program.CommonExamples_CultureFormatting();

            Console.WriteLine("------------------------------");

            Program.SpecificExamples_TruncationAndFallback();

            Console.WriteLine("------------------------------");

            Program.SpecificExamples_SerializationFormatting();

            Console.WriteLine("------------------------------");

            Program.SpecificExamples_RelationsFormatting();

            Console.WriteLine("------------------------------");

            Program.SpecificExamples_ExceptionFormatting();

            Console.ReadKey();
        }

        private static void CommonExamples_SimpleFormatting()
        {
            String result;

            // Index-based with system types.
            result = Template.Format("text {1} text {0} text {2}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text ABC text 111 text 333.33"

            // Index-based with system types and one missing argument.
            result = Template.Format("text {1} text {0} text {2}", 111, "ABC");
            Console.WriteLine(result); // => "text ABC text 111 text {2}"

            // Index-based with system types and inline argument array.
            result = Template.Format("text {1} text {0} text {2} text {3} text {2}", "ABC", new[] { 111, 222, 333 });
            Console.WriteLine(result); // => "text 111 text ABC text 222 text 333 text 222"

            // Index-based with system types with stringification.
            result = Template.Format("text {$1} text {$0} text {$2}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text ABC text 111 text 333.33" => For sure, stringification for system types does not really make sense.

            // Index-based with system types with spreadification (serialization).
            result = Template.Format("text {@1} text {@0} text {@2}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text ABC text 111 text 333.33" => For sure, spreadification for system types does not really make sense.

            // Label-based with system types.
            result = Template.Format("text {ValA} text {ValB} text {ValC}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text 111 text ABC text 333.33"

            // Label-based with system types and one missing argument.
            result = Template.Format("text {ValA} text {ValB} text {ValC}", 111, "ABC");
            Console.WriteLine(result); // => "text 111 text ABC text {ValC}"

            // Label-based with system types and inline argument array.
            result = Template.Format("text {ValA} text {ValB} text {ValC} text {ValD}", "ABC", new[] { 111, 222, 333 });
            Console.WriteLine(result); // => "text ABC text 111 text 222 text 333"

            // Label-based with system types with stringification.
            result = Template.Format("text {$ValA} text {$ValB} text {$ValC}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text 111 text ABC text 333.33" => For sure, stringification for system types does not really make sense.

            // Label-based with system types with spreadification (serialization).
            result = Template.Format("text {@ValA} text {@ValB} text {@Valc}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text 111 text ABC text 333.33" => For sure, spreadification for system types does not really make sense.

            // The usage of two curly braces causes the formatter to take the content as it is.

            // Index-based with system types but with escaped data.
            result = Template.Format("text {1} text {{escaped}} text {2}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text ABC text {escaped} text 333.33"

            // Label-based with system types but with escaped data.
            result = Template.Format("text {ValA} text {{escaped}} text {ValC}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text 111 text {escaped} text ABC"
        }

        private static void CommonExamples_MixedFormatting()
        {
            String result;

            // Mixed index and label based formatting
            result = Template.Format("text {1} text {ValA} text {2}", 111, "ABC", 333.33);
            Console.WriteLine(result); // => "text 111 text ABC text 333.33"

            // The order of argument processing is always left-to-right, as soon as at least 
            // one of the formatting instruction is of type label. This fits the rules of template formatting.
        }

        private static void CommonExamples_ExtendedFormatting()
        {
            String result;

            // Index-based with system types and format instructions.
            result = Template.Format("text {1:N2} text {0:C} text {2}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 2,222.22 text $1,111,111.11 text 333.33" => This should be expected because of its the same result as for String.Format().

            // Index-based with system types with stringification and format instructions.
            result = Template.Format("text {$1:N2} text {$0:C} text {$2}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 2222.2222 text 1111111.1111 text 333.33" => The reason is that ToString() is called instead of performing argument formatting.

            // Index-based with system types with spreadification (serialization) and format instructions.
            result = Template.Format("text {@1:N2} text {@0:C} text {@2}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 2,222.22 text $1,111,111.11 text 333.33" => Ends up with same result as for standard formatting.

            // Label-based with system types and format instructions.
            result = Template.Format("text {ValA:N2} text {ValB:C} text {ValC}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 1,111,111.11 text $2,222.22 text 333.33" => This should be expected because of its the same result as for String.Format().

            // Label-based with system types with stringification and format instructions.
            result = Template.Format("text {$ValA:N2} text {$ValB:C} text {$ValC}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 1111111.1111 text 2222.2222 text 333.33" => The reason is that ToString() is called instead of performing argument formatting.

            // Label-based with system types with spreadification (serialization) and format instructions.
            result = Template.Format("text {@ValA:N2} text {@ValB:C} text {@ValC}", 1111111.1111, 2222.2222, 333.33);
            Console.WriteLine(result); // => "text 1,111,111.11 text $2,222.22 text 333.33" => Ends up with same result as for standard formatting.
        }

        private static void CommonExamples_AlignmentFormatting()
        {
            String result;

            // The alignment is separated by comma and is followed by the total character width. 
            // A negative value means "left justified" and a positive value means "right justified". 
            // But not a plus (+) for a positive value is not allowed. 

            // Index-based with system types and value alignment.
            result = Template.Format("left [{1,-20}] right [{0,20}]", 111111, 222222);
            Console.WriteLine(result); // => "left [222222              ] right [              111111]" 

            // Label-based with system types and value alignment.
            result = Template.Format("left [{ValA,-20}] right [{ValB,20}]", 111111, 222222);
            Console.WriteLine(result); // => "left [111111              ] right [              222222]" 

            // Index-based with system types and value alignment and format instructions.
            result = Template.Format("left [{1,-20:N2}] right [{0,20:C3}]", 111111, 222222);
            Console.WriteLine(result); // => "left [222,222.00          ] right [        $111,111.000]" 

            // Label-based with system types and value alignment.
            result = Template.Format("left [{ValA,-20:N2}] right [{ValB,20:C3}]", 111111, 222222);
            Console.WriteLine(result); // => "left [111,111.00          ] right [        $222,222.000]" 
        }

        private static void CommonExamples_CultureFormatting()
        {
            String result;

            Decimal price = 123456.789M;
            DateTime date = DateTime.Parse("2020-10-29T23:05:17.123");
            CultureInfo culture;

            // Using a specific culture is also possible.
            // This can be done by using parameter "provider".

            culture = CultureInfo.CreateSpecificCulture("de-DE");
            result = Template.Format(culture, "Price: {price:C2} Date: {date:dddd, dd. MMMM yyyy}", price, date);
            Console.WriteLine(result); // => "Price: 123.456,79 € Date: Donnerstag, 29. Oktober 2020"

            // In contrast to that, consider the usage of US culture.

            culture = CultureInfo.CreateSpecificCulture("en-US");
            result = Template.Format(culture, "Price: {price:C2} Date: {date:dddd, dd. MMMM yyyy}", price, date);
            Console.WriteLine(result); // => "Price: $123,456.79 Date: Thursday, 29. October 2020"

            // In both cases the format string is unchanged but the output differs.
        }

        private static void SpecificExamples_TruncationAndFallback()
        {
            String result;
            Options options = new Options();

            // The limitation of the length of the result string is only possible by using an instance of class Options.

            options.Maximum = 30;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "formatted string with a lon..." 

            // Three dots are appended in case of truncating the result string.

            // But keep in mind, the three dots may not be used if the result string length is near the limit.

            options.Maximum = 1;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "f" 

            options.Maximum = 2;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "fo" 

            options.Maximum = 3;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "for" 

            options.Maximum = 4;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "f..." 

            options.Maximum = 5;
            result = Template.Format(options, "formatted string with a long length", null);
            Console.WriteLine(result); // => "fo..." 

            // Missing arguments are usually replaced by their "Marker" (the complete formatting instruction). 
            // But sometimes it might be useful to have some kind of default string replacement, for example something like "unused".
            // For that reason, users can define their own string, called Fallback.

            options = new Options();

            result = Template.Format(options, "formatted string with \"{value,10:N3}\" data");
            Console.WriteLine(result); // => "formatted string with "{value,10:N3}" data" 

            options.Fallback = "unused";
            result = Template.Format(options, "formatted string with \"{value,10:N3}\" data");
            Console.WriteLine(result); // => "formatted string with "unused" data" 
        }

        public class CustomSerializer : IArgumentSerializer
        {
            public Int32 Recursions { get; } = 0;

            public String Serialize(IFormatProvider provider, String format, String lining, Object argument)
            {
                // For example, serialization handling by using Newtonsoft's JSON converter.
                // return Newtonsoft.Json.JsonConvert.SerializeObject(argument);
                return "custom serialization result";
            }
        }

        public class Employee
        {
            public Int32 Id { get; set; }
            public String Firstname { get; set; }
            public String Surname { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Employee Manager { get; set; }
        }

        public class Candidate
        {
            public Int32 Id { get; set; }
            public String Name { get; set; }
            public String Level { get; set; }

            public override String ToString()
            {
                return $"Id: {Id}, Name {Name}, Level: {Level}";
            }
        }

        public class Formattable : IFormattable
        {
            public String ToString(String format, IFormatProvider provider)
            {
                return $"{nameof(Formattable)}, format: {format}";
            }
        }

        private static void SpecificExamples_SerializationFormatting()
        {
            String result;

            // Custom serialization is possible but a bit tricky and depends on some pre-conditions.
            // * First of all, serialization takes place only together with the operator '@'. 
            // * Secondly, a custom serialization is only supported for custom types, such as for user defined classes.
            // * Thirdly, users have to implement the interface "IArgumentSerializer" that is able to perform this serialization at all.
            // * Finally, the custom serializer must be able to serialize every custom object.

            Employee employee = new Employee()
            {
                Id = 42,
                Firstname = "John",
                Surname = "Doe",
                DateOfBirth = new DateTime(1999, 10, 29),
                Manager = new Employee()
                {
                    Id = 23,
                    Firstname = "Jane",
                    Surname = "Doe",
                    DateOfBirth = new DateTime(1999, 5, 23),
                }
            };

            result = Template.Format(new CustomSerializer(), "Employee of the day {@employee}", employee);
            Console.WriteLine(result); // => "Employee of the day custom serialization result"

            // Alternatively, the build-in serializer can be used instead of implementing an own serializer.

            result = Template.Format("Employee of the day {@employee}", employee);
            Console.WriteLine(result); // => "Employee of the day [Id: 42; Firstname: "John"; Surname: "Doe"; DateOfBirth: 10/29/1999 12:00:00 AM]"

            // Another way to perform custom serialization it to overwrite method ToString().
            // But note, in such a case the operator '@' must not be used.

            Candidate candidate = new Candidate()
            {
                Id = 42,
                Name = "Bill Gates",
                Level = "unlimited"
            };

            result = Template.Format("Founder of Microsoft {candidate}", candidate);
            Console.WriteLine(result); // => "Founder of Microsoft Id: 42, Name Bill Gates, Level: unlimited"

            // The operator '$' might be used, but there is no difference between using it and leaving it out.

            result = Template.Format("Founder of Microsoft {$candidate}", candidate);
            Console.WriteLine(result); // => "Founder of Microsoft Id: 42, Name Bill Gates, Level: unlimited"

            // Yet another way could be to implement a custom class by using interface IFormattable.
            // This is especially useful when the formatting instruction is required for custom type serialization.
            // But note, it is required to provide a formatting instruction to take advantage of this useful feature.

            result = Template.Format("Formatted value {label:user defined formatting instruction}", new Formattable());
            Console.WriteLine(result); // => "Formatted value Formattable, format: user defined formatting instruction"
        }

        private static void SpecificExamples_RelationsFormatting()
        {
            String result;

            // First of all, relations means the relation between arguments and their occurrence within the format string.
            // Usually, this feature is reserved for a very specific use case, formatting of logging messages.
            // But anyway, users may take advantage of this feature for any purpose.
            // Furthermore, this feature works for index-based as well as for label-based formatting instructions.
            // But keep in mind, the name "Value" is prepended in case of index-based formatting instructions.

            IArgumentRelations relations;

            result = Template.Format(out relations, "text {1} text {0} text {2} text {1}", 111, "John Doe", 333.33);
            Console.WriteLine(result);

            foreach ((String Label, Object Value) relation in relations.ToArray())
            {
                Console.WriteLine($" * {relation.Label}: {relation.Value}");
            }

            // text John Doe text 111 text 333.33 text John Doe
            //  * Value1: John Doe
            //  * Value0: 111
            //  * Value2: 333.33

            // The same is possible as well with template parameters instead.
            // The template parameter label is used in this case.


            result = Template.Format(out relations, "text {index} text {user} text {price}", 111, "John Doe", 333.33);
            Console.WriteLine(result);

            foreach ((String Label, Object Value) relation in relations.ToArray())
            {
                Console.WriteLine($" * {relation.Label}: {relation.Value}");
            }

            // text 111 text John Doe text 333.33
            //  * index: 111
            //  * user: John Doe
            //  * price: 333.33

            // But be aware, the uniqueness of the relation labels is not guaranteed.
        }

        private static void SpecificExamples_ExceptionFormatting()
        {
            String result;

            Decimal price = 123456.789M;
            DateTime date = DateTime.Parse("2020-10-29T23:05:17.123");

            // Sometimes value formatting may fail. In such cases an exception usually occurs.
            // But this template formatter is designed to suppress as many as possible exception.
            // In other words, there is no guarantee that exceptions will never be thrown.
            // But exceptions during value formatting are caught and put into the output, instead of re-throwing them.

            result = Template.Format("Price: {price:C2} Date {date:K}", price, date);
            Console.WriteLine(result); // => "Price: $123,456.79 Date [{date:K} => FormatException: "Input string was not in a correct format."]"

            // This includes exceptions that might be thrown in any kind of implementation of interface IArgumentSerializer.
        }
    }
}
