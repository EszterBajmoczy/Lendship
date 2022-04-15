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
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using static Lendship.Backend.DTO.ReservationDetailDto;

namespace Lendship.Backend.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ReservationDto : IEquatable<ReservationDto>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets ReservationState
        /// </summary>
        [Required]
        
        [DataMember(Name="reservationState")]
        public ReservationStateEnum? ReservationState { get; set; }
                
        [DataMember(Name="dateFrom")]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Gets or Sets DateTo
        /// </summary>
        [Required]
        
        [DataMember(Name="dateTo")]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Reservation {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  DateFrom: ").Append(DateFrom).Append("\n");
            sb.Append("  DateTo: ").Append(DateTo).Append("\n");
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
            return obj.GetType() == GetType() && Equals((ReservationDto)obj);
        }

        /// <summary>
        /// Returns true if Reservation instances are equal
        /// </summary>
        /// <param name="other">Instance of Reservation to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ReservationDto other)
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
                    DateFrom == other.DateFrom ||
                    DateFrom != null &&
                    DateFrom.Equals(other.DateFrom)
                ) && 
                (
                    DateTo == other.DateTo ||
                    DateTo != null &&
                    DateTo.Equals(other.DateTo)
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
                    if (DateFrom != null)
                    hashCode = hashCode * 59 + DateFrom.GetHashCode();
                    if (DateTo != null)
                    hashCode = hashCode * 59 + DateTo.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(ReservationDto left, ReservationDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ReservationDto left, ReservationDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
