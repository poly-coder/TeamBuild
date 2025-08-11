namespace TeamBuild.Core.UnitTests;

[Trait("Category", "Unit")]
[Trait("Area", "Core")]
[Trait("Project", "Preamble")]
public class StringExtensionsTests
{
    [Theory]
    [ClassData(typeof(ToTextSearchDataTestData))]
    public void ToTextSearchDataTest(string[] inputs, string expected)
    {
        // Act
        var actual = StringExtensions.ToTextSearchData(inputs);

        // Assert
        actual.ShouldBe(expected);
    }

    public class ToTextSearchDataTestData : TheoryData<string[], string>
    {
        public ToTextSearchDataTestData()
        {
            Add([], "");
            Add(["abc xyz"], "abcxyz");
            Add(["abc", "xyz"], "abc/xyz");
            Add(["á ⒝ Ċ", "Ẋ-Ỿ-Ⓩ", "₀\\①/⒉"], "abc/xyz/012");
        }
    }

    [Theory]
    [ClassData(typeof(FoldToASCIITestData))]
    public void FoldToASCIITest(string input, string expected)
    {
        // Act
        var actual = input.FoldToASCII();

        // Assert
        actual.ShouldBe(expected);
    }

    public class FoldToASCIITestData : TheoryData<string, string>
    {
        public FoldToASCIITestData()
        {
            Add("", "");

            // a
            Add(
                "A À Á Â Ã Ä Å Ā Ă Ą Ə Ǎ Ǟ Ǡ Ǻ Ȁ Ȃ Ȧ Ⱥ ᴀ Ḁ Ạ Ả Ấ Ầ Ẩ Ẫ Ậ Ắ Ằ Ẳ Ẵ Ặ Ⓐ Ａ",
                "A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A"
            );
            Add(
                "a à á â ã ä å ā ă ą ǎ ǟ ǡ ǻ ȁ ȃ ȧ ɐ ə ɚ ᶏ ᶕ ạ ả ạ ả ấ ầ ẩ ẫ ậ ắ ằ ẳ ẵ ặ ₐ ₔ ⓐ ⱥ Ɐ ａ",
                "a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a a"
            );
            Add("Ꜳ", "AA");
            Add("Æ Ǣ Ǽ ᴁ", "AE AE AE AE");
            Add("Ꜵ", "AO");
            Add("Ꜷ", "AU");
            Add("Ꜹ Ꜻ", "AV AV");
            Add("Ꜽ", "AY");
            Add("⒜", "(a)");
            Add("ꜳ", "aa");
            Add("æ ǣ ǽ ᴂ", "ae ae ae ae");
            Add("ꜵ", "ao");
            Add("ꜵ", "ao");
            Add("ꜷ", "au");
            Add("ꜹ ꜻ", "av av");
            Add("ꜽ", "ay");

            // b
            Add("B Ɓ Ƃ Ƀ ʙ ᴃ Ḃ Ḅ Ḇ Ⓑ Ｂ", "B B B B B B B B B B B");
            Add("b ƀ ƃ ɓ ᵬ ᶀ ḃ ḅ ḇ ⓑ ｂ", "b b b b b b b b b b b");
            Add("⒝", "(b)");

            // c
            Add("C Ç Ć Ĉ Ċ Č Ƈ Ȼ ʗ ᴄ Ḉ Ⓒ Ｃ", "C C C C C C C C C C C C C");
            Add("c ç ć ĉ ċ č ƈ ȼ ɕ ḉ ↄ ⓒ Ꜿ ꜿ ｃ", "c c c c c c c c c c c c c c c");
            Add("⒞", "(c)");

            // d
            Add("D Ð Ď Đ Ɖ Ɗ Ƌ ᴅ ᴆ Ḋ Ḍ Ḏ Ḑ Ḓ Ⓓ Ꝺ Ｄ", "D D D D D D D D D D D D D D D D D");
            Add("d ð ď đ ƌ ȡ ɖ ɗ ᵭ ᶁ ᶑ ḋ ḍ ḏ ḑ ḓ ⓓ ꝺ ｄ", "d d d d d d d d d d d d d d d d d d d");
            Add("Ǆ Ǳ", "DZ DZ");
            Add("ǅ ǲ", "Dz Dz");
            Add("⒟", "(d)");
            Add("ȸ", "db");
            Add("ǆ ǳ ʣ ʥ", "dz dz dz dz");

            // e
            Add(
                "E È É Ê Ë Ē Ĕ Ė Ę Ě Ǝ Ɛ Ȅ Ȇ Ȩ Ɇ ᴇ Ḕ Ḗ Ḙ Ḛ Ḝ Ẹ Ẻ Ẽ Ế Ề Ể Ễ Ệ Ⓔ ⱻ Ｅ",
                "E E E E E E E E E E E E E E E E E E E E E E E E E E E E E E E E E"
            );
            Add(
                "e è é ê ë ē ĕ ė ę ě ǝ ȅ ȇ ȩ ɇ ɘ ɛ ɜ ɝ ɞ ʚ ᴈ ᶒ ᶓ ᶔ ḕ ḗ ḙ ḛ ḝ ẹ ẻ ẽ ế ề ể ễ ệ ₑ ⓔ ⱸ ｅ",
                "e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e e"
            );
            Add("⒠", "(e)");

            // f
            Add("F Ƒ Ḟ Ⓕ ꜰ Ꝼ ꟻ Ｆ", "F F F F F F F F");
            Add("f ƒ ᵮ ᶂ ḟ ẛ ⓕ ꝼ ｆ", "f f f f f f f f f");
            Add("⒡", "(f)");
            Add("ﬀ", "ff");
            Add("ﬃ", "ffi");
            Add("ﬄ", "ffl");
            Add("ﬁ", "fi");
            Add("ﬂ", "fl");

            // g
            Add("G Ĝ Ğ Ġ Ģ Ɠ Ǥ ǥ Ǧ ǧ Ǵ ɢ ʛ Ḡ Ⓖ Ᵹ Ꝿ Ｇ", "G G G G G G G G G G G G G G G G G G");
            Add("g ĝ ğ ġ ģ ǵ ɠ ɡ ᵷ ᵹ ᶃ ḡ ⓖ ꝿ ｇ", "g g g g g g g g g g g g g g g");
            Add("⒢", "(g)");

            // h
            Add("H Ĥ Ħ Ȟ ʜ Ḣ Ḥ Ḧ Ḩ Ḫ Ⓗ Ⱨ Ⱶ Ｈ", "H H H H H H H H H H H H H H");
            Add("h ĥ ħ ȟ ɥ ɦ ʮ ʯ ḣ ḥ ḧ ḩ ḫ ẖ ⓗ ⱨ ⱶ ｈ", "h h h h h h h h h h h h h h h h h h");
            Add("Ƕ", "HV");
            Add("⒣", "(h)");
            Add("ƕ", "hv");

            // i
            Add(
                "I Ì Í Î Ï Ĩ Ī Ĭ Į İ Ɩ Ɨ Ǐ Ȉ Ȋ ɪ ᵻ Ḭ Ḯ Ỉ Ị Ⓘ ꟾ Ｉ",
                "I I I I I I I I I I I I I I I I I I I I I I I I"
            );
            Add(
                "i ì í î ï ĩ ī ĭ į ı ǐ ȉ ȋ ɨ ᴉ ᵢ ᵼ ᶖ ḭ ḯ ỉ ị ⁱ ⓘ ｉ",
                "i i i i i i i i i i i i i i i i i i i i i i i i i"
            );
            Add("Ĳ", "IJ");
            Add("⒤", "(i)");
            Add("ĳ", "ij");

            // j
            Add("J Ĵ Ɉ ᴊ Ⓙ Ｊ", "J J J J J J");
            Add("j ĵ ǰ ȷ ɉ ɟ ʄ ʝ ⓙ ⱼ ｊ", "j j j j j j j j j j j");
            Add("⒥", "(j)");
        }
    }
}
