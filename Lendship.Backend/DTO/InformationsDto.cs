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
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lendship.Backend.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class InformationsDto : IEquatable<InformationsDto>
    {/// <summary>
     /// Gets CompanyName
     /// </summary>        
        [DataMember(Name = "companyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets Address
        /// </summary>        
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets Phone
        /// </summary>        
        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets Email
        /// </summary>        
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets Description
        /// </summary>        
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Advertisement {\n");
            sb.Append("  CompanyName: ").Append(CompanyName).Append("\n");
            sb.Append("  Address: ").Append(Address).Append("\n");
            sb.Append("  Phone: ").Append(Phone).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
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
            return obj.GetType() == GetType() && Equals((InformationsDto)obj);
        }

        /// <summary>
        /// Returns true if Advertisement instances are equal
        /// </summary>
        /// <param name="other">Instance of Advertisement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(InformationsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    CompanyName == other.CompanyName ||
                    CompanyName != null &&
                    CompanyName.Equals(other.CompanyName)
                ) && 
                (
                    Address == other.Address ||
                    Address != null &&
                    Address.Equals(other.Address)
                ) && 
                (
                    Phone == other.Phone ||
                    Phone != null &&
                    Phone.Equals(other.Phone)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) ;
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
                    if (CompanyName != null)
                    hashCode = hashCode * 59 + CompanyName.GetHashCode();
                    if (Address != null)
                    hashCode = hashCode * 59 + Address.GetHashCode();
                    if (Phone != null)
                    hashCode = hashCode * 59 + Phone.GetHashCode();
                    if (Email != null)
                    hashCode = hashCode * 59 + Email.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(InformationsDto left, InformationsDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InformationsDto left, InformationsDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
