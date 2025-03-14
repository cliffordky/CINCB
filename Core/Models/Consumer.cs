using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{




    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Consumer
    {

        private ConsumerDetails detailsField;

        private ConsumerFraudIndicatorsSummary fraudIndicatorsSummaryField;

        private ConsumerConsumerAccountSummary consumerAccountSummaryField;

        private ConsumerPropertyInfoSummary propertyInfoSummaryField;

        private ConsumerDirectorSummary directorSummaryField;

        private ConsumerMonthlyAccountPayment[] monthlyAccountPaymentField;

        private ConsumerMonthlyAccountPaymentDefinition[] monthlyAccountPaymentDefinitionField;

        private ConsumerFraudIndicatorsDetail fraudIndicatorsDetailField;

        private ConsumerPropertyInfoDetail propertyInfoDetailField;

        private ConsumerNameHistory nameHistoryField;

        private ConsumerAddressHistory[] addressHistoryField;

        private ConsumerTelephoneHistory[] telephoneHistoryField;

        private ConsumerEmploymentHistory[] employmentHistoryField;

        private string matchResultField;

        /// <remarks/>
        public ConsumerDetails Details
        {
            get
            {
                return this.detailsField;
            }
            set
            {
                this.detailsField = value;
            }
        }

        /// <remarks/>
        public ConsumerFraudIndicatorsSummary FraudIndicatorsSummary
        {
            get
            {
                return this.fraudIndicatorsSummaryField;
            }
            set
            {
                this.fraudIndicatorsSummaryField = value;
            }
        }

        /// <remarks/>
        public ConsumerConsumerAccountSummary ConsumerAccountSummary
        {
            get
            {
                return this.consumerAccountSummaryField;
            }
            set
            {
                this.consumerAccountSummaryField = value;
            }
        }

        /// <remarks/>
        public ConsumerPropertyInfoSummary PropertyInfoSummary
        {
            get
            {
                return this.propertyInfoSummaryField;
            }
            set
            {
                this.propertyInfoSummaryField = value;
            }
        }

        /// <remarks/>
        public ConsumerDirectorSummary DirectorSummary
        {
            get
            {
                return this.directorSummaryField;
            }
            set
            {
                this.directorSummaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MonthlyAccountPayment")]
        public ConsumerMonthlyAccountPayment[] MonthlyAccountPayment
        {
            get
            {
                return this.monthlyAccountPaymentField;
            }
            set
            {
                this.monthlyAccountPaymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MonthlyAccountPaymentDefinition")]
        public ConsumerMonthlyAccountPaymentDefinition[] MonthlyAccountPaymentDefinition
        {
            get
            {
                return this.monthlyAccountPaymentDefinitionField;
            }
            set
            {
                this.monthlyAccountPaymentDefinitionField = value;
            }
        }

        /// <remarks/>
        public ConsumerFraudIndicatorsDetail FraudIndicatorsDetail
        {
            get
            {
                return this.fraudIndicatorsDetailField;
            }
            set
            {
                this.fraudIndicatorsDetailField = value;
            }
        }

        /// <remarks/>
        public ConsumerPropertyInfoDetail PropertyInfoDetail
        {
            get
            {
                return this.propertyInfoDetailField;
            }
            set
            {
                this.propertyInfoDetailField = value;
            }
        }

        /// <remarks/>
        public ConsumerNameHistory NameHistory
        {
            get
            {
                return this.nameHistoryField;
            }
            set
            {
                this.nameHistoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AddressHistory")]
        public ConsumerAddressHistory[] AddressHistory
        {
            get
            {
                return this.addressHistoryField;
            }
            set
            {
                this.addressHistoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TelephoneHistory")]
        public ConsumerTelephoneHistory[] TelephoneHistory
        {
            get
            {
                return this.telephoneHistoryField;
            }
            set
            {
                this.telephoneHistoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EmploymentHistory")]
        public ConsumerEmploymentHistory[] EmploymentHistory
        {
            get
            {
                return this.employmentHistoryField;
            }
            set
            {
                this.employmentHistoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MatchResult
        {
            get
            {
                return this.matchResultField;
            }
            set
            {
                this.matchResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerDetails
    {

        private string initialsField;

        private string firstNameField;

        private string secondNameField;

        private string thirdNameField;

        private string surnameField;

        private ulong iDNoField;

        private string passportNoField;

        private System.DateTime birthDateField;

        private string genderField;

        private string titleDescField;

        private string maritalStatusDescField;

        private string spouseFirstNameField;

        private string spouseSurnameField;

        private string residentialAddressField;

        private string postalAddressField;

        private uint homeTelephoneNoField;

        private uint workTelephoneNoField;

        private uint cellularNoField;

        private string emailAddressField;

        private string employerDetailField;

        private string referenceNoField;

        private string externalReferenceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Initials
        {
            get
            {
                return this.initialsField;
            }
            set
            {
                this.initialsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ThirdName
        {
            get
            {
                return this.thirdNameField;
            }
            set
            {
                this.thirdNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong IDNo
        {
            get
            {
                return this.iDNoField;
            }
            set
            {
                this.iDNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PassportNo
        {
            get
            {
                return this.passportNoField;
            }
            set
            {
                this.passportNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime BirthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TitleDesc
        {
            get
            {
                return this.titleDescField;
            }
            set
            {
                this.titleDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string MaritalStatusDesc
        {
            get
            {
                return this.maritalStatusDescField;
            }
            set
            {
                this.maritalStatusDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SpouseFirstName
        {
            get
            {
                return this.spouseFirstNameField;
            }
            set
            {
                this.spouseFirstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SpouseSurname
        {
            get
            {
                return this.spouseSurnameField;
            }
            set
            {
                this.spouseSurnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ResidentialAddress
        {
            get
            {
                return this.residentialAddressField;
            }
            set
            {
                this.residentialAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PostalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint HomeTelephoneNo
        {
            get
            {
                return this.homeTelephoneNoField;
            }
            set
            {
                this.homeTelephoneNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint WorkTelephoneNo
        {
            get
            {
                return this.workTelephoneNoField;
            }
            set
            {
                this.workTelephoneNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint CellularNo
        {
            get
            {
                return this.cellularNoField;
            }
            set
            {
                this.cellularNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EmployerDetail
        {
            get
            {
                return this.employerDetailField;
            }
            set
            {
                this.employerDetailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ReferenceNo
        {
            get
            {
                return this.referenceNoField;
            }
            set
            {
                this.referenceNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ExternalReference
        {
            get
            {
                return this.externalReferenceField;
            }
            set
            {
                this.externalReferenceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerFraudIndicatorsSummary
    {

        private string homeAffairsVerificationField;

        private System.DateTime homeAffairsDeceasedDateField;

        private string sAFPSListingField;

        private string xDSAuthenticationListingField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HomeAffairsVerification
        {
            get
            {
                return this.homeAffairsVerificationField;
            }
            set
            {
                this.homeAffairsVerificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime HomeAffairsDeceasedDate
        {
            get
            {
                return this.homeAffairsDeceasedDateField;
            }
            set
            {
                this.homeAffairsDeceasedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SAFPSListing
        {
            get
            {
                return this.sAFPSListingField;
            }
            set
            {
                this.sAFPSListingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XDSAuthenticationListing
        {
            get
            {
                return this.xDSAuthenticationListingField;
            }
            set
            {
                this.xDSAuthenticationListingField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerConsumerAccountSummary
    {

        private decimal totalMontlyInstallmentAmtField;

        private decimal totalOutstandingDebtAmtField;

        private byte totalNoOfCreditAccountsField;

        private byte noOfBankAccountsField;

        private byte noOfApparelAccountsField;

        private byte noOfFurnitureAccountsField;

        private byte noOfCellularAccountsField;

        private byte noOfAdverseAccountsField;

        private decimal totalAdverseAmtField;

        private byte noOfAccountInArrearsField;

        private decimal totalArrearsAmtField;

        private byte monthsInArrearsField;

        private byte noOfJudgementsField;

        private decimal totalJudgementAmtField;

        private System.DateTime latestJudgementDateField;

        private string adminOrderYNField;

        private string debtReviewStatusDescField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalMontlyInstallmentAmt
        {
            get
            {
                return this.totalMontlyInstallmentAmtField;
            }
            set
            {
                this.totalMontlyInstallmentAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalOutstandingDebtAmt
        {
            get
            {
                return this.totalOutstandingDebtAmtField;
            }
            set
            {
                this.totalOutstandingDebtAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte TotalNoOfCreditAccounts
        {
            get
            {
                return this.totalNoOfCreditAccountsField;
            }
            set
            {
                this.totalNoOfCreditAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfBankAccounts
        {
            get
            {
                return this.noOfBankAccountsField;
            }
            set
            {
                this.noOfBankAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfApparelAccounts
        {
            get
            {
                return this.noOfApparelAccountsField;
            }
            set
            {
                this.noOfApparelAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfFurnitureAccounts
        {
            get
            {
                return this.noOfFurnitureAccountsField;
            }
            set
            {
                this.noOfFurnitureAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfCellularAccounts
        {
            get
            {
                return this.noOfCellularAccountsField;
            }
            set
            {
                this.noOfCellularAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfAdverseAccounts
        {
            get
            {
                return this.noOfAdverseAccountsField;
            }
            set
            {
                this.noOfAdverseAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalAdverseAmt
        {
            get
            {
                return this.totalAdverseAmtField;
            }
            set
            {
                this.totalAdverseAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfAccountInArrears
        {
            get
            {
                return this.noOfAccountInArrearsField;
            }
            set
            {
                this.noOfAccountInArrearsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalArrearsAmt
        {
            get
            {
                return this.totalArrearsAmtField;
            }
            set
            {
                this.totalArrearsAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte MonthsInArrears
        {
            get
            {
                return this.monthsInArrearsField;
            }
            set
            {
                this.monthsInArrearsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte NoOfJudgements
        {
            get
            {
                return this.noOfJudgementsField;
            }
            set
            {
                this.noOfJudgementsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalJudgementAmt
        {
            get
            {
                return this.totalJudgementAmtField;
            }
            set
            {
                this.totalJudgementAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime LatestJudgementDate
        {
            get
            {
                return this.latestJudgementDateField;
            }
            set
            {
                this.latestJudgementDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AdminOrderYN
        {
            get
            {
                return this.adminOrderYNField;
            }
            set
            {
                this.adminOrderYNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DebtReviewStatusDesc
        {
            get
            {
                return this.debtReviewStatusDescField;
            }
            set
            {
                this.debtReviewStatusDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerPropertyInfoSummary
    {

        private byte totalPropertyCountField;

        private decimal totalPropertyValueAmtField;

        private byte currentPropertyInterestsField;

        private byte previousPropertyInterestsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte TotalPropertyCount
        {
            get
            {
                return this.totalPropertyCountField;
            }
            set
            {
                this.totalPropertyCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal TotalPropertyValueAmt
        {
            get
            {
                return this.totalPropertyValueAmtField;
            }
            set
            {
                this.totalPropertyValueAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte CurrentPropertyInterests
        {
            get
            {
                return this.currentPropertyInterestsField;
            }
            set
            {
                this.currentPropertyInterestsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte PreviousPropertyInterests
        {
            get
            {
                return this.previousPropertyInterestsField;
            }
            set
            {
                this.previousPropertyInterestsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerDirectorSummary
    {

        private byte totalNoOfDirectorshipsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte TotalNoOfDirectorships
        {
            get
            {
                return this.totalNoOfDirectorshipsField;
            }
            set
            {
                this.totalNoOfDirectorshipsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerMonthlyAccountPayment
    {

        private System.DateTime accountOpenedDateField;

        private string subscriberNameField;

        private decimal creditLimitAmtField;

        private decimal currentBalanceAmtField;

        private decimal monthlyInstalmentAmtField;

        private decimal arrearsAmtField;

        private string arrearsTypeIndField;

        private string accountTypeField;

        private string accountNoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime AccountOpenedDate
        {
            get
            {
                return this.accountOpenedDateField;
            }
            set
            {
                this.accountOpenedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SubscriberName
        {
            get
            {
                return this.subscriberNameField;
            }
            set
            {
                this.subscriberNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal CreditLimitAmt
        {
            get
            {
                return this.creditLimitAmtField;
            }
            set
            {
                this.creditLimitAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal CurrentBalanceAmt
        {
            get
            {
                return this.currentBalanceAmtField;
            }
            set
            {
                this.currentBalanceAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal MonthlyInstalmentAmt
        {
            get
            {
                return this.monthlyInstalmentAmtField;
            }
            set
            {
                this.monthlyInstalmentAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal ArrearsAmt
        {
            get
            {
                return this.arrearsAmtField;
            }
            set
            {
                this.arrearsAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ArrearsTypeInd
        {
            get
            {
                return this.arrearsTypeIndField;
            }
            set
            {
                this.arrearsTypeIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AccountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AccountNo
        {
            get
            {
                return this.accountNoField;
            }
            set
            {
                this.accountNoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerMonthlyAccountPaymentDefinition
    {

        private string definitionCodeField;

        private string definitionDescField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DefinitionCode
        {
            get
            {
                return this.definitionCodeField;
            }
            set
            {
                this.definitionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DefinitionDesc
        {
            get
            {
                return this.definitionDescField;
            }
            set
            {
                this.definitionDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerFraudIndicatorsDetail
    {

        private string xDSAuthenticationListingField;

        private System.DateTime xDSAuthenticationDateField;

        private string xDSAuthenticationResultField;

        private string xDSAuthenticationReasonField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XDSAuthenticationListing
        {
            get
            {
                return this.xDSAuthenticationListingField;
            }
            set
            {
                this.xDSAuthenticationListingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime XDSAuthenticationDate
        {
            get
            {
                return this.xDSAuthenticationDateField;
            }
            set
            {
                this.xDSAuthenticationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XDSAuthenticationResult
        {
            get
            {
                return this.xDSAuthenticationResultField;
            }
            set
            {
                this.xDSAuthenticationResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string XDSAuthenticationReason
        {
            get
            {
                return this.xDSAuthenticationReasonField;
            }
            set
            {
                this.xDSAuthenticationReasonField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerPropertyInfoDetail
    {

        private string titleDeedNoField;

        private string erfNoField;

        private string deedsOfficeField;

        private string physicalAddressField;

        private string propertyTypeDescField;

        private string erfSizeField;

        private System.DateTime purchaseDateField;

        private decimal purchasePriceAmtField;

        private decimal buyerSharePercField;

        private string bondHolderNameField;

        private string bondAccountNoField;

        private decimal bondAmtField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TitleDeedNo
        {
            get
            {
                return this.titleDeedNoField;
            }
            set
            {
                this.titleDeedNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ErfNo
        {
            get
            {
                return this.erfNoField;
            }
            set
            {
                this.erfNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DeedsOffice
        {
            get
            {
                return this.deedsOfficeField;
            }
            set
            {
                this.deedsOfficeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PhysicalAddress
        {
            get
            {
                return this.physicalAddressField;
            }
            set
            {
                this.physicalAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PropertyTypeDesc
        {
            get
            {
                return this.propertyTypeDescField;
            }
            set
            {
                this.propertyTypeDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ErfSize
        {
            get
            {
                return this.erfSizeField;
            }
            set
            {
                this.erfSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime PurchaseDate
        {
            get
            {
                return this.purchaseDateField;
            }
            set
            {
                this.purchaseDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal PurchasePriceAmt
        {
            get
            {
                return this.purchasePriceAmtField;
            }
            set
            {
                this.purchasePriceAmtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal BuyerSharePerc
        {
            get
            {
                return this.buyerSharePercField;
            }
            set
            {
                this.buyerSharePercField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BondHolderName
        {
            get
            {
                return this.bondHolderNameField;
            }
            set
            {
                this.bondHolderNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BondAccountNo
        {
            get
            {
                return this.bondAccountNoField;
            }
            set
            {
                this.bondAccountNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal BondAmt
        {
            get
            {
                return this.bondAmtField;
            }
            set
            {
                this.bondAmtField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerNameHistory
    {

        private string initialsField;

        private string firstNameField;

        private string secondNameField;

        private string surnameField;

        private ulong iDNoField;

        private string passportNoField;

        private System.DateTime birthDateField;

        private string titleDescField;

        private System.DateTime lastUpdatedDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Initials
        {
            get
            {
                return this.initialsField;
            }
            set
            {
                this.initialsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SecondName
        {
            get
            {
                return this.secondNameField;
            }
            set
            {
                this.secondNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong IDNo
        {
            get
            {
                return this.iDNoField;
            }
            set
            {
                this.iDNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PassportNo
        {
            get
            {
                return this.passportNoField;
            }
            set
            {
                this.passportNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime BirthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TitleDesc
        {
            get
            {
                return this.titleDescField;
            }
            set
            {
                this.titleDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime LastUpdatedDate
        {
            get
            {
                return this.lastUpdatedDateField;
            }
            set
            {
                this.lastUpdatedDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerAddressHistory
    {

        private string addressTypeField;

        private string address1Field;

        private string address2Field;

        private string address3Field;

        private string address4Field;

        private string postalCodeField;

        private System.DateTime lastUpdatedDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AddressType
        {
            get
            {
                return this.addressTypeField;
            }
            set
            {
                this.addressTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Address3
        {
            get
            {
                return this.address3Field;
            }
            set
            {
                this.address3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Address4
        {
            get
            {
                return this.address4Field;
            }
            set
            {
                this.address4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string PostalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime LastUpdatedDate
        {
            get
            {
                return this.lastUpdatedDateField;
            }
            set
            {
                this.lastUpdatedDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerTelephoneHistory
    {

        private string telephoneTypeField;

        private uint telephoneNoField;

        private string emailAddressField;

        private System.DateTime lastUpdatedDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string TelephoneType
        {
            get
            {
                return this.telephoneTypeField;
            }
            set
            {
                this.telephoneTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint TelephoneNo
        {
            get
            {
                return this.telephoneNoField;
            }
            set
            {
                this.telephoneNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EmailAddress
        {
            get
            {
                return this.emailAddressField;
            }
            set
            {
                this.emailAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime LastUpdatedDate
        {
            get
            {
                return this.lastUpdatedDateField;
            }
            set
            {
                this.lastUpdatedDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ConsumerEmploymentHistory
    {

        private string employerDetailField;

        private string designationField;

        private System.DateTime lastUpdatedDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string EmployerDetail
        {
            get
            {
                return this.employerDetailField;
            }
            set
            {
                this.employerDetailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Designation
        {
            get
            {
                return this.designationField;
            }
            set
            {
                this.designationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime LastUpdatedDate
        {
            get
            {
                return this.lastUpdatedDateField;
            }
            set
            {
                this.lastUpdatedDateField = value;
            }
        }
    }


}
