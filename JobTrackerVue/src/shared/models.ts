// API models
export interface ApplicationModel {
  id: string
  jobTitle: string
  company: string
  recruiter: string
  phone: string
  email: string
  jobListingUrl: string
  jobDescription: string
  statusHistory: StatusModel[]
}

export interface AddApplicationModel {
  jobTitle: string
  company: string
  recruiter: string
  phone: string
  email: string
  jobListingUrl: string
  jobDescription: string
  applicationDate: string
  userId: string
}

export interface StatusModel {
  status: string
  date: string
}

export interface StatusReportModel {
  applied: number
  rejected: number
  screened: number
  interviewing: number
  noOffer: number
  offer: number
  accepted: number
  success: boolean
  total: number
}

// Status select dropdown
export interface StatusSelectModel {
  Value: string
  Text: string
}

export class StatusId {
  static readonly Applied: string = 'applied'
  static readonly Rejected: string = 'rejected'
  static readonly Interviewing: string = 'interview'
  static readonly NoOffer: string = 'nooffer'
  static readonly Offer: string = 'offered'
  static readonly Accepted: string = 'accepted'
  static readonly PhoneScreen: string = 'screen'
}

export const Statuses: StatusSelectModel[] = [
  { Value: StatusId.Applied, Text: 'Applied' }, //0
  { Value: StatusId.Rejected, Text: 'Rejected' }, //1
  { Value: StatusId.PhoneScreen, Text: 'Phone Screen' }, //2
  { Value: StatusId.Interviewing, Text: 'Interviewing' }, //3
  { Value: StatusId.NoOffer, Text: 'No Offer' }, //4
  { Value: StatusId.Offer, Text: 'Offer Made' }, //5
  { Value: StatusId.Accepted, Text: 'Offer Accepted' }, //6
]

export interface FilterUpdatePayload {
  dateRange: [Date, Date] | null
  status: string
  search: string
}
