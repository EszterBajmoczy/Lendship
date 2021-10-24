/*
 * Simple Inventory API
 *
 * This is a simple API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: you@your-company.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lendship.Backend.DTO
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class EvaluationAdvertiserDto : IEquatable<EvaluationAdvertiserDto>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets UserFrom
        /// </summary>
        [Required]
        
        [DataMember(Name="userFrom")]
        public UserDto UserFrom { get; set; }

        /// <summary>
        /// Gets or Sets UserTo
        /// </summary>
        [Required]
        
        [DataMember(Name="userTo")]
        public UserDto UserTo { get; set; }

        /// <summary>
        /// Gets or Sets AdvertisementId
        /// </summary>

        [DataMember(Name="advertisementId")]
        public int AdvertisementId { get; set; }

        /// <summary>
        /// Gets or Sets Flexibility
        /// </summary>
        [Required]
        
        [DataMember(Name="flexibility")]
        public int Flexibility { get; set; }

        /// <summary>
        /// Gets or Sets Reliability
        /// </summary>
        [Required]
        
        [DataMember(Name="reliability")]
        public int Reliability { get; set; }

        /// <summary>
        /// Gets or Sets QualityOfProduct
        /// </summary>
        [Required]
        
        [DataMember(Name="qualityOfProduct")]
        public int QualityOfProduct { get; set; }

        /// <summary>
        /// Gets or Sets Comment
        /// </summary>

        [DataMember(Name="comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or Sets Anonymous
        /// </summary>
        [Required]

        [DataMember(Name="isAnonymous")]
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or Sets Creation
        /// </summary>

        [DataMember(Name="creation")]
        public DateTime? Creation { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class EvaluationAdvertiser {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  UserFrom: ").Append(UserFrom).Append("\n");
            sb.Append("  UserTo: ").Append(UserTo).Append("\n");
            sb.Append("  AdvertisementId: ").Append(AdvertisementId).Append("\n");
            sb.Append("  Flexibility: ").Append(Flexibility).Append("\n");
            sb.Append("  Reliability: ").Append(Reliability).Append("\n");
            sb.Append("  QualityOfProduct: ").Append(QualityOfProduct).Append("\n");
            sb.Append("  Comment: ").Append(Comment).Append("\n");
            sb.Append("  Anonymous: ").Append(IsAnonymous).Append("\n");
            sb.Append("  Creation: ").Append(Creation).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EvaluationAdvertiserDto)obj);
        }

        /// <summary>
        /// Returns true if EvaluationAdvertiser instances are equal
        /// </summary>
        /// <param name="other">Instance of EvaluationAdvertiser to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(EvaluationAdvertiserDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) && 
                (
                    UserFrom == other.UserFrom ||
                    UserFrom != null &&
                    UserFrom.Equals(other.UserFrom)
                ) && 
                (
                    UserTo == other.UserTo ||
                    UserTo != null &&
                    UserTo.Equals(other.UserTo)
                ) && 
                (
                    AdvertisementId == other.AdvertisementId ||
                    AdvertisementId != null &&
                    AdvertisementId.Equals(other.AdvertisementId)
                ) && 
                (
                    Flexibility == other.Flexibility ||
                    Flexibility != null &&
                    Flexibility.Equals(other.Flexibility)
                ) && 
                (
                    Reliability == other.Reliability ||
                    Reliability != null &&
                    Reliability.Equals(other.Reliability)
                ) && 
                (
                    QualityOfProduct == other.QualityOfProduct ||
                    QualityOfProduct != null &&
                    QualityOfProduct.Equals(other.QualityOfProduct)
                ) && 
                (
                    Comment == other.Comment ||
                    Comment != null &&
                    Comment.Equals(other.Comment)
                ) && 
                (
                    IsAnonymous == other.IsAnonymous ||
                    IsAnonymous != null &&
                    IsAnonymous.Equals(other.IsAnonymous)
                ) && 
                (
                    Creation == other.Creation ||
                    Creation != null &&
                    Creation.Equals(other.Creation)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    if (UserFrom != null)
                    hashCode = hashCode * 59 + UserFrom.GetHashCode();
                    if (UserTo != null)
                    hashCode = hashCode * 59 + UserTo.GetHashCode();
                    if (AdvertisementId != null)
                    hashCode = hashCode * 59 + AdvertisementId.GetHashCode();
                    if (Flexibility != null)
                    hashCode = hashCode * 59 + Flexibility.GetHashCode();
                    if (Reliability != null)
                    hashCode = hashCode * 59 + Reliability.GetHashCode();
                    if (QualityOfProduct != null)
                    hashCode = hashCode * 59 + QualityOfProduct.GetHashCode();
                    if (Comment != null)
                    hashCode = hashCode * 59 + Comment.GetHashCode();
                    if (IsAnonymous != null)
                    hashCode = hashCode * 59 + IsAnonymous.GetHashCode();
                    if (Creation != null)
                    hashCode = hashCode * 59 + Creation.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(EvaluationAdvertiserDto left, EvaluationAdvertiserDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EvaluationAdvertiserDto left, EvaluationAdvertiserDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
