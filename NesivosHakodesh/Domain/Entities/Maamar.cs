using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NesivosHakodesh.Domain.Entities;
using NesivosHakodesh.Providers.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NesivosHakodesh.Domain.Entities
{
    public class Maamar : BaseEntity
    {
        public int MaamarID { get; set; }
        public Source Source { get; set; }
        public Topic Topic { get; set; }
        public List<MaamarTopic> SubTopics { get; set; }
        public MaamarType? Type { get; set; }
        public string StatusDetails { get; set; }
        public MaamarimStatus Status { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Parsha { get; set; }
        public string Year { get; set; }
        public string WeeklyIndex { get; set; }
        public string LocationDetails { get; set; }
        public DateTime? Date { get; set; }
        public bool BechatzrPrinted { get; set; }
        public string BechatzrPrintedWeek { get; set; }
        public int AccuracyRate { get; set; }
        public string AccuracyDescriptin { get; set; }
        public bool Locked { get; set; } = false;

        //attachements
        public string OriginalFileName { get; set; }
        public string PdfFileName { get; set; }
        public string AudioFileName { get; set; }
        public Library TitleLibraryId { get; set; }

        public string Comments { get; set; }
        public List<MaamarParagraph> MaamarParagraphs { get; set; }

        public List<MaamarTorahLink> TorahLinks { get; set; }
        public List<MaamarLibraryLink> LibraryLink { get; set; }

        public string TypeValue => Util.GetEnumValue(Type);
        public string StatusValue => Util.GetEnumValue(Status);
        public string OriginalFileNameValue => OriginalFileName?.Replace($"{MaamarID}_word_", "");
        public string PdfFileNameValue => PdfFileName?.Replace($"{MaamarID}_pdf_", "");
        public string AudioFileNameValue => AudioFileName?.Replace($"{MaamarID}_audio_", "");

        public string OtherDetails
        {
            get
            {
                switch (Type)
                {
                    case MaamarType.PisguminKadishin:

                        return $"{Parsha} [{WeeklyIndex}]";

                    case MaamarType.BechatzarHakodesh:
                    case MaamarType.BH_BerchesKodesh:
                    case MaamarType.BH_MaameremProtem:
                    case MaamarType.BH_NoamSheikh:
                    case MaamarType.BH_SeperiKodesh:
                    case MaamarType.BH_SichesKodesh:


                        return BechatzrPrinted ? $"{BechatzrPrintedWeek}" : "עדיין לא פורסם";

                    default:
                        return "";
                }
            }
        }

        [NotMapped]
        public BaseEntity LastUpdatedObject { get; set; }
        public DateTime? LastUpdatedTime => LastUpdatedObject?.UpdatedTime;
        public User LastUpdatedUser => LastUpdatedObject?.UpdatedUser;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MaamarType
    {
        [EnumValue("פתגמין קדישין")]
        PisguminKadishin = 0,
        [EnumValue("הדרכות ישרות")]
        HadrachosYesharos = 1,
        [EnumValue("בחצר הקודש")]
        BechatzarHakodesh = 2,
        [EnumValue("הדרכות פרטיות")]
        Personals = 3,

        [EnumValue("נועם שיח")]
        BH_NoamSheikh = 10,
        [EnumValue("שיחות קודש")]
        BH_SichesKodesh = 11,
        [EnumValue("ברכות קודש והמסתעף")]
        BH_BerchesKodesh = 12,
        [EnumValue("מאמרים פרטיים")]
        BH_MaameremProtem = 13,
        [EnumValue("סיפורי קודש")]
        BH_SeperiKodesh = 14,




        None = 100,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MaamarimStatus
    {
        /*  [EnumValue("חדש")]
          New = 0,
          [EnumValue("ממתין")]
          Pending = 1,
          [EnumValue("הושלם")]
          Completed = 2,*/
        [EnumValue("נמחק")]
        Deleted = 3,

        [EnumValue("חדש בלי תוכן")]
        NewWithOutDetails = 10,
        [EnumValue("יש למלאות פרטים וקישורים")]
        NeedDetails = 11,
        [EnumValue("עדיין לא מוגה")]
        NotMuggedYet = 12,
        [EnumValue("יש עבודה עליו")]
        NeedToWork = 13,
        [EnumValue("מושלם")]
        Perfect = 14,
    }

    public partial class Configuration : IEntityTypeConfiguration<Maamar>
    {
        public void Configure(EntityTypeBuilder<Maamar> builder)
        {
            //builder.ToTable("Maamarim");

            builder.HasQueryFilter(x => x.Status != MaamarimStatus.Deleted);

            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Parsha);
            builder.HasIndex(x => x.Year);
            builder.HasIndex(x => x.Date);
            builder.HasIndex(x => x.Title);
        }
    }
}
