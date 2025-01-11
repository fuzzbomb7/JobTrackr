import type { AddApplicationModel } from './models'

const API_ROOT: string = 'https://jobtrackrapi.azurewebsites.net'

export async function getApplications(): Promise<Response> {
  return fetch(`${API_ROOT}/Applications`)
}

export async function postApplication(application: AddApplicationModel): Promise<Response> {
  return fetch(`${API_ROOT}/Application`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(application),
  })
}

export async function patchApplication(
  id: number,
  newStatus: string,
  newStatusDate?: string,
): Promise<Response> {
  const query = newStatusDate !== undefined ? `?date=${newStatusDate}` : ''
  return fetch(`${API_ROOT}/Application/${id}/${newStatus}${query}`, {
    method: 'PATCH',
  })
}

export async function deleteApplication(id: number): Promise<Response> {
  return fetch(`${API_ROOT}/Application/${id}`, {
    method: 'DELETE',
  })
}

export async function getReport(): Promise<Response> {
  return fetch(`${API_ROOT}/Report`)
}
