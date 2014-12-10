namespace Odyssey.UserInterface.Style
{
    // Summary:
    //     Represents the style of a font face as normal, italic, or oblique.
    //
    // Remarks:
    //     Three terms categorize the slant of a font: normal, italic, and oblique.
    //     Font styleDescription NormalThe characters in a normal, or roman, font are
    //     upright. Italic The characters in an italic font are truly slanted and appear
    //     as they were designed. ObliqueThe characters in an oblique font are artificially
    //     slanted. ?For Oblique, the slant is achieved by performing a shear transformation
    //     on the characters from a normal font. When a true italic font is not available
    //     on a computer or printer, an oblique style can be generated from the normal
    //     font and used to simulate an italic font. The following illustration shows
    //     the normal, italic, and oblique font styles for the Palatino Linotype font.
    //     Notice how the italic font style has a more flowing and visually appealing
    //     appearance than the oblique font style, which is simply created by skewing
    //     the normal font style version of the text.Note?? Values other than the ones
    //     defined in the enumeration are considered to be invalid, and they are rejected
    //     by font API functions.
    public enum FontStyle
    {
        // Summary:
        //      Font style : Normal.
        Normal = 0,
        //
        // Summary:
        //      Font style : Oblique.
        Oblique = 1,
        //
        // Summary:
        //      Font style : Italic.
        Italic = 2,
    }

    // Summary:
    //     Represents the density of a typeface, in terms of the lightness or heaviness
    //     of the strokes. The enumerated values correspond to the usWeightClass definition
    //     in the OpenType specification. The usWeightClass represents an integer value
    //     between 1 and 999. Lower values indicate lighter weights; higher values indicate
    //     heavier weights.
    //
    // Remarks:
    //     Weight differences are generally differentiated by an increased stroke or
    //     thickness that is associated with a given character in a typeface, as compared
    //     to a "normal" character from that same typeface. The following illustration
    //     shows an example of Normal and UltraBold weights for the Palatino Linotype
    //     typeface.Note??Not all weights are available for all typefaces. When a weight
    //     is not available for a typeface, the closest matching weight is returned.Font
    //     weight values less than 1 or greater than 999 are considered invalid, and
    //     they are rejected by font API functions.
    public enum FontWeight
    {
        // Summary:
        //      Predefined font weight : Thin (100).
        Thin = 100,
        //
        // Summary:
        //      Predefined font weight : Extra-light (200).
        ExtraLight = 200,
        //
        // Summary:
        //      Predefined font weight : Ultra-light (200).
        UltraLight = 200,
        //
        // Summary:
        //      Predefined font weight : Light (300).
        Light = 300,
        //
        // Summary:
        //      Predefined font weight : Normal (400).
        SemiLight = 350,
        //
        // Summary:
        //      Predefined font weight : Regular (400).
        Normal = 400,
        //
        // Summary:
        //      Predefined font weight : Medium (500).
        Regular = 400,
        //
        // Summary:
        //      Predefined font weight : Demi-bold (600).
        Medium = 500,
        //
        // Summary:
        //      Predefined font weight : Bold (700).
        SemiBold = 600,
        //
        // Summary:
        //      Predefined font weight : Semi-bold (600).
        DemiBold = 600,
        //
        // Summary:
        //      Predefined font weight : Extra-bold (800).
        Bold = 700,
        //
        // Summary:
        //      Predefined font weight : Black (900).
        UltraBold = 800,
        //
        // Summary:
        //      Predefined font weight : Ultra-bold (800).
        ExtraBold = 800,
        //
        // Summary:
        //      Predefined font weight : Extra-black (950).
        Heavy = 900,
        //
        // Summary:
        //      Predefined font weight : Heavy (900).
        Black = 900,
        //
        // Summary:
        //      Predefined font weight : Ultra-black (950).
        ExtraBlack = 950,
        //
        // Summary:
        //     No documentation.
        UltraBlack = 950,
    }

    // Summary:
    //     Specifies the alignment of paragraph text along the reading direction axis,
    //     relative to the leading and trailing edge of the layout box.
    public enum TextAlignment
    {
        // Summary:
        //      The leading edge of the paragraph text is aligned to the leading edge of
        //     the layout box.
        Leading = 0,
        //
        // Summary:
        //      The trailing edge of the paragraph text is aligned to the trailing edge
        //     of the layout box.
        Trailing = 1,
        //
        // Summary:
        //      The center of the paragraph text is aligned to the center of the layout
        //     box.
        Center = 2,
        //
        // Summary:
        //     No documentation.
        Justified = 3,
    }

    // Summary:
    //      Specifies the alignment of paragraph text along the flow direction axis,
    //     relative to the top and bottom of the flow's layout box.
    public enum ParagraphAlignment
    {
        // Summary:
        //      The top of the text flow is aligned to the top edge of the layout box.
        Near = 0,
        //
        // Summary:
        //      The bottom of the text flow is aligned to the bottom edge of the layout
        //     box.
        Far = 1,
        //
        // Summary:
        //      The center of the flow is aligned to the center of the layout box.
        Center = 2,
    }
}