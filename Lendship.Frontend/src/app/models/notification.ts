export interface INotification {
  id: number,
  advertisementId: number,
  advertisementTitle: string,
  reservationDateFrom: Date,
  reservationDateFromString: string,
  reservationDateTo: Date,
  reservationDateToString: string,
  reservationId: number,
  updateInformation: string,
  new: boolean
}
