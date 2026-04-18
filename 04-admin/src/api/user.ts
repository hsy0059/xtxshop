import request from '@/utils/request'
import type { User } from '@/types'

export const getUserList = (params: { page: number; pageSize: number; keyword?: string }) => {
  return request.get<{ list: User[]; total: number }>('/admin/User', { params })
}

export const getUserById = (id: string) => {
  return request.get<User>(`/admin/User/${id}`)
}

export const updateUserStatus = (id: string, status: number) => {
  return request.put(`/admin/User/${id}/status`, { status })
}
