import request from '@/utils/request'
import type { Banner, HotRecommend } from '@/types'

export const getBanners = () => {
  return request.get<Banner[]>('/admin/Banner')
}

export const createBanner = (data: Partial<Banner>) => {
  return request.post<Banner>('/admin/Banner', data)
}

export const updateBanner = (id: string, data: Partial<Banner>) => {
  return request.put<Banner>(`/admin/Banner/${id}`, data)
}

export const deleteBanner = (id: string) => {
  return request.delete(`/admin/Banner/${id}`)
}

export const getHotRecommends = () => {
  return request.get<HotRecommend[]>('/admin/HotRecommend')
}

export const createHotRecommend = (data: Partial<HotRecommend>) => {
  return request.post<HotRecommend>('/admin/HotRecommend', data)
}

export const updateHotRecommend = (id: string, data: Partial<HotRecommend>) => {
  return request.put<HotRecommend>(`/admin/HotRecommend/${id}`, data)
}

export const deleteHotRecommend = (id: string) => {
  return request.delete(`/admin/HotRecommend/${id}`)
}
