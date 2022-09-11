using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Lendship.Backend.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class NotificationDTO
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        [DataMember(Name = "id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [Required]
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or Sets AdvertisementId
        /// </summary>
        [Required]
        [DataMember(Name = "advertisementId")]
        public int AdvertisementId { get; set; }

        /// <summary>
        /// Gets or Sets AdvertisementTitle
        /// </summary>
        [Required]
        [DataMember(Name = "advertisementTitle")]
        public string AdvertisementTitle { get; set; }

        /// <summary>
        /// Gets or Sets ReservationDateFrom
        /// </summary>
        [Required]

        [DataMember(Name = "reservationDateFrom")]
        public DateTime? ReservationDateFrom { get; set; }

        /// <summary>
        /// Gets or Sets ReservationDateTo
        /// </summary>
        [Required]

        [DataMember(Name = "reservationDateTo")]
        public DateTime? ReservationDateTo { get; set; }

        /// <summary>
        /// Gets or Sets ReservationId
        /// </summary>
        [Required]
        [DataMember(Name = "reservationId")]
        public int ReservationId { get; set; }

        /// <summary>
        /// Gets or Sets UpdateInformation
        /// </summary>
        [Required]
        [DataMember(Name = "updateInformation")]
        public string UpdateInformation { get; set; }


        /// <summary>
        /// Gets or Sets New
        /// </summary>
        [Required]
        [DataMember(Name = "new")]
        public bool New { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Notification {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  UserId: ").Append(UserId).Append("\n");
            sb.Append("  AdvertisementId: ").Append(AdvertisementId).Append("\n");
            sb.Append("  AdvertisementTitle: ").Append(AdvertisementTitle).Append("\n");
            sb.Append("  ReservationDateFrom: ").Append(ReservationDateFrom).Append("\n");
            sb.Append("  ReservationDateTo: ").Append(ReservationDateTo).Append("\n");
            sb.Append("  ReservationId: ").Append(ReservationId).Append("\n");
            sb.Append("  UpdateInformation: ").Append(UpdateInformation).Append("\n");
            sb.Append("  New: ").Append(New).Append("\n");
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
            return obj.GetType() == GetType() && Equals((NotificationDTO)obj);
        }

        /// <summary>
        /// Returns true if Advertisement instances are equal
        /// </summary>
        /// <param name="other">Instance of Advertisement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NotificationDTO other)
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
                    UserId == other.UserId ||
                    UserId != null &&
                    UserId.Equals(other.UserId)
                ) &&
                (
                    AdvertisementId == other.AdvertisementId ||
                    AdvertisementId != null &&
                    AdvertisementId.Equals(other.AdvertisementId)
                ) &&
                (
                    AdvertisementTitle == other.AdvertisementTitle ||
                    AdvertisementTitle != null &&
                    AdvertisementTitle.Equals(other.AdvertisementTitle)
                ) &&
                (
                    ReservationDateFrom == other.ReservationDateFrom ||
                    ReservationDateFrom != null &&
                    ReservationDateFrom.Equals(other.ReservationDateFrom)
                ) &&
                (
                    ReservationDateTo == other.ReservationDateTo ||
                    ReservationDateTo != null &&
                    ReservationDateTo.Equals(other.ReservationDateTo)
                ) &&
                (
                    ReservationId == other.ReservationId ||
                    ReservationId != null &&
                    ReservationId.Equals(other.ReservationId)
                ) &&
                (
                    UpdateInformation == other.UpdateInformation ||
                    UpdateInformation != null &&
                    UpdateInformation.Equals(other.UpdateInformation)
                ) &&
                (
                    New == other.New ||
                    New != null &&
                    New.Equals(other.New)
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
                if (UserId != null)
                    hashCode = hashCode * 59 + UserId.GetHashCode();
                if (AdvertisementId != null)
                    hashCode = hashCode * 59 + AdvertisementId.GetHashCode();
                if (AdvertisementTitle != null)
                    hashCode = hashCode * 59 + AdvertisementTitle.GetHashCode();
                if (ReservationDateFrom != null)
                    hashCode = hashCode * 59 + ReservationId.GetHashCode();
                if (ReservationDateTo != null)
                    hashCode = hashCode * 59 + ReservationId.GetHashCode();
                if (ReservationId != null)
                    hashCode = hashCode * 59 + ReservationId.GetHashCode();
                if (UpdateInformation != null)
                    hashCode = hashCode * 59 + UpdateInformation.GetHashCode();
                if (New != null)
                    hashCode = hashCode * 59 + New.GetHashCode();
                return hashCode;
            }
        }
    }
}
