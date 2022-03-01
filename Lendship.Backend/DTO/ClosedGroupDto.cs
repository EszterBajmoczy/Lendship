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
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lendship.Backend.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ClosedGroupDto : IEquatable<ClosedGroupDto>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets AdvertisementId
        /// </summary>
        [Required]
        
        [DataMember(Name="advertisementId")]
        public int AdvertisementId { get; set; }

        /// <summary>
        /// Gets or Sets Users
        /// </summary>
        [Required]
        
        [DataMember(Name="users")]
        public List<UserDto> Users { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ClosedGroup {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  AdvertisementId: ").Append(AdvertisementId).Append("\n");
            sb.Append("  Users: ").Append(Users).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ClosedGroupDto)obj);
        }

        /// <summary>
        /// Returns true if ClosedGroup instances are equal
        /// </summary>
        /// <param name="other">Instance of ClosedGroup to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ClosedGroupDto other)
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
                    AdvertisementId == other.AdvertisementId ||
                    AdvertisementId != null &&
                    AdvertisementId.Equals(other.AdvertisementId)
                ) && 
                (
                    Users == other.Users ||
                    Users != null &&
                    Users.SequenceEqual(other.Users)
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
                    if (AdvertisementId != null)
                    hashCode = hashCode * 59 + AdvertisementId.GetHashCode();
                    if (Users != null)
                    hashCode = hashCode * 59 + Users.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(ClosedGroupDto left, ClosedGroupDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ClosedGroupDto left, ClosedGroupDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
