import request from '@/utils/request'
import type { Order } from '@/types'

export const getOrderList = (params: { page: number; pageSize: number; orderState?: number; keyword?: string }) => {
  return request.get<{ list: Order[]; total: number }>('/admin/Order', { params })
}

export const getOrderById = (id: string) => {
  return request.get<Order>(`/admin/Order/${id}`)
}

export const updateOrderStatus = (id: string, data: { orderState: number; remark?: string }) => {
  return request.put(`/admin/Order/${id}/status`, data)
}

export const shipOrder = (id: string, data: { logisticsCompany: string; logisticsNo: string }) => {
  return request.put(`/admin/Order/${id}/ship`, data)
}
