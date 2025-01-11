import { StatusId } from './models'

export function statusColor(status: string) {
  switch (status) {
    case StatusId.Applied:
      return 'text-blue-500'
    case StatusId.Rejected:
      return 'text-red-500'
    case StatusId.Interviewing:
      return 'text-orange-900'
    case StatusId.NoOffer:
      return 'text-orange-600'
    case StatusId.Offer:
      return 'text-violet-500'
    case StatusId.Accepted:
      return 'text-green-600'
    case StatusId.PhoneScreen:
      return 'text-fuchsia-500'
    default:
      return 'text-gray-400'
  }
}
