namespace TeamBuild.Core.UnitTests;

[Trait("Category", "Unit")]
[Trait("Area", "Core")]
[Trait("Project", "Preamble")]
public class StringExtensionsTests
{
    #region [ ToTextSearchData/Query ]

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
    [ClassData(typeof(ToTextSearchQueryTestData))]
    public void ToTextSearchQueryTest(string query, string[] expected)
    {
        // Act
        var actual = query.ToTextSearchQuery();

        // Assert
        actual.ShouldBeEquivalentTo(expected);
    }

    public class ToTextSearchQueryTestData : TheoryData<string, string[]>
    {
        public ToTextSearchQueryTestData()
        {
            Add("", []);
        }
    }

    #endregion [ ToTextSearchData/Query ]

    #region [ FoldToASCII ]

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

            // k
            Add("K Ķ Ƙ Ǩ ᴋ Ḱ Ḳ Ḵ Ⓚ Ⱪ Ꝁ Ꝃ Ꝅ Ｋ", "K K K K K K K K K K K K K K");
            Add("k ķ ƙ ǩ ʞ ᶄ ḱ ḳ ḵ ⓚ ⱪ ꝁ ꝃ ꝅ ｋ", "k k k k k k k k k k k k k k k");
            Add("⒦", "(k)");

            // l
            Add(
                "L Ĺ Ļ Ľ Ŀ Ł Ƚ ʟ ᴌ Ḷ Ḹ Ḻ Ḽ Ⓛ Ⱡ Ɫ Ꝇ Ꝉ Ꞁ Ｌ",
                "L L L L L L L L L L L L L L L L L L L L"
            );
            Add(
                "l ĺ ļ ľ ŀ ł ƚ ȴ ɫ ɬ ɭ ᶅ ḷ ḹ ḻ ḽ ⓛ ⱡ ꝇ ꝉ ꞁ ｌ",
                "l l l l l l l l l l l l l l l l l l l l l l"
            );
            Add("Ǉ", "LJ");
            Add("Ỻ", "LL");
            Add("ǈ", "Lj");
            Add("⒧", "(l)");
            Add("ǉ", "lj");
            Add("ỻ", "ll");
            Add("ʪ", "ls");
            Add("ʫ", "lz");

            // m
            Add("M Ɯ ᴍ Ḿ Ṁ Ṃ Ⓜ Ɱ ꟽ ꟿ Ｍ", "M M M M M M M M M M M");
            Add("m ɯ ɰ ɱ ᵯ ᶆ ḿ ṁ ṃ ⓜ ｍ", "m m m m m m m m m m m");
            Add("⒨", "(m)");

            // n
            Add("N Ñ Ń Ņ Ň Ŋ Ɲ Ǹ Ƞ ɴ ᴎ Ṅ Ṇ Ṉ Ṋ Ⓝ Ｎ", "N N N N N N N N N N N N N N N N N");
            Add(
                "n ñ ń ņ ň ŉ ŋ ƞ ǹ ȵ ɲ ɳ ᵰ ᶇ ṅ ṇ ṉ ṋ ⁿ ⓝ ｎ",
                "n n n n n n n n n n n n n n n n n n n n n"
            );
            Add("Ǌ", "NJ");
            Add("ǋ", "Nj");
            Add("⒩", "(n)");
            Add("ǌ", "nj");

            // o
            Add(
                "O Ò Ó Ô Õ Ö Ø Ō Ŏ Ő Ɔ Ɵ Ơ Ǒ Ǫ Ǭ Ǿ Ȍ Ȏ Ȫ Ȭ Ȯ Ȱ ᴏ ᴐ Ṍ Ṏ Ṑ Ṓ Ọ Ỏ Ố Ồ Ổ Ỗ Ộ Ớ Ờ Ở Ỡ Ợ Ⓞ Ꝋ Ꝍ Ｏ",
                "O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O O"
            );
            Add(
                "o ò ó ô õ ö ø ō ŏ ő ơ ǒ ǫ ǭ ǿ ȍ ȏ ȫ ȭ ȯ ȱ ɔ ɵ ᴖ ᴗ ᶗ ṍ ṏ ṑ ṓ ọ ỏ ố ồ ổ ỗ ộ ớ ờ ở ỡ ợ ₒ ⓞ ⱺ ꝋ ꝍ ｏ",
                "o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o"
            );
            Add("Œ ɶ", "OE OE");
            Add("Ꝏ", "OO");
            Add("Ȣ ᴕ", "OU OU");
            Add("⒪", "(o)");
            Add("œ ᴔ", "oe oe");
            Add("ꝏ", "oo");
            Add("ȣ", "ou");

            // p
            Add("P Ƥ ᴘ Ṕ Ṗ Ⓟ Ᵽ Ꝑ Ꝓ Ꝕ Ｐ", "P P P P P P P P P P P");
            Add("p ƥ ᵱ ᵽ ᶈ ṕ ṗ ⓟ ꝑ ꝓ ꝕ ꟼ ｐ", "p p p p p p p p p p p p p");
            Add("⒫", "(p)");

            // q
            Add("Q Ɋ Ⓠ Ꝗ Ꝙ Ｑ", "Q Q Q Q Q Q");
            Add("q ĸ ɋ ʠ ⓠ ꝗ ꝙ ｑ", "q q q q q q q q");
            Add("⒬", "(q)");
            Add("ȹ", "qp");

            // r
            Add(
                "R Ŕ Ŗ Ř Ȓ Ȓ Ɍ ʀ ʁ ᴙ ᴚ Ṙ Ṛ Ṝ Ṟ Ⓡ Ɽ Ꝛ Ꞃ Ｒ",
                "R R R R R R R R R R R R R R R R R R R R"
            );
            Add(
                "r ŕ ŗ ř ȑ ȓ ɍ ɼ ɽ ɾ ɿ ᵣ ᵲ ᵳ ᶉ ṙ ṛ ṝ ṟ ⓡ ꝛ ꞃ ｒ",
                "r r r r r r r r r r r r r r r r r r r r r r r"
            );
            Add("⒭", "(r)");

            // s
            Add("S Ś Ŝ Ş Š Ș Ṡ Ṣ Ṥ Ṧ Ṩ Ⓢ ꜱ ꞅ Ｓ", "S S S S S S S S S S S S S S S");
            Add(
                "s ś ŝ ş š ſ ș ȿ ʂ ᵴ ᶊ ṡ ṣ ṥ ṧ ṩ ẜ ẝ ⓢ Ꞅ ｓ",
                "s s s s s s s s s s s s s s s s s s s s s"
            );
            Add("ẞ", "SS");
            Add("⒮", "(s)");
            Add("ß", "ss");
            Add("ﬆ", "st");

            // t
            Add("T Ţ Ť Ŧ Ƭ Ʈ Ț Ⱦ ᴛ Ṫ Ṭ Ṯ Ṱ Ⓣ Ꞇ Ｔ", "T T T T T T T T T T T T T T T T");
            Add("t ţ ť ŧ ƫ ƭ ț ȶ ʇ ʈ ᵵ ṫ ṭ ṯ ṱ ẗ ⓣ ⱦ ｔ", "t t t t t t t t t t t t t t t t t t t");
            Add("Þ Ꝧ", "TH TH");
            Add("Ꜩ", "TZ");
            Add("⒯", "(t)");
            Add("ʨ", "tc");
            Add("þ ᵺ ꝧ", "th th th");
            Add("ʦ", "ts");
            Add("ꜩ", "tz");

            // u
            Add(
                "U Ù Ú Û Ü Ũ Ū Ŭ Ů Ű Ų Ư Ǔ Ǖ Ǘ Ǚ Ǜ Ȕ Ȗ Ʉ ᴜ ᵾ Ṳ Ṵ Ṷ Ṹ Ṻ Ụ Ủ Ứ Ừ Ử Ữ Ự Ⓤ Ｕ",
                "U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U U"
            );
            Add(
                "u ù ú û ü ũ ū ŭ ů ű ų ư ǔ ǖ ǘ ǚ ǜ ȕ ȗ ʉ ᵤ ᶙ ṳ ṵ ṷ ṹ ṻ ụ ủ ứ ừ ử ữ ự ⓤ ｕ",
                "u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u u"
            );
            Add("⒰", "(u)");
            Add("ᵫ", "ue");

            // v
            Add("V Ʋ Ʌ ᴠ Ṽ Ṿ Ỽ Ⓥ Ꝟ Ꝩ Ｖ", "V V V V V V V V V V V");
            Add("v ʋ ʌ ᵥ ᶌ ṽ ṿ ⓥ ⱱ ⱴ ꝟ ｖ", "v v v v v v v v v v v v");
            Add("Ꝡ", "VY");
            Add("⒱", "(v)");
            Add("ꝡ", "vy");

            // w
            Add("W Ŵ Ƿ ᴡ Ẁ Ẃ Ẅ Ẇ Ẉ Ⓦ Ⱳ Ｗ", "W W W W W W W W W W W W");
            Add("w ŵ ƿ ʍ ẁ ẃ ẅ ẇ ẉ ẘ ⓦ ⱳ ｗ", "w w w w w w w w w w w w w");
            Add("⒲", "(w)");

            // x
            Add("X Ẋ Ẍ Ⓧ Ｘ", "X X X X X");
            Add("x ᶍ ẋ ẍ ₓ ⓧ ｘ", "x x x x x x x");
            Add("⒳", "(x)");

            // y
            Add("Y Ý Ŷ Ÿ Ƴ Ȳ Ɏ ʏ Ẏ Ỳ Ỵ Ỷ Ỹ Ỿ Ⓨ Ｙ", "Y Y Y Y Y Y Y Y Y Y Y Y Y Y Y Y");
            Add("y ý ÿ ŷ ƴ ȳ ɏ ʎ ẏ ẙ ỳ ỵ ỷ ỹ ỿ ⓨ ｙ", "y y y y y y y y y y y y y y y y y");
            Add("⒴", "(y)");

            // z
            Add("Z Ź Ż Ž Ƶ Ȝ Ȥ ᴢ Ẑ Ẓ Ẕ Ⓩ Ⱬ Ꝣ Ｚ", "Z Z Z Z Z Z Z Z Z Z Z Z Z Z Z");
            Add("z ź ż ž ƶ ȝ ȥ ɀ ʐ ʑ ᵶ ᶎ ẑ ẓ ẕ ⓩ ⱬ ꝣ ｚ", "z z z z z z z z z z z z z z z z z z z");
            Add("⒵", "(z)");
        }
    }

    #endregion [ FoldToASCII ]
}
