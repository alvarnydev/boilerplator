﻿using CodeGenerator.Datamodel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Generator
{
    public class BaseGenerator
    {

        /// <summary> Variable to store the currently used file. </summary> 
        public StreamWriter outputFile;

        /// <summary> Variable which will contain the needed information for the class generator logic. </summary> 
        private UML_BaseExtension classOrInterface;
        private bool isClass;

        /// <summary> Tab needed for indentation purposes.</summary>
        public string structureTab = "\t";


        // Constructor
        public BaseGenerator(StreamWriter outputFile, UML_BaseExtension classOrInterface)
        {
            this.outputFile = outputFile;
            this.classOrInterface = classOrInterface;

            // Determine object type
            this.isClass = classOrInterface.GetType() == typeof(UML_Class) ? true : false;
        }

        public void generateContent()
        {
            // Write beginning
            writeBeginning();

            // Write attributes
            writeAttributes();

            // Write methods
            writeMethods();

            // Ending line
            outputFile.WriteLine("}");
        }

        /// <summary> Writes the introductory class line with the name matching what is found in the UML_Class object. </summary>
        void writeBeginning()
        {
            // First line
            StringBuilder sb = writeBeginning_FirstLine(classOrInterface);

            // Last line
            sb.Append("\n{\n");

            // Write out built string
            outputFile.WriteLine(sb.ToString());
        }



        void writeAttributes()
        {

            // Comment in file
            outputFile.WriteLine($"{structureTab}// Attributes");

            // Extract attributes
            List<UML_Attribute> umlAttributes = classOrInterface.umlAttributes;

            // Iterate over attribute list
            foreach (UML_Attribute umlAttribute in umlAttributes)
            {
                // Write attribute to file
                string attributeString = writeAttribute(umlAttribute);

                outputFile.WriteLine(structureTab + attributeString);
            }

            // Trailing line
            outputFile.WriteLine("");

        }

        

        /// <summary> Writes empty functions into a specified file with the name and parameters matching what is found in the passed list of methods that belong to the current class. </summary>
        void writeMethods()
        {
            // Comment in file
            outputFile.WriteLine($"{structureTab}// Methods");

            // Extract methods
            List<UML_Method> umlMethods = classOrInterface.umlMethods;

            // Iterate over method list
            foreach (UML_Method umlMethod in umlMethods)
            {
                StringBuilder sb = new StringBuilder();

                // First line
                sb.Append($"{structureTab}{umlMethod.accessModifier} {umlMethod.type} {umlMethod.name}(");

                // Append parameters
                if (umlMethod.parameters.Count > 0)
                {
                    foreach (UML_Parameter umlParameter in umlMethod.parameters)
                    {
                        sb.Append($"{umlParameter.parameterName} {umlParameter.parameterType}, ");
                    }
                    sb.Length -= 2;
                }

                // Close parantheses
                sb.Append(")");

                // Write body
                writeMethod_Body(sb);

                // Write built string
                outputFile.WriteLine(sb.ToString());

            }
        }



        // To be overwritten in inheriting classes
        public virtual StringBuilder writeBeginning_FirstLine(UML_BaseExtension classOrInterface)
        {
            return new StringBuilder();
        }

        public virtual string writeAttribute(UML_Attribute umlAttribute)
        {
            return "";
        }

        public virtual void writeMethod_Body(StringBuilder sb)
        {

        }

    }
}
