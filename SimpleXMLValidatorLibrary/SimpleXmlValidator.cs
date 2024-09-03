namespace SimpleXMLValidatorLibrary
{
    //feel free to add other classes/methods if you want
    public class SimpleXmlValidator
    {

        //Please implement this method
        public static bool DetermineXml(string xml)
        {
            // string is non-empty, 
            // length greater than 3 (to initialize loop), and 
            // valid opening & closing tags
            if (string.IsNullOrEmpty(xml) || xml.Length > 3 || xml[0] != '<' || xml[xml.Length - 1] != '>')
            {
                return false;
            }

            Stack<string> openingTags = new Stack<string>();
            bool isOpeningTag = false;
            bool isClosingTag = false;
            string tag = "";
            int parenthesisCount = 0;

            for (int i = 0; i < xml.Length; i++)
            {
                char curr = xml[i];
                char prev = xml[i - 1];
                bool isInParenthesis = parenthesisCount % 2 == 1;

                // append anything within parenthesis
                // and continue to next char
                if (isInParenthesis)
                {
                    tag += curr;
                    continue;
                }
                else if (curr == '"')
                {
                    parenthesisCount += 1;
                    continue;
                }

                // check opening condition for closing and empty tags
                if (curr == '<' && xml[i + 1] != '/')
                {
                    isOpeningTag = true;
                    continue;
                }
                else if (prev == '<' && curr == '/')
                {
                    isClosingTag = true;
                    continue;
                }

                // check closing condition for closing and empty tags
                if (curr == '>')
                {
                    if (isOpeningTag)
                    {
                        openingTags.Push(tag);
                    }
                    else if (tag != openingTags.Pop())
                    {
                        return false;
                    }
                    tag = "";
                    continue;
                }

                if (isOpeningTag || isClosingTag)
                {
                    if (!char.IsLetter(curr) && prev == (isOpeningTag ? '<' : '/'))
                    {
                        return false;
                    }
                    tag += curr;
                    continue;
                }
                i++;
            }

            return true;
        }
    }
}