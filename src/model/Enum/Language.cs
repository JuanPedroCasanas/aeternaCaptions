using System.ComponentModel;
using System.Runtime.Serialization;

namespace aeternaCaptions.src.model
{
    public enum Language
    {
        [Description("Undefined")]
        [EnumMember(Value = "")]
        undefined,
        
        [Description("Global English")]
        [EnumMember(Value = "en")]
        en,

        [Description("Australian English")]
        [EnumMember(Value = "en_au")]
        en_au,

        [Description("British English")]
        [EnumMember(Value = "en_uk")]
        en_uk,

        [Description("US English")]
        [EnumMember(Value = "en_us")]
        en_us,

        [Description("Spanish")]
        [EnumMember(Value = "es")]
        es,

        [Description("French")]
        [EnumMember(Value = "fr")]
        fr,

        [Description("German")]
        [EnumMember(Value = "de")]
        de,

        [Description("Italian")]
        [EnumMember(Value = "it")]
        it,

        [Description("Portuguese")]
        [EnumMember(Value = "pt")]
        pt,

        [Description("Dutch")]
        [EnumMember(Value = "nl")]
        nl,

        [Description("Hindi")]
        [EnumMember(Value = "hi")]
        hi,

        [Description("Japanese")]
        [EnumMember(Value = "ja")]
        ja,

        [Description("Chinese")]
        [EnumMember(Value = "zh")]
        zh,

        [Description("Finnish")]
        [EnumMember(Value = "fi")]
        fi,

        [Description("Korean")]
        [EnumMember(Value = "ko")]
        ko,

        [Description("Polish")]
        [EnumMember(Value = "pl")]
        pl,

        [Description("Russian")]
        [EnumMember(Value = "ru")]
        ru,

        [Description("Turkish")]
        [EnumMember(Value = "tr")]
        tr,

        [Description("Ukrainian")]
        [EnumMember(Value = "uk")]
        uk,

        [Description("Vietnamese")]
        [EnumMember(Value = "vi")]
        vi
    }
}