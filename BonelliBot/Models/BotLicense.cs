using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using QLicense;

namespace BonelliBot.Models
{
    public class BotLicense : LicenseEntity
    {
        public BotLicense()
        {
            this.AppName = "BonelliBot";
        }

        [DisplayName("فالو/آنفالو")]
        [Category("License Options")]
        [XmlElement("EnableFollow/Unfollow")]
        [ShowInLicenseInfo(true, "فالو/آنفالو", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool EnableFeature01 { get; set; }

        [DisplayName("تاریخ انقضا")]
        [Category("License Options")]
        [XmlElement("ExpirationDate")]
        [ShowInLicenseInfo(true, "تاریخ انقضا", ShowInLicenseInfoAttribute.FormatType.DateTime)]
        public DateTime ExpiriedDate { get; set; }

        [DisplayName("اکانت اول")]
        [Category("License Options")]
        [XmlElement("FirstAccount")]
        [ShowInLicenseInfo(true, "اکانت اول", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool FirstAccount { get; set; }

        [DisplayName("اکانت دوم")]
        [Category("License Options")]
        [XmlElement("SecondAccount")]
        [ShowInLicenseInfo(true, "اکانت دوم", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool SecondAccount { get; set; }

        [DisplayName("اکانت سوم")]
        [Category("License Options")]
        [XmlElement("ThirdAccount")]
        [ShowInLicenseInfo(true, "اکانت سوم", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool ThirdAccount { get; set; }

        [DisplayName("اکانت چهارم")]
        [Category("License Options")]
        [XmlElement("ForthAccount")]
        [ShowInLicenseInfo(true, "اکانت چهارم", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool ForthAccount { get; set; }

        [DisplayName("اکانت پنجم")]
        [Category("License Options")]
        [XmlElement("FifthAccount")]
        [ShowInLicenseInfo(true, "اکانت پنجم", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool FifthAccount { get; set; }

        [DisplayName("آنفالو غیر فالو بک ها")]
        [Category("License Options")]
        [XmlElement("UnfollowNonfollowers")]
        [ShowInLicenseInfo(true, "آنفالو غیر فالو بک ها", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool UnfollowNonfollowers { get; set; }

        [DisplayName("فالو بر اساس جنسیت")]
        [Category("License Options")]
        [XmlElement("FollowBaseOnGender")]
        [ShowInLicenseInfo(true, "فالو بر اساس جنسیت", ShowInLicenseInfoAttribute.FormatType.String)]
        public bool FollowBaseOnGender { get; set; }

        public override LicenseStatus DoExtraValidation(out string validationMsg)
        {
            LicenseStatus _licStatus = LicenseStatus.UNDEFINED;
            validationMsg = string.Empty;

            switch (this.Type)
            {
                case LicenseTypes.Single:
                    //For Single License, check whether UID is matched
                    if (this.UID == LicenseHandler.GenerateUID(this.AppName))
                    {
                        _licStatus = LicenseStatus.VALID;
                    }
                    else
                    {
                        validationMsg = "The license is NOT for this copy!";
                        _licStatus = LicenseStatus.INVALID;
                    }
                    break;
                case LicenseTypes.Volume:
                    //No UID checking for Volume License
                    _licStatus = LicenseStatus.VALID;
                    break;
                default:
                    validationMsg = "Invalid license";
                    _licStatus = LicenseStatus.INVALID;
                    break;
            }

            return _licStatus;
        }
    }
}
