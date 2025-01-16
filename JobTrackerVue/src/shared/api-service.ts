import type { AddApplicationModel } from './models'

const API_ROOT: string = 'https://jobtrackrapi.azurewebsites.net' // 'https://localhost:7048'

export async function getApplications(token: string): Promise<Response> {
  return fetch(`${API_ROOT}/Applications`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
}

export async function postApplication(
  application: AddApplicationModel,
  token: string,
): Promise<Response> {
  return fetch(`${API_ROOT}/Application`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify(application),
  })
}

export async function patchApplication(
  id: string,
  newStatus: string,
  token: string,
  newStatusDate?: string,
): Promise<Response> {
  const query = newStatusDate !== undefined ? `?date=${newStatusDate}` : ''
  return fetch(`${API_ROOT}/Application/${id}/${newStatus}${query}`, {
    method: 'PATCH',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
}

export async function deleteApplication(id: string, token: string): Promise<Response> {
  return fetch(`${API_ROOT}/Application/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
}

export async function getReport(token: string): Promise<Response> {
  return fetch(`${API_ROOT}/Report`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })
}
