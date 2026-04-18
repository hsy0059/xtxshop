import request from '@/utils/request'
import type { AdminLoginResult, AdminUserInfo, ApiResponse } from '@/types'

export const login = (data: { username: string; password: string }) => {
  return request.post<AdminLoginResult>('/admin/Auth/login', data)
}

export const getAdminInfo = () => {
  return request.get<AdminUserInfo>('/admin/Auth/info')
}
