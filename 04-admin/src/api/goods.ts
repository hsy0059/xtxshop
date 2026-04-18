import request from '@/utils/request'
import type { Goods, Category } from '@/types'

export const getGoodsList = (params: { page: number; pageSize: number; keyword?: string; categoryId?: string }) => {
  return request.get<{ list: Goods[]; total: number }>('/admin/Goods', { params })
}

export const getGoodsById = (id: string) => {
  return request.get<Goods>(`/admin/Goods/${id}`)
}

export const createGoods = (data: Partial<Goods>) => {
  return request.post<Goods>('/admin/Goods', data)
}

export const updateGoods = (id: string, data: Partial<Goods>) => {
  return request.put<Goods>(`/admin/Goods/${id}`, data)
}

export const deleteGoods = (id: string) => {
  return request.delete(`/admin/Goods/${id}`)
}

export const getCategories = () => {
  return request.get<Category[]>('/admin/Category')
}

export const createCategory = (data: Partial<Category>) => {
  return request.post<Category>('/admin/Category', data)
}

export const updateCategory = (id: string, data: Partial<Category>) => {
  return request.put<Category>(`/admin/Category/${id}`, data)
}

export const deleteCategory = (id: string) => {
  return request.delete(`/admin/Category/${id}`)
}
