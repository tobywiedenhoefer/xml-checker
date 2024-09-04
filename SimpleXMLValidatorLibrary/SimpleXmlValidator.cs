namespace SimpleXMLValidatorLibrary
{
    //feel free to add other classes/methods if you want
    public class SimpleXmlValidator
    {
        private enum Cases
        {
            Skip = -1,
            CurrCharIsParenthesisChar = 0,
            CurrCharIsInQuotes = 1,
            IsOpeningTagStart = 2,
            IsOpeningTagEnd = 3,
            IsClosingTagStart = 4,
            IsClosingTagEnd = 5,
            TryAppendingCharToTag = 6,
        }

        private static Cases DetermineCase(char curr, char prev, bool isInParenthesis, bool isOpeningTag, bool isClosingTag)
        {
            // Determine the case of which the curr and prev characters will be evaluated in the DetermineXML class.

            // Parenthesis/Quotes
            if (curr == '"')
            {
                return Cases.CurrCharIsParenthesisChar;
            }
            if (isInParenthesis)
            {
                return Cases.CurrCharIsInQuotes;
            }

            // Start of tag (Opening or Closing)
            if (prev == '<')
            {
                if (curr == '/')
                {
                    return Cases.IsClosingTagStart;
                }
                else
                {
                    return Cases.IsOpeningTagStart;
                }
            }

            // End of tag (Opening or Closing)
            if (curr == '>')
            {
                if (isOpeningTag)
                {
                    return Cases.IsOpeningTagEnd;
                }
                else if (isClosingTag)
                {
                    return Cases.IsClosingTagEnd;
                }
            }

            // Tells logic to try appending valid chars to tag
            if (isOpeningTag || isClosingTag)
            {
                return Cases.TryAppendingCharToTag;
            }

            return Cases.Skip;  // These are characters between '>' and '<'. ex: <root>these chars</root>
        }

        //Please implement this method
        public static bool DetermineXml(string xml)
        {
            // Iterates over every char in the string, pushing opening tags to a stack and popping matching tags.
            // Returns true if XML is valid, else false.

            if (string.IsNullOrEmpty(xml) || xml.Length < 2 || xml[0] != '<' || xml[xml.Length - 1] != '>')
            {
                // string is non-empty, 
                // length is less than 2 (where iteration starts below), and 
                // valid opening ('<') & closing ('>') chars at beginning and end of string
                return false;
            }

            Stack<string> openingTags = new Stack<string>();  // used to push/pop tags to compare validity
            bool isOpeningTag = false;  // defines state. if true, tag will be appended to openingTags when '>' is reached.
            bool isClosingTag = false;  // defines state. if true, tag will be compared to popped value of openingTags.
            string tag = "";  // built from chars when isOpeningTag or isClosingTag is true.
            int parenthesisCount = 0;  // defines state. useful for defining validity of characters within parenthesis/quotes.
            int pairsMatched = 0;  // number of pairs of tags matched. used to determine if multiple roots exist.

            for (int i = 1; i < xml.Length; i++)
            {
                char curr = xml[i];
                char prev = xml[i - 1];

                // logic controller
                Cases currentCase = DetermineCase(curr, prev, parenthesisCount % 2 == 1, isOpeningTag, isClosingTag);

                switch (currentCase)
                {
                    case Cases.CurrCharIsParenthesisChar:
                        // curr is '"'
                        parenthesisCount += 1;
                        tag += curr;
                        break;
                    case Cases.CurrCharIsInQuotes:
                        // curr is currently between quotes, append whatever's between the quotes.
                        // creating a seperate case for this allows for future restricting of an element's attribute.
                        // ex: <root attr="<elem />"></root> can be defined as valid or invalid here.
                        tag += curr;
                        break;
                    case Cases.IsOpeningTagStart:
                        // previous char is '<' and curr char is not '/'.
                        // chars from here on will be added to "tag" until the '>' char.
                        isOpeningTag = true;
                        tag = curr.ToString();
                        break;
                    case Cases.IsOpeningTagEnd:
                        // curr char is '>'.
                        // this can be expanded to include single elements:
                        // ex: <node />
                        // we would just need to check if prev is '/', then break early.
                        if (pairsMatched > 0 && openingTags.Count == 0)
                        {
                            // if more than one pairs have been matched and the openingTags stack is empty,
                            // there are multiple roots to the xml string, which is not valid.
                            return false;
                        }
                        openingTags.Push(tag);
                        isOpeningTag = false;
                        tag = "";
                        break;
                    case Cases.IsClosingTagStart:
                        // previous char is '<' and curr char is '/'.
                        // chars from here on will be added to "tag" until curr is '>'.
                        isClosingTag = true;
                        break;
                    case Cases.IsClosingTagEnd:
                        // curr char is '>'.
                        // compare "tag" with a popped var from openingTags.
                        // Check validity "tag" with popped openingTags value.
                        isClosingTag = false;
                        try
                        {
                            if (tag != openingTags.Pop())
                            {
                                return false;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            return false;
                        }
                        tag = "";
                        pairsMatched += 1;
                        break;
                    case Cases.TryAppendingCharToTag:
                        // if curr char is not an invalid char, append it to "tag".
                        // if it is invalid, return early as XML is not valid.
                        if (!char.IsLetter(curr) && prev == (isOpeningTag ? '<' : '/'))  // keeps non-letters from proceeding '<' or "</". ex: "< elem/>"
                        {
                            return false;
                        }
                        tag += curr;
                        break;
                }

            }
            return openingTags.Count == 0;  // stack is empty if all tags have been popped
        }
    }
}